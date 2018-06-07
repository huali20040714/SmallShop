using System.Web;
using System.Collections.Specialized;
using System.Net;
using System.Linq;
using System;

namespace SmallShop.Utilities
{
    /// <summary>
    /// 表示Web处理的助手类。
    /// </summary>
    public static class WebHelper
    {
        /// <summary>
        /// 获取请求参数
        /// </summary>
        public static NameValueCollection Params
        {
            get
            {
                return GetRequestParams();
            }
        }

        /// <summary>
        /// 获取本次Web请求参数的容器，利用它可以取得本次请求的相关参数。
        /// </summary>
        private static NameValueCollection GetRequestParams()
        {
            if (HttpContext.Current.Request.RequestType == "POST")
                return HttpContext.Current.Request.Form;

            return HttpContext.Current.Request.QueryString;
        }

        /// <summary>
        /// 返回当前用户ip（有些是通过加速器的）
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            string ip = string.Empty;
            //代理加速器
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Headers["Cdn-Src-Ip"]))               // 代理加速
                ip = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X-Forwarded-For"]))      // 代理加速
                ip = HttpContext.Current.Request.Headers["X-Forwarded-For"];
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_VIA"]) && !string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.Headers["REMOTE_ADDR"]))
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            else
                ip = HttpContext.Current.Request.UserHostAddress;

            return string.IsNullOrEmpty(ip) ? "0.0.0.0" : ip.ToString();
        }

        public static IPAddress DnsToIp(string address)
        {
            IPAddress ip;
            if (address == "localhost") return IPAddress.Parse("127.0.0.1");
            if (IPAddress.TryParse(address, out ip))
            {
                return ip;
            }
            else
            {
                try
                {
                    var ips = Dns.GetHostAddresses(address);
                    if (ips.Length > 0)
                    {
                        ip = IPAddress.Parse(ips[0].ToString());
                    }
                }
                catch { }

                if (ip != null)
                {
                    return ip;
                }
                else
                {
                    DnsToIp(address);
                }

            }

            return ip;
        }

        public static void ResponseWrite(HttpResponseBase response, string message)
        {
            response.Write(message);
            response.Write("<script type='text/javascript'>document.body.scrollTop = 1000000;</script>");
            response.Flush();
        }

        public static bool IsAjax(NameValueCollection headers)
        {
            return headers.AllKeys.Contains("isAjax", StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsApp(NameValueCollection headers)
        {
            return headers.AllKeys.Contains("isApp", StringComparer.OrdinalIgnoreCase);
        }
    }
}
