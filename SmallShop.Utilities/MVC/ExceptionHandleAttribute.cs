using System.Web.Mvc;

namespace SmallShop.Utilities
{
    public class ExceptionHandleAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //是否是（ajax请求和mobile请求）Result响应
            var isResultResponse = WebHelper.IsAjax(filterContext.HttpContext.Request.Headers) || WebHelper.IsApp(filterContext.HttpContext.Request.Headers);
            if (filterContext.Exception is UnloginException)
            {
                if (isResultResponse)
                {
                    var json = JsonHelper.ToJson(new Result
                    {
                        HasException = true,
                        Value = "AjaxRequestLoginTimeout",
                        Message = "AjaxRequestLoginTimeout",
                    });
                    filterContext.HttpContext.Response.Write(json);
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("~/Login/Login");
                }
            }
            else if (filterContext.Exception is UnAuthorizeException)
            {
                if (isResultResponse)
                {
                    var json = JsonHelper.ToJson(new Result { Success = false, HasException = true, Message = filterContext.Exception.Message });
                    filterContext.HttpContext.Response.Write(json);
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    var error = filterContext.Exception.Message;
                    error = error.Replace("\r", "").Replace("\n", "");
                    if (error.Length > 256)
                        error = "无访问权限";

                    filterContext.HttpContext.Response.Redirect("~/Home/Error?error=" + error);
                }
            }
            else
            {
                if (isResultResponse)
                {
                    var json = JsonHelper.ToJson(new Result { Success = false, HasException = true, Message = filterContext.Exception.Message });
                    filterContext.HttpContext.Response.Write(json);
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    var error = filterContext.Exception.Message;
                    error = error.Replace("\r", "").Replace("\n", "");
                    if (error.Length > 256)
                        error = "错误异常";

                    filterContext.HttpContext.Response.Redirect("~/Home/Error?error=" + error);
                }
            }
        }
    }
}
