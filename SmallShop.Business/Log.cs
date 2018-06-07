using System;
using System.Web;
using SmallShop.Data;
using SmallShop.Entities;
using SmallShop.Utilities;

namespace SmallShop.BackStage.Business
{
    /// <summary>
    /// 日志类
    /// </summary>
    public static class Log
    {
        public static void WriteExceptionLog(Exception ex)
        {
            try
            {
                DataProvider.Instance.CreateExceptionLog(new ExceptionLogInfo()
                {
                    LoginName = Business.User.IsLogin ? Business.User.Current.User.LoginName : "",
                    Url = HttpContext.Current.Request.Url.ToString(),
                    Message = ex.Message,
                    StackTrace = ex.ToString(),
                    Ip = WebHelper.GetClientIp(),
                    CreateTime = DateTime.Now
                });
            }
            catch
            {
            }
        }

        public static void WriteExceptionLog(string message)
        {
            WriteExceptionLog(new Exception(message));
        }
    }
}
