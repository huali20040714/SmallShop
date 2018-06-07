using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class LogCustomWriter : IDisposable
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public LogCustomWriter()
        {
            _loggerType = LoggerListenerType.RollingFlatFile;
            var nonExistantLogSource = new LogSource("Empty");
            _logWriterImpl = new LogWriterImpl(new List<ILogFilter>(), _traceSources, nonExistantLogSource, "");
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private readonly LogWriterImpl _logWriterImpl;

        /// <summary>
        /// 
        /// </summary>
        private readonly object _safeObj = new object();

        /// <summary>
        /// 
        /// </summary>
        private readonly IDictionary<string, LogSource> _traceSources = new Dictionary<string, LogSource>();

        /// <summary>
        /// 
        /// </summary>
        private LoggerListenerType _loggerType;

        #endregion Fields

        #region Properties Public

        public int RollSizeKB
        {
            set;
            get;
        }

        public string SavePath
        {
            set;
            get;
        }

        #endregion Properties Public

        #region Methods Public

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="logFormatType"></param>
        public void AddLogSource(string fileName, LogFormatType logFormatType)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName", "Log FileName is Null");

            if (!_traceSources.ContainsKey(fileName))
            {
                lock (_safeObj)
                {
                    if (!_traceSources.ContainsKey(fileName))
                    {
                        var logRollingListener = new LogRollingFlatFileListener(Path.Combine(SavePath, fileName + ".Log"), LogTemplate.GetRollingFlatFileFormatter(logFormatType), RollSizeKB).CreateListener(_loggerType);
                        var debugTraceListener = new DebugTraceListener(LogTemplate.GetDebugTraceFormatter(logFormatType)).CreateListener(_loggerType);
                        var mainLogSource = new LogSource("MainLogSource", SourceLevels.All);
                        mainLogSource.Listeners.Clear();
                        mainLogSource.Listeners.Add(logRollingListener);
                        mainLogSource.Listeners.Add(debugTraceListener);
                        _traceSources.Add(fileName, mainLogSource);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerType"></param>
        public void ChangeLoggerListenerType(LoggerListenerType loggerType)
        {
            _loggerType = loggerType;
            foreach (var traceSource in _traceSources.Values)
            {
                if (loggerType == LoggerListenerType.All)
                {
                    foreach (var traceListener in traceSource.Listeners)
                    {
                        if (traceListener.Filter is EventTypeFilter)
                            ((EventTypeFilter)traceListener.Filter).EventType = SourceLevels.All;
                    }
                }
                else if ((LoggerListenerType.Debug & loggerType) == LoggerListenerType.Debug)
                {
                    var debugTraceListener = traceSource.Listeners.OfType<DebugTrace>().First();
                    if (debugTraceListener.Filter is EventTypeFilter)
                        ((EventTypeFilter)debugTraceListener.Filter).EventType = SourceLevels.All;
                }
                else if ((LoggerListenerType.RollingFlatFile & loggerType) == LoggerListenerType.RollingFlatFile)
                {
                    var rollingFlatFileTraceListener = traceSource.Listeners.OfType<RollingFlatFileTraceListener>().First();
                    if (rollingFlatFileTraceListener.Filter is EventTypeFilter)
                        ((EventTypeFilter)rollingFlatFileTraceListener.Filter).EventType = SourceLevels.All;
                }
            }
        }

        public void Dispose()
        {
            _logWriterImpl.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logEntry"></param>
        /// <param name="fileNameCategory"></param>
        /// <param name="dic"></param>
        public void WriteLog(LogEntry logEntry, string fileNameCategory, Dictionary<string, object> dic)
        {
            _logWriterImpl.Write(logEntry, fileNameCategory, dic);
        }

        #endregion Methods Public
    }
}