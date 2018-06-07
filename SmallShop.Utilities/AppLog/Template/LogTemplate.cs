#region Import

using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

#endregion

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// 
    /// </summary>
    internal static class LogTemplate
    {

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private const string MsgRollingFlatFileTemplate = "[{timestamp(local:yyyy-MM-dd HH:mm:ss,fff)} {keyvalue(severity)} {keyvalue(ManagedThreadId)}]  {keyvalue(msg)}" +
                                                 "{keyvalue(ex)}";

        /// <summary>
        /// 
        /// </summary>
        private const string MsgDebugTraceTemplate = "[{timestamp(local:yyyy-MM-dd HH:mm:ss,fff)} {keyvalue(severity)} {keyvalue(ManagedThreadId)}]  {keyvalue(msg)}" +
                                                 "{keyvalue(ex)}";

        /// <summary>
        /// 2
        /// </summary>
        private const string MsgRollingFlatFileTemplate2 = "{timestamp(local:yyyy-MM-dd HH:mm:ss,fff)} {keyvalue(severity)}  {keyvalue(msg)}" +
                                                 "{keyvalue(ex)}";

        /// <summary>
        /// 2
        /// </summary>
        private const string MsgDebugTraceTemplate2 = "{timestamp(local:yyyy-MM-dd HH:mm:ss,fff)} {keyvalue(severity)}  {keyvalue(msg)}" +
                                                 "{keyvalue(ex)}";

        /// <summary>
        /// [EVS]
        /// </summary>
        private const string MsgRollingFlatFileTemplate3 = "{timestamp(local:yyyy-MM-dd HH:mm:ss,fff)} {keyvalue(ManagedThreadId)}  {keyvalue(msg)}" +
                                                 "{keyvalue(ex)}";

        /// <summary>
        /// [EVS]
        /// </summary>
        private const string MsgDebugTraceTemplate3 = "{timestamp(local:yyyy-MM-dd HH:mm:ss,fff)} {keyvalue(ManagedThreadId)}  {keyvalue(msg)}" +
                                                 "{keyvalue(ex)}";

        #endregion

        #region Methods

        internal static ILogFormatter GetRollingFlatFileFormatter(LogFormatType logFormatType)
        {
            var template = MsgRollingFlatFileTemplate;
            if (logFormatType == LogFormatType.Details)
                template = MsgRollingFlatFileTemplate2;
            else if (logFormatType == LogFormatType.Release)
                template = MsgRollingFlatFileTemplate3;
            return new TextFormatter(template);
        }

        internal static ILogFormatter GetDebugTraceFormatter(LogFormatType logFormatType)
        {
            var template = MsgDebugTraceTemplate;
            if (logFormatType == LogFormatType.Details)
                template = MsgDebugTraceTemplate2;
            else if (logFormatType == LogFormatType.Release)
                template = MsgDebugTraceTemplate3;
            return new TextFormatter(template);
        }

        internal static string GetSeverity(TraceEventType severity, LogFormatType logFormatType)
        {
            switch (severity)
            {
                case TraceEventType.Critical:
                    return logFormatType == LogFormatType.Default ? "C" : "Critical";
                case TraceEventType.Error:
                    return logFormatType == LogFormatType.Default ? "E" : "Error";
                case TraceEventType.Information:
                    return logFormatType == LogFormatType.Default ? "I" : "Info";
                case TraceEventType.Warning:
                    return logFormatType == LogFormatType.Default ? "W" : "Warn";
                case TraceEventType.Verbose:
                    return "V";
                default:
                    throw new ArgumentOutOfRangeException("severity");
            }
        }

        public static string GetException(Exception ee)
        {
            if (ee == null)
                return " ";

            var msgContent = new StringBuilder($"\r\n\r\n[{ee.GetType()}]\r\n");
            msgContent.Append(GetMsgContent(ee));

            for (int i = 0; i < 10; i++)
            {
                if (ee.InnerException != null)
                {
                    ee = ee.InnerException;
                    msgContent.Append("\r\n[{ee.GetType()}]\r\n");
                    msgContent.Append(GetMsgContent(ee));
                }
                else
                    break;
            }

            return msgContent.ToString();
        }

        private static string GetMsgContent(Exception ee)
        {
            string ret = ee.Message;
            if (!string.IsNullOrEmpty(ee.StackTrace))
                ret += "\r\n" + ee.StackTrace;
            ret += "\r\n";
            return ret;
        }

        #endregion

    }
}
