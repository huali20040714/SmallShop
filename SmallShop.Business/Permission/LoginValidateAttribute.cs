using SmallShop.Utilities;
using System.Web.Mvc;

namespace SmallShop.BackStage.Business
{
    public class LoginValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!User.IsLogin)
            {
                throw new UnloginException();
            }
        }
    }
}
