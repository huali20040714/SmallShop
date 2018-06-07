using System.Configuration;

namespace SmallShop.Utilities
{
    public class ConfigHelper
    {
        public static string ConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
                {
                    var connnStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    if (!string.IsNullOrEmpty(connnStr))
                        return EncryptHelper.Decrypt(connnStr);
                }

                return null;
            }
        }

        public static string Admin
        {
            get
            {
                return "admin";
            }
        }

        public static int AdminId
        {
            get
            {
                return 1001;
            }
        }

    }
}