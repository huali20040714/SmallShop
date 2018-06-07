using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SmallShop.BackStage.Business;
using SmallShop.BackStage.Web.Models;
using SmallShop.Data;
using SmallShop.Entities;
using SmallShop.Utilities;
using SmallShop.Utilities.DbProxy;

namespace SmallShop.BackStage.Web.Controllers
{
    [LoginValidate]
    public class HomeController : Controller
    {
        private static DataProvider provider = DataProvider.Instance;

        #region 首页或错误显示页

        public ActionResult Index()
        {
            BackStageLocation.Path = "Index";
            var agent = provider.GetUser(Business.User.Current.MainUser.Id);
            var user = provider.GetUser(Business.User.Current.User.Id);
            user.Balance = agent.Balance;

            return View(user);
        }

        public ActionResult Error()
        {
            ViewBag.Message = WebHelper.Params["error"] ?? string.Empty;

            return View();
        }

        #endregion
        
        #region 修改密码

        #region 修改自己的密码

        [UserPermissionAuthorize(PermissionType.控制台_修改密码)]
        public ActionResult UpdatePassword(int? Id)
        {
            BackStageLocation.Path = "Index/UpdatePassword";

            return View(Id);
        }

        [UserPermissionAuthorize(PermissionType.控制台_修改密码)]
        public JsonResult SaveUpdatePassword()
        {
            var oldPassword = WebHelper.Params["OldPassword"];
            var newPassword = WebHelper.Params["NewPassword"];
            var rePassword = WebHelper.Params["RePassword"];
            if (string.IsNullOrEmpty(oldPassword))
                return Json(Result.Error("原始密码不能为空"));

            if (string.IsNullOrEmpty(newPassword))
                return Json(Result.Error("新密码不能为空"));

            if (newPassword.Length < 6)
                return Json(Result.Error("新密码长度不能小于6位"));

            if (newPassword != rePassword)
                return Json(Result.Error("两次输入的密码不一致"));

            var user = Business.User.Current.User;
            if (user.Password != EncryptHelper.MD5Encode(oldPassword).ToLower())
                return Json(Result.Error("所输入原密码错误"));

            var newPasswordMd5 = EncryptHelper.MD5Encode(newPassword).ToLower();
            provider.UpdateUser($"{UserCols.Id} = {user.Id}", $"{UserCols.Password} = {newPasswordMd5.ToSafeSql()}");
            user.Password = newPasswordMd5;

            // 写入密码修改操作日志
            var operationLog = new OperationLogInfo()
            {
                LoginName = user.LoginName,
                Type = OperationType.修改,
                BusinessName = "本人修改密码",
                Description = $"{oldPassword}->{newPassword}",
                Ip = WebHelper.GetClientIp(),
                CreateTime = DateTime.Now
            };
            provider.CreateOperationLog(operationLog);

            return Json(Result.Default);
        }

        #endregion

        #region 修改下级的密码

        [UserPermissionAuthorize(PermissionType.用户管理_用户管理, PermissionType.用户管理_代理管理, PermissionType.用户管理_会员管理, PermissionType.用户管理_子帐号管理)]
        public ActionResult ChangePassword(int id)
        {
            return View(id);
        }

        [UserPermissionAuthorize(PermissionType.用户管理_用户管理, PermissionType.用户管理_代理管理, PermissionType.用户管理_会员管理, PermissionType.用户管理_子帐号管理)]
        public JsonResult SavePassword(UserInfo user)
        {
            if (user == null || string.IsNullOrEmpty(user.Password))
                return Json(Result.Error("密码不能为空"));

            var password = user.Password;           //新密码
            user = provider.GetUser(user.Id);
            if (password.Length < 6)
                return Json(Result.Error("密码长度不能小于6位"));

            var rePassword = WebHelper.Params["RePassword"];
            if (password != rePassword)
                return Json(Result.Error("两次输入的密码不一致"));
       
            var passwordMd5 = EncryptHelper.MD5Encode(password).ToLower();
            provider.UpdateUser($"{UserCols.Id} = {user.Id}", $"{UserCols.Password} = {passwordMd5.ToSafeSql()}");

            // 写入密码修改操作日志
            var operationLog = new OperationLogInfo()
            {
                LoginName = Business.User.Current.User.LoginName,
                Type = OperationType.修改,
                BusinessName = "上级代改密码",
                Description = $"账号[{user.LoginName}]密码被修改为：{password}",
                Ip = WebHelper.GetClientIp(),
                CreateTime = DateTime.Now
            };
            provider.CreateOperationLog(operationLog);

            return Json(Result.Default);
        }

        #endregion

        #endregion
        
        #region 用户管理

        [UserPermissionAuthorize(PermissionType.用户管理_用户管理)]
        public new ActionResult User()
        {
            BackStageLocation.Path = "Index/User";

            return View();
        }

        [UserPermissionAuthorize(PermissionType.用户管理_用户管理)]
        public JsonResult GetUsers()
        {
            var ret = Result.Default;
            var queryParams = new QueryParams(WebHelper.Params);
            if (string.IsNullOrEmpty(queryParams.OrderBy))
            {
                queryParams.OrderBy = $"{UserCols.Id}";
                queryParams.IsAsc = false;
            }
            var isVague = 0.TryParse(WebHelper.Params["IsVague"]) == 1; //是否为模糊查询
            queryParams.Table = $"{EntityType.User}";
            var dateType = "CreateTime";
            switch (WebHelper.Params["DateType"])
            {
                case "CreateTime":
                    dateType = "CreateTime";
                    break;
                case "AvailableTime":
                    dateType = "AvailableTime";
                    break;
                case "XiMaJieSuanTime":
                    dateType = "XiMaJieSuanTime";
                    break;
            }
            queryParams.WhereClause = dateType.ToSaftBetweenDateSql(WebHelper.Params["dtStart"], WebHelper.Params["dtEnd"], true);
            if (Business.User.Current.User.Type == UserType.管理子帐号)
                queryParams.WhereClause += $" and {UserCols.Id} != {Business.User.Current.User.Id}";
           
            var userType = (-1).TryParse(WebHelper.Params["UserType"]);
            if (userType > 0)
                queryParams.WhereClause += $" and [{UserCols.Type}] = {userType}";

            if (!string.IsNullOrEmpty(WebHelper.Params["LoginName"]))
                queryParams.WhereClause += $" and {UserCols.LoginName} {WebHelper.Params["LoginName"].ToSaftVagueSql(isVague)}";
          
            if (!string.IsNullOrEmpty(WebHelper.Params["PartnerId"]))
                queryParams.WhereClause += $" and Id in(select UserId from UserCashPartner where PartnerId = {WebHelper.Params["PartnerId"].ToSafeSql()})";

            var isPartner = 0.TryParse(WebHelper.Params["IsPartner"]);
            if (isPartner == 1)
                queryParams.WhereClause += $" and Id in(select UserId from UserCashPartner)";
            else if (isPartner == 2)
                queryParams.WhereClause += $" and Id not in(select UserId from UserCashPartner)";

            var list = DbProxy.GetPageRecords<UserInfo>(queryParams);
            ret.Value = this.RenderView("User.Partial", list);
            ret.Tag = queryParams.RecordCount;

            return Json(ret);
        }

        [UserTypeAuthorize(UserType.后台管理 | UserType.管理子帐号)]
        [UserPermissionAuthorize(PermissionType.用户管理_用户管理)]
        public ActionResult UserRoleEditor(int id)
        {
            var model = new UserRoleEditorModel
            {
                UserId = id,
                Roles = provider.GetRoles(),
                RoleUser = provider.GetRoleUsers($"{RoleUserCols.UserId}={id}")
            };

            return View(model);
        }

        #endregion

        #region 代理,会员 管理
        
        
        #endregion

        #region  子帐号管理

        [UserTypeAuthorize(UserType.后台管理 )]
        [UserPermissionAuthorize(PermissionType.用户管理_子帐号管理)]
        public ActionResult SubAccount()
        {
            BackStageLocation.Path = "Index/SubAccount";
            return View();
        }

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.用户管理_子帐号管理)]
        public JsonResult GetSubAccounts()
        {
            var ret = Result.Default;
            var queryParams = new QueryParams(WebHelper.Params);
            if (string.IsNullOrEmpty(queryParams.OrderBy))
            {
                queryParams.OrderBy = $"{UserCols.Id}";
                queryParams.IsAsc = false;
            }
            var isVague = 0.TryParse(WebHelper.Params["IsVague"]) == 1; //是否为模糊查询
            queryParams.Table = $"{EntityType.User}";
            queryParams.WhereClause = $"{UserCols.ParentId} = '{Business.User.Current.User.Id}'";
            queryParams.WhereClause += " and " + "CreateTime".ToSaftBetweenDateSql(WebHelper.Params["dtStart"], WebHelper.Params["dtEnd"], false);
            if (!string.IsNullOrEmpty(WebHelper.Params["LoginName"]))
                queryParams.WhereClause += $" and {UserCols.LoginName} {WebHelper.Params["LoginName"].ToSaftVagueSql(isVague)}";
            
            switch (Business.User.Current.User.Type)
            {
                case UserType.后台管理:
                    queryParams.WhereClause += $" and [{UserCols.Type}] = {(int)UserType.管理子帐号}";
                    break;
             
                default:
                    ret.Tag = 0;
                    return Json(ret);
            }

            var list = DbProxy.GetPageRecords<UserInfo>(queryParams);
            ret.Value = this.RenderView("SubAccount.Partial", list);
            ret.Tag = queryParams.RecordCount;

            return Json(ret);
        }
        
        #endregion
        
    }
}