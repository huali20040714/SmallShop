using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using SmallShop.Data;
using SmallShop.Entities;
using SmallShop.Utilities;

namespace SmallShop.BackStage.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var initConnectionString = DataProvider.Instance.ConnectionString;

            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = this.Context.Server.GetLastError().GetBaseException();
            Business.Log.WriteExceptionLog(ex);
            Server.ClearError();
        }

        // 为了使用Session(Business.User.Current)
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            // 写入请求参数操作日志(排除系统设置目录里面的查询相关操作)
            var absolutePath = Request.Url.AbsolutePath;
            if (WebHelper.IsAjax(Request.Headers) && !string.IsNullOrEmpty(absolutePath) && !absolutePath.StartsWith("/System/Get", StringComparison.OrdinalIgnoreCase))
            {
                var _params = WebHelper.Params.AllKeys.Aggregate(string.Empty, (current, key) => current + (key + "=" + WebHelper.Params[key] + "&"));
                if (!string.IsNullOrEmpty(_params))
                {
                    var loginName = Business.User.IsLogin ? Business.User.Current.User.LoginName : "--未登录--";
                    var operationLog = new OperationLogInfo()
                    {
                        LoginName = loginName,
                        Type = OperationType.请求参数,
                        BusinessName = absolutePath,
                        Description = $"{_params.Trim('&')}",
                        Ip = WebHelper.GetClientIp(),
                        CreateTime = DateTime.Now
                    };
                    DataProvider.Instance.CreateOperationLog(operationLog);
                }
            }
        }
    }
}
