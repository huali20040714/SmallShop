using System;

namespace SmallShop.Utilities
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static int GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (int)ts.TotalSeconds;
        }        
    }
}
