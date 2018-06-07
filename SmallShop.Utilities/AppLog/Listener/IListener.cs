#region Import

using System.Diagnostics;

#endregion

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IListener
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultLoggerType">default filter</param>
        /// <returns></returns>
        TraceListener CreateListener(LoggerListenerType defaultLoggerType);
    }
}
