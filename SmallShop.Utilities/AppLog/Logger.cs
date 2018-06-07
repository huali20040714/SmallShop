using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// Provide log function, base on the enterprise library log module。
    /// </summary>
    public static class Logger
    {
        #region Constructors

        static Logger()
        {
            LogCustomWriter.SavePath = AppDomain.CurrentDomain.BaseDirectory + "Logs";
            LogCustomWriter.RollSizeKB = 10000;
            _initManagedThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        #endregion Constructors

        #region Fields

        private static readonly LogCustomWriter LogCustomWriter = new LogCustomWriter();

        private static volatile bool _isEnabled = true;

        private static readonly int _initManagedThreadId;

        /// <summary>
        /// 
        /// </summary>
        public const string RollingFileDateTimeFormat = "yyyy-MM-dd HH-mm-ss";

        private static LogFormatType _logformatType = LogFormatType.Default;

        #endregion Fields

        #region Properties Public

        /// <summary>
        /// 
        /// </summary>
        public static string RootLogPath
        {
            get { return LogCustomWriter.SavePath; }
        }

        /// <summary>
        /// Default log Name, "log".
        /// </summary>
        public static string DefaultLogName
        {
            get { return "log"; }
        }

        /// <summary>
        /// Is enabled to write log. ture will normal write log, false will nothing happened when call write log method.
        /// </summary>
        public static bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        #endregion Properties Public

        #region Methods Public

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="savePath">Save path of log file.</param>
        /// <param name="rollSizeKB">Log file max size, when it happend, auto create new one.</param>
        /// <param name="logFormatType">File Format.</param>
        public static void Init(string savePath, int rollSizeKB = 10000, LogFormatType logFormatType = LogFormatType.Default)
        {
            LogCustomWriter.SavePath = savePath;
            LogCustomWriter.RollSizeKB = rollSizeKB;
            _logformatType = logFormatType;
            CheckInParam();
        }

        /// <summary>
        /// Change Logger listener type.
        /// </summary>
        /// <param name="loggerType">Logger listener type.</param>
        public static void ChangeListenerType(LoggerListenerType loggerType)
        {
            LogCustomWriter.ChangeLoggerListenerType(loggerType);
        }

        /// <summary>
        /// Log Critical with file name.
        /// </summary>
        /// <param name="moduleName">File which write for.</param>
        /// <param name="msg">Some Message.</param>
        /// <param name="ex">Exception.</param>
        public static void Critical(string moduleName, string msg, Exception ex = null)
        {
            Write(TraceEventType.Critical, moduleName, msg, ex);
            Write(TraceEventType.Critical, moduleName + ".error", msg, ex);
        }

        /// <summary>
        /// Log Critical with default file(log.txt)
        /// </summary>
        /// <param name="msg">Some Message.</param>
        /// <param name="ex">Exception.</param>
        public static void Critical(string msg, Exception ex = null)
        {
            Write(TraceEventType.Critical, DefaultLogName, msg, ex);
            Write(TraceEventType.Critical, DefaultLogName + ".error", msg, ex);
        }

        /// <summary>
        /// Log Error with file name.
        /// </summary>
        /// <param name="moduleName">File which write for.</param>
        /// <param name="msg">Some Message.</param>
        /// <param name="ex">Exception.</param>
        public static void Error(string moduleName, string msg, Exception ex = null)
        {
            Write(TraceEventType.Error, moduleName, msg, ex);
            Write(TraceEventType.Error, moduleName + ".error", msg, ex);
        }

        /// <summary>
        /// Log Error with default file(log.txt)
        /// </summary>
        /// <param name="msg">Some Message.</param>
        /// <param name="ex">Exception.</param>
        public static void Error(string msg, Exception ex = null)
        {
            Write(TraceEventType.Error, DefaultLogName, msg, ex);
            Write(TraceEventType.Error, DefaultLogName + ".error", msg, ex);
        }

        /// <summary>
        /// Log Info with file name.
        /// </summary>
        /// <param name="moduleName">File which write for.</param>
        /// <param name="msg">Some Message.</param>
        /// <param name="ex">Exception.</param>
        public static void Info(string moduleName, string msg, Exception ex = null)
        {
            Write(TraceEventType.Information, moduleName, msg, ex);
        }

        /// <summary>
        /// Log Info with default file(log.txt)
        /// </summary>
        /// <param name="msg">Some Message.</param>
        /// <param name="ex">Exception.</param>
        public static void Info(string msg, Exception ex = null)
        {
            Write(TraceEventType.Information, DefaultLogName, msg, ex);
        }

        /// <summary>
        /// Log Info with file name.
        /// </summary>
        /// <param name="moduleName">File which write for.</param>
        /// <param name="obj">Some Message.</param>
        public static void Info(string moduleName, object obj)
        {
            Write(TraceEventType.Information, moduleName, obj.ToStrFromObject());
        }

        /// <summary>
        /// Log Verbose with file name.
        /// </summary>
        /// <param name="moduleName">File which write for.</param>
        /// <param name="msg">Some Message.</param>
        /// <param name="ex">Exception.</param>
        /// 
        public static void Verbose(string moduleName, string msg, Exception ex = null)
        {
            Write(TraceEventType.Verbose, moduleName, msg, ex);
        }

        /// <summary>
        /// Log Verbose with default file(log.txt)
        /// </summary>
        /// <param name="msg">Some Message.</param>
        /// <param name="ex">Exception.</param>
        public static void Verbose(string msg, Exception ex = null)
        {
            Write(TraceEventType.Verbose, DefaultLogName, msg, ex);
        }

        /// <summary>
        /// Log Critical with file name.
        /// </summary>
        /// <param name="moduleName">File which write for.</param>
        /// <param name="msg">Some Message.</param>
        /// <param name="ex">Exception.</param>
        public static void Warning(string moduleName, string msg, Exception ex = null)
        {
            Write(TraceEventType.Warning, moduleName, msg, ex);
        }

        /// <summary>
        /// Log Critical with default file(log.txt).
        /// </summary>
        /// <param name="msg">Some Message.</param>
        /// <param name="ex">Exception.</param>
        public static void Warning(string msg, Exception ex = null)
        {
            Write(TraceEventType.Warning, DefaultLogName, msg, ex);
        }

        #endregion Methods Public

        #region Methods Private

        private static string ToStrFromObject(this object obj)
        {
            var str = new StringBuilder();
            var sourceType = obj.GetType();
            var classAttributes = obj.GetType().GetCustomAttributes(false);
            if (classAttributes.Length > 0 && classAttributes[0] is ILogFormatAttribute)
            {
                str.Append(((ILogFormatAttribute)classAttributes[0]).GetLogInfo(sourceType));
                var publicProperties = sourceType.GetProperties();
                foreach (var publicProperty in publicProperties)
                {
                    var propertyAttributes = publicProperty.GetCustomAttributes(false);
                    if (propertyAttributes.Length > 0 && propertyAttributes[0] is ILogFormatAttribute)
                    {
                        str.Append(((ILogFormatAttribute)propertyAttributes[0]).GetLogInfo(sourceType));
                    }
                }
            }
            return str.ToString();
        }

        private static void CheckInParam()
        {
            if (String.IsNullOrEmpty(LogCustomWriter.SavePath))
            {
                throw new ArgumentNullException("LogCustomWriter.SavePath", "Log SavePath is Null");
            }
        }

        private static string GetThreadId()
        {
            var managedThreadId = NativeMethods.GetCurrentThreadId().ToString();
            if (Thread.CurrentThread.ManagedThreadId == _initManagedThreadId)
                return (managedThreadId + "*").PadRight(5);
            return managedThreadId.PadRight(5);
        }

        private static void Write(TraceEventType severity, string fileName, string msg, Exception ex = null)
        {
            if (!IsEnabled)
                return;

            CheckInParam();
            var dic = new Dictionary<string, object>();
            var entry = new LogEntry();
            LogCustomWriter.AddLogSource(fileName, _logformatType);
            dic.Add("ex", LogTemplate.GetException(ex));
            dic.Add("msg", msg);
            dic.Add("severity", LogTemplate.GetSeverity(severity, _logformatType));
            dic.Add("ManagedThreadId", GetThreadId());
            LogCustomWriter.WriteLog(entry, fileName, dic);
        }

        #endregion Methods Private
    }
}