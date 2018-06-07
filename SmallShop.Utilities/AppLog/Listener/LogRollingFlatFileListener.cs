using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// Rolling Flat File
    /// </summary>
    internal class LogRollingFlatFileListener : IListener, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RollingFlatFileTraceListener _rollingFlatFileTraceListener;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="formatter"></param>
        /// <param name="rollSizeKB"></param>
        public LogRollingFlatFileListener(string fileName, ILogFormatter formatter, int rollSizeKB)
        {
            _rollingFlatFileTraceListener = new RollingFlatFileTraceListener(fileName, string.Empty, string.Empty, formatter, rollSizeKB, Logger.RollingFileDateTimeFormat, RollFileExistsBehavior.Increment,
                RollInterval.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerType"></param>
        /// <returns></returns>
        public TraceListener CreateListener(LoggerListenerType loggerType)
        {
            _rollingFlatFileTraceListener.Filter = (LoggerListenerType.RollingFlatFile & loggerType) == LoggerListenerType.RollingFlatFile ? new EventTypeFilter(SourceLevels.All) : new EventTypeFilter(SourceLevels.Off);
            return _rollingFlatFileTraceListener;
        }

        public void Dispose()
        {
            _rollingFlatFileTraceListener.Dispose();
        }
    }
}
