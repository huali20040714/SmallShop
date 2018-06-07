using System;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;

namespace SmallShop.Utilities
{
    public class LoadHttpBase
    {
        #region Field

        private CookieContainer myCookieContainer = new CookieContainer();
        public CookieContainer MyCookieContainer
        {
            get { return myCookieContainer; }
            set { myCookieContainer = value; }
        }
        public void ClearMyCookieContainer() { AllowSetCookie = false; myCookieContainer = new CookieContainer(); Encode = null; }
        private string securityFile = string.Empty;

        private Encoding encode;
        public Encoding Encode
        {
            get { return encode; }
            set { encode = value; }
        }

        private int timeout = 45000;
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        public string SecurityFile
        {
            get { return securityFile; }
            set { securityFile = value; }
        }

        private bool allowAutoRedirect = true;
        public bool AllowAutoRedirect
        {
            get { return allowAutoRedirect; }
            set { allowAutoRedirect = value; }
        }

        public bool AllowSetCookie { get; set; }

        /// <summary>
        /// 本地发出请求的ip地址(一网卡对应多ip的情况)，自己自定IP
        /// </summary>
        public string LocalAddress { get; set; }

        public bool IsAjax { get; set; }

        public Dictionary<string, string> SelfHeader { get; set; }

        public Uri ResponseUri { get; set; }

        #endregion

        #region Method

        public Bitmap GetImage(string url)
        {
            return GetImage(url, string.Empty);
        }
        public Bitmap GetImage(string url, string refer)
        {
            System.Drawing.Bitmap bmp = null;
            try
            {
                HttpWebRequest myHttpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                myHttpWebRequest.Timeout = timeout;
                myHttpWebRequest.Method = "GET";
                SetRequestHeaders(myHttpWebRequest, refer, url.Substring(0, 5));

                HttpWebResponse myHttpWebResponse = myHttpWebRequest.GetResponse() as HttpWebResponse;
                Stream myResponseStream = myHttpWebResponse.GetResponseStream();
                bmp = new System.Drawing.Bitmap(myResponseStream);

                //保存返回cookie
                SaveResponseCookie(myHttpWebRequest, myHttpWebResponse);

                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
                //SqlDataProvider.CreateLog(1, "托水服务:图片抓取" + ex.ToString());
                return null;
            }
            return bmp;
        }

        public string GetData(string url)
        {
            return GetData(url, string.Empty, Encode);
        }
        public string GetData(string url, Encoding ec)
        {
            return GetData(url, string.Empty, ec);
        }
        public string GetData(string url, string refererurl)
        {
            return GetData(url, refererurl, Encode);
        }
        public string GetData(string url, string refererurl, Encoding ec)
        {
            Encode = ec;
            string receiveData = string.Empty;
            try
            {
                HttpWebRequest myHttpWebRequest = WebRequest.Create(url) as HttpWebRequest;

                myHttpWebRequest.Timeout = timeout;
                myHttpWebRequest.Method = "GET";
                SetRequestHeaders(myHttpWebRequest, refererurl, url.Substring(0, 5));

                HttpWebResponse myHttpWebResponse = myHttpWebRequest.GetResponse() as HttpWebResponse;
                if (AllowAutoRedirect)
                    this.ResponseUri = myHttpWebResponse.ResponseUri;
                myHttpWebResponse.Cookies = myCookieContainer.GetCookies(myHttpWebRequest.RequestUri);
                Stream myResponseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader;
                if (Encode != null)
                {
                    myStreamReader = new StreamReader(myResponseStream, Encode);
                }
                else
                {
                    myStreamReader = new StreamReader(myResponseStream, Encoding.Default);
                }
                receiveData = myStreamReader.ReadToEnd();

                //保存返回cookie
                SaveResponseCookie(myHttpWebRequest, myHttpWebResponse);

                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                return "操作超时";
            }
            return receiveData;
        }

        public string PostData(string url, string indata)
        {
            return PostData(url, string.Empty, indata);
        }
        public string PostData(string url, string refererurl, string indata)
        {
            string receiveData = string.Empty;
            try
            {
                HttpWebRequest myHttpWebRequest = WebRequest.Create(url) as HttpWebRequest;

                myHttpWebRequest.Timeout = timeout;
                myHttpWebRequest.Method = "POST";
                SetRequestHeaders(myHttpWebRequest, refererurl, url.Substring(0, 5));

                var buff = (Encode ?? Encoding.Default).GetBytes(indata);
                myHttpWebRequest.ContentLength = buff.Length;
                Stream myRequestStream = myHttpWebRequest.GetRequestStream();
                myRequestStream.Write(buff, 0, buff.Length);
                myRequestStream.Close();

                HttpWebResponse myHttpWebResponse = myHttpWebRequest.GetResponse() as HttpWebResponse;
                if (AllowAutoRedirect)
                    this.ResponseUri = myHttpWebResponse.ResponseUri;
                myHttpWebResponse.Cookies = myCookieContainer.GetCookies(myHttpWebRequest.RequestUri);
                Stream myResponseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader;
                if (Encode != null)
                {
                    myStreamReader = new StreamReader(myResponseStream, Encode);
                }
                else
                {
                    myStreamReader = new StreamReader(myResponseStream, Encoding.Default);
                }
                receiveData = myStreamReader.ReadToEnd();

                //保存返回cookie
                SaveResponseCookie(myHttpWebRequest, myHttpWebResponse);

                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
                return "操作超时";
            }
            return receiveData;
        }

        public string EncodeConvert(string ecContent, Encoding encode, int codepage)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(ecContent);
            bytes = System.Text.Encoding.Convert(encode, System.Text.Encoding.GetEncoding(codepage), bytes);
            return System.Text.Encoding.GetEncoding(codepage).GetString(bytes);

        }

        #endregion

        #region HttpHeader

        private void SetRequestHeaders(HttpWebRequest myRequest, string refer, string protocol)
        {
            if (IsAjax)
            {
                if (Encode == null)
                    myRequest.ContentType = "application/json";
                else
                    myRequest.ContentType = $"application/json; charset={Encode.BodyName}";
                myRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }
            else
            {
                if (Encode == null)
                    myRequest.ContentType = "application/x-www-form-urlencoded";
                else
                    myRequest.ContentType = $"application/x-www-form-urlencoded; charset={Encode.BodyName}";
            }
            myRequest.KeepAlive = true;
            myRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; CIBA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET4.0C; .NET4.0E)";
            myRequest.Headers.Add("Cache-Control", "no-cache");
            myRequest.Headers.Add("Pragma", "no-cache");
            myRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
            myRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            myRequest.Credentials = CredentialCache.DefaultCredentials;
            myRequest.UnsafeAuthenticatedConnectionSharing = true;
            myRequest.UseDefaultCredentials = true;
            myRequest.AllowWriteStreamBuffering = false;
            myRequest.AllowAutoRedirect = this.AllowAutoRedirect;
            myRequest.CookieContainer = myCookieContainer;
            myRequest.Referer = refer;
            myRequest.ServicePoint.Expect100Continue = false;

            if (SelfHeader != null && SelfHeader.Count > 0)
            {
                foreach (var item in SelfHeader)
                    myRequest.Headers.Add(item.Key, item.Value);
            }

            if (ServicePointManager.DefaultConnectionLimit == 2)
            {      //连接并发数 默认为2
                ServicePointManager.DefaultConnectionLimit = 512;
            }
            if (!string.IsNullOrEmpty(LocalAddress))
            {                  //指定出口ip地址
                myRequest.ServicePoint.BindIPEndPointDelegate = new BindIPEndPoint(BindIPEndPointCallback);
            }

            if (protocol.ToLower() == "https")
            {
                try
                {
                    //ServicePointManager.CertificatePolicy = new CertPolicy();
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                    if (securityFile != string.Empty)
                    {
                        X509Certificate Cert = X509Certificate.CreateFromCertFile(securityFile);
                        myRequest.ClientCertificates.Add(Cert);
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 本地发出请求的ip地址，和端口
        /// </summary>
        /// <param name="servicePoint"></param>
        /// <param name="remoteEndPoint"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        private IPEndPoint BindIPEndPointCallback(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
        {
            if (retryCount < 3 && !string.IsNullOrEmpty(LocalAddress))
                return new IPEndPoint(IPAddress.Parse(LocalAddress), 9099);     //"192.168.1.118"
            else
                return new IPEndPoint(IPAddress.Any, 0);                        //0表示系统自动分配的一个端口
        }

        /// <summary>
        /// https调用的地方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //if (sslPolicyErrors == SslPolicyErrors.None) {
            //  return true;
            //}

            //Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            //// Do not allow this client to communicate with unauthenticated servers.
            //return false;

            return true;
        }


        //保存返回cookie
        private void SaveResponseCookie(HttpWebRequest myHttpWebRequest, HttpWebResponse myHttpWebResponse)
        {

            if (AllowSetCookie)
            {

                if (Array.Exists<string>(myHttpWebResponse.Headers.AllKeys, (key) => { return key == "Set-Cookie"; }))
                {
                    myCookieContainer = new CookieContainer();
                    string[] cookielist = myHttpWebResponse.Headers.GetValues("Set-Cookie");
                    for (int i = 0; i < cookielist.Length; i++)
                    {
                        string[] cookiearray = cookielist[i].Split(';');
                        var cookiestr = cookiearray[0];
                        int index = cookiestr.IndexOf('=');
                        if (index >= 0)
                        {
                            string name = cookiestr.Substring(0, index);
                            string value = cookiestr.Substring(index + 1);
                            myCookieContainer.Add(new Cookie(name, value, "/", myHttpWebRequest.RequestUri.Host));
                        }
                    }
                }

            }
        }

        #endregion

        #region SecurityClass
        class CertPolicy : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
            {
                return true;
            }
        }
        #endregion
    }
}
