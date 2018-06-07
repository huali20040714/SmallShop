#region Import

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

#endregion

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// 
    /// </summary>
    internal class DebugTraceListener : IListener
    {

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private readonly DebugTrace _debugTraceListener;

        #endregion

        #region Contructors

        /// <summary>
        /// 
        /// </summary>
        public DebugTraceListener(ILogFormatter formatter)
        {
            _debugTraceListener = new DebugTrace {Formatter = formatter};
        }

        #endregion

        #region IListener 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerType"></param>
        /// <returns></returns>
        public TraceListener CreateListener(LoggerListenerType loggerType)
        {
            _debugTraceListener.Filter = (LoggerListenerType.Debug & loggerType) == LoggerListenerType.Debug ? new EventTypeFilter(SourceLevels.All) : new EventTypeFilter(SourceLevels.Off);
            return _debugTraceListener;
        }

        #endregion

    }
}
