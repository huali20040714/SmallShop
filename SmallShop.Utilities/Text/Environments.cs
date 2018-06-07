using System.Text.RegularExpressions;
using System.Web;

namespace SmallShop.Utilities
{
    public enum ClientType
    {
        Phone,
        Ipad,
        OtherMobileClient,
        Computer
    }
    
    public static class Environments
    {
        public static bool IsMobileClient
        {
            get
            {
                var userAgent = HttpContext.Current.Request.UserAgent;
                if (HttpContext.Current.Request.Browser["IsMobileDevice"] == "true" || HttpContext.Current.Request.Browser.Platform == "WinCE")
                    return true;

                return userAgent != null && Regex.IsMatch(userAgent, "AppleWebKit.*Mobile|MIDP|SymbianOS|NOKIA|SAMSUNG|LG|NEC|TCL|Alcatel|BIRD|DBTEL|Dopod|PHILIPS|HAIER|LENOVO|MOT-|Nokia|SonyEricsson|SIE-|Amoi|ZTE|android|ipod|iphone|ucweb");
            }
        }

        public static ClientType GetClientType()
        {
            var userAgent = HttpContext.Current.Request.UserAgent;
            if (IsMobileClient)
            {
                if (Regex.IsMatch(userAgent, "Android|webOS|iPhone|iPod|BlackBerry"))
                    return ClientType.Phone;
                else if (Regex.IsMatch(userAgent, "iPad"))
                    return ClientType.Ipad;
                else
                    return ClientType.OtherMobileClient;
            }

            return ClientType.Computer;
        }

        public static bool IsMobileBrowser()
        {
            //GETS THE CURRENT USER CONTEXT
            HttpContext context = HttpContext.Current;

            //FIRST TRY BUILT IN ASP.NT CHECK
            if (context.Request.Browser.IsMobileDevice)
                return true;

            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
                return true;

            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
                return true;

            //AND FINALLY CHECK THE HTTP_USER_AGENT 
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                //根据HTTP_USER_AGENT，用正则表达式来判断是否是手机在访问
                string u = context.Request.ServerVariables["HTTP_USER_AGENT"];
                Regex b = new Regex(@"android|avantgo|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\\/|plucker|pocket|psp|symbian|treo|up\\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\\-(n|u)|c55\\/|capi|ccwa|cdm\\-|cell|chtm|cldc|cmd\\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\\-s|devi|dica|dmob|do(c|p)o|ds(12|\\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\\-|_)|g1 u|g560|gene|gf\\-5|g\\-mo|go(\\.w|od)|gr(ad|un)|haie|hcit|hd\\-(m|p|t)|hei\\-|hi(pt|ta)|hp( i|ip)|hs\\-c|ht(c(\\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\\-(20|go|ma)|i230|iac( |\\-|\\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\\/)|klon|kpt |kwc\\-|kyo(c|k)|le(no|xi)|lg( g|\\/(k|l|u)|50|54|e\\-|e\\/|\\-[a-w])|libw|lynx|m1\\-w|m3ga|m50\\/|ma(te|ui|xo)|mc(01|21|ca)|m\\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\\-2|po(ck|rt|se)|prox|psio|pt\\-g|qa\\-a|qc(07|12|21|32|60|\\-[2-7]|i\\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\\-|oo|p\\-)|sdk\\/|se(c(\\-|0|1)|47|mc|nd|ri)|sgh\\-|shar|sie(\\-|m)|sk\\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\\-|v\\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\\-|tdg\\-|tel(i|m)|tim\\-|t\\-mo|to(pl|sh)|ts(70|m\\-|m3|m5)|tx\\-9|up(\\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|xda(\\-|2|g)|yas\\-|your|zeto|zte\\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                    return true;

                //Create a list of all mobile types
                string[] mobiles = new[]
                                    {
                                        "midp", "j2me", "avant", "docomo",
                                        "novarra", "palmos", "palmsource",
                                        "240x320", "opwv", "chtml",
                                        "pda", "windows ce", "mmp/",
                                        "blackberry", "mib/", "symbian",
                                        "wireless", "nokia", "hand", "mobi",
                                        "phone", "cdm", "up.b", "audio",
                                        "SIE-", "SEC-", "samsung", "HTC",
                                        "mot-", "mitsu", "sagem", "sony"
                                        , "alcatel", "lg", "eric", "vx",
                                        "NEC", "philips", "mmm", "xx",
                                        "panasonic", "sharp", "wap", "sch",
                                        "rover", "pocket", "benq", "java",
                                        "pt", "pg", "vox", "amoi",
                                        "bird", "compal", "kg", "voda",
                                        "sany", "kdd", "dbt", "sendo",
                                        "sgh", "gradi", "jb", "dddi",
                                        "moto", "iphone"
                                    };

                //Loop through each item in the list created above 
                //and check if the header contains that text
                foreach (string s in mobiles)
                {
                    if (context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
                        return true;
                }
            }

            return false;
        }
    }
}
