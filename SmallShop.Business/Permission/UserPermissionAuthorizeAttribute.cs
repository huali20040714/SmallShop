using System.Web.Mvc;
using SmallShop.Utilities;

namespace SmallShop.BackStage.Business
{
    public class UserPermissionAuthorizeAttribute : ActionFilterAttribute
    {
        private PermissionType[] _permissions;

        public UserPermissionAuthorizeAttribute(params PermissionType[] permissions)
        {
            _permissions = permissions;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!User.IsLogin)
            {
                throw new UnloginException("未登录");
            }

            foreach (var p in _permissions)
            {
                if (User.Current.Permissions.Contains(string.Format("|{0}|", (int)p)))
                {
                    return;
                }
            }

            throw new UnAuthorizeException(string.Format("没有{0}权限", string.Join("|", _permissions)));
        }
    }
}
