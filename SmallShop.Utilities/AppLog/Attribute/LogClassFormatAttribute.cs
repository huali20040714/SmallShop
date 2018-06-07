#region Import

using System;

#endregion

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// coperate with log
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LogClassFormatAttribute : Attribute, ILogFormatAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format">default format：{0}--class name</param>
        public LogClassFormatAttribute(string format = "{0} >>>>>>>>>>")
        {
            Format = format;
        }

        public string GetLogInfo(object source)
        {
            if (source is Type)
            {
                var className = ((Type)source).Name;
                return string.Format(Format, className);
            }
            return string.Empty;
        }

        public string Format { private set; get; }

    }
}
