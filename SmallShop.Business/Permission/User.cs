using System;
using System.Web;
using System.Linq;
using SmallShop.Utilities;
using SmallShop.Data;
using SmallShop.Entities;

namespace SmallShop.BackStage.Business
{
    public class User
    {
        private static DataProvider provider = DataProvider.Instance;
        private const string UserLoginSessionKey = "USER_LOGIN_SESSION_KEY";

        public static bool IsLogin
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return false;
                }
                if (HttpContext.Current.Session[UserLoginSessionKey] != null)
                {
                    return true;
                }
                return false;
            }
        }

        public static CurrentUserInfo Current
        {
            get
            {
                if (!IsLogin)
                {
                    throw new UnloginException();
                }
                var info = HttpContext.Current.Session[UserLoginSessionKey] as CurrentUserInfo;
                return info;
            }
            private set
            {
                HttpContext.Current.Session[UserLoginSessionKey] = value;
            }
        }

        public static Result<object> Login(string loginName, string password)
        {
            var users = provider.GetUsers($"{UserCols.LoginName} = {loginName.ToSafeSql()}");
            if (users.Count == 0)
                return Result.Error("用户不存在!");

            var user = users[0];
            var agent = user;

            switch (user.Type)
            {
                case UserType.管理子帐号:
                    agent = provider.GetUser(user.ParentId);
                    break;
            }
            
            if (user.Type == UserType.会员帐号)
                return Result.Error("非法帐号类型!");
            
            if (EncryptHelper.MD5Encode(password).ToLower() != user.Password)
                return Result.Error("密码错误!");
            
            //创建当前用户,权限、目录树
            var currentUser = new CurrentUserInfo()
            {
                User = user,
                Permissions = GetPermissions(user),
                MainUser = agent,
            };

            var funTree = FunTreeHelper.FunTree.BuildSubTree(currentUser.Permissions);
            currentUser.FunTreeHtml = funTree.RenderHtmlLeftMenu(funTree.SubNodes);

            //初始化Session
            Current = currentUser;

            //登录成功
            return Result.Default;
        }

        public static void Logout()
        {
            Current = null;
            HttpContext.Current.Session.Abandon();
        }

        private static string GetPermissions(UserInfo user)
        {
            var permissions = string.Empty;
            var roleUserPermissions = provider.GetRoleUserPermissions(user.Id);
            foreach (var role in roleUserPermissions)
                permissions += role.Permissions ?? string.Empty;

            if (user.LoginName.Equals(ConfigHelper.Admin, StringComparison.OrdinalIgnoreCase) && !permissions.Contains(string.Format("|{0}|", (int)PermissionType.系统管理_权限设置)))
            {
                permissions = permissions.Replace("|" + (int)PermissionType.系统管理 + "|", "|");
                permissions += (int)PermissionType.系统管理 + "|" + (int)PermissionType.系统管理_权限设置 + "|";
            }
            permissions = permissions.Replace("||", "|");
            if (!permissions.StartsWith("|"))
                permissions = "|" + permissions;

            return permissions;
        }
    }
}
