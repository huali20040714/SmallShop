using SmallShop.Entities;
using SmallShop.Utilities;
using System.Web.Mvc;

namespace SmallShop.BackStage.Business
{
    public class UserTypeAuthorizeAttribute : ActionFilterAttribute
    {
        private UserType _userType;

        public UserTypeAuthorizeAttribute(UserType userType)
        {
            _userType = userType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!User.IsLogin)
            {
                throw new UnloginException("未登录");
            }
            if (!_userType.HasFlag(User.Current.User.Type))
            {
                throw new UnAuthorizeException(string.Format("没有{0}权限", _userType.ToString()));
            }
        }
    }
}
