using System;

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// 
    /// </summary>
    public static class LogTime
    {
        private static DateTime? _lastTime;
        private static DateTime? _srcTime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="info"></param>
        public static void Begin(string logName, string info = null)
        {
            _srcTime = DateTime.Now;
            _lastTime = _srcTime;
            Logger.Info(logName, info == null
                                     ? string.Format("====> LogTime start.")
                                     : string.Format("====> LogTime start. {0}", info));
        }

        /// <summary>
        /// 。
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="info"></param>
        public static void Log(string logName, string info = null)
        {
            if (_lastTime == null)
            {
                _srcTime = DateTime.Now;
                _lastTime = _srcTime;
                Logger.Info(logName, string.Format("====> _lastTime is null. init time."));
                return;
            }

            DateTime now = DateTime.Now;
            TimeSpan? lastSpan = now - _lastTime;
            TimeSpan? allSpan = now - _srcTime;

            if (allSpan == null)
            {
                Logger.Info(logName, string.Format("====> timeSpan is null."));
                return;
            }

            _lastTime = now;
            double consume = lastSpan.Value.TotalMilliseconds;
            double allConsume = allSpan.Value.TotalMilliseconds;

            Logger.Info(logName, info == null
                                     ? string.Format("====> {0} / {1}", (int)consume, (int)allConsume)
                                     : string.Format("====> {0} / {1}  {2}", (int)consume, (int)allConsume, info));
        }
    }
}