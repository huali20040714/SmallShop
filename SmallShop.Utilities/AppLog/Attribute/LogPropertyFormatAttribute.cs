#region Import

using System;
using System.Reflection;

#endregion

namespace SmallShop.Utilities.Lib.AppLog
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LogPropertyFormatAttribute : Attribute, ILogFormatAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format">default：{0}--property name  {1}--property value</param>
        public LogPropertyFormatAttribute(string format = "{0} : {1}")
        {
            Format = format;
        }

        public string GetLogInfo(object source)
        {
            if (source is PropertyInfo)
            {
                var propertyInfo = (PropertyInfo) source;
                var propertyValue = propertyInfo.GetValue(propertyInfo.ReflectedType, null);
                return string.Format(Format, propertyValue);
            }
            return string.Empty;
        }

        public string Format { private set; get; }
    }
}
