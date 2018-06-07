using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using SmallShop.Utilities;
using SmallShop.Data;
using SmallShop.Entities;

namespace SmallShop.BackStage.Web.Controllers
{
    public class LoginController : Controller
    {
        private static readonly string login_verify_code_image = "LoginVerifyCodeImage";
        private static DataProvider provider = DataProvider.Instance;

        public ActionResult LoadingDialog()
        {
            return View();
        }

        //心跳保持
        public void KeepAlive()
        {
            Response.Write("keepAlive_" + System.Environment.TickCount.ToString());
            Response.End();
        }

        public ActionResult Login()
        {
#if DEBUG
            ViewBag.LoginName = ConfigHelper.Admin;
            ViewBag.Password = "123123";
            ViewBag.VerifyCode = "1234";            
#endif
            Session[login_verify_code_image] = "";
            return View();
        }

        public JsonResult CheckLogin()
        {
            var ret = Result.Default;
            string loginName = WebHelper.Params["loginName"];
            string password = WebHelper.Params["password"];
            if (!string.IsNullOrEmpty(loginName) && !string.IsNullOrEmpty(password))
            {
                if (!ValidateVerifyCode())
                {
                    Session[login_verify_code_image] = null;
                    ret = Result.Error("验证码不正确");
                }
                else
                {
                    ret = Business.User.Login(loginName, password);
                    if (ret.Success)
                        ret.Value = $"/Home/Index";

                    // 插入登录日志
                    var operationLog = new OperationLogInfo()
                    {
                        LoginName = loginName,
                        Type = OperationType.登录,
                        BusinessName = ret.Success ? "登录成功" : "登录失败",
                        Description = ret.Success ? $"UserType={Business.User.Current.User.Type}" : $"帐号：{loginName}，密码：{password}，{ret.Message}",
                        Ip = WebHelper.GetClientIp(),
                        CreateTime = DateTime.Now
                    };
                    provider.CreateOperationLog(operationLog);
                }
            }
            else
            {
                ret = Result.Error("帐号密码不能为空");
            }

            return Json(ret);
        }

        private bool ValidateVerifyCode()
        {
            string code = WebHelper.Params["verifyCode"];
            if (Session[login_verify_code_image] == null)
                return false;

            if (string.IsNullOrEmpty(code))
                return false;

            return String.Equals(code, Session[login_verify_code_image].ToString(), StringComparison.CurrentCultureIgnoreCase);
        }

        public void VerifyCodeImage()
        {
            var vCode = new ValidationCode();
            try
            {
                vCode.FontSize = 15;
#if DEBUG
                vCode.Code = "1234";
#endif
                var image = vCode.NextImage(4, HatchStyle.DarkVertical, false);
                Session[login_verify_code_image] = vCode.Code;

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Gif);
                    Response.ClearContent();
                    Response.ContentType = "image/Gif";
                    Response.BinaryWrite(ms.ToArray());
                    image.Dispose();
                    Response.End();
                }
            }
            catch
            {
            }
        }

        //浏览器版本问题
        public ActionResult LowerBrowser()
        {
            ViewBag.ErrorMessage = "500!当前浏览器版本太低,请使用更高版本！";

            return View("LoginErrorCodePartial");
        }

        public ActionResult Logout()
        {
            Business.User.Logout();
            return RedirectToAction("Login");
        }

        public ActionResult Test()
        {
            ViewBag.ErrorMessage = Business.CacheHelper.Instance.GetStatistic();

            return View("LoginErrorCodePartial");
        }
    }
}