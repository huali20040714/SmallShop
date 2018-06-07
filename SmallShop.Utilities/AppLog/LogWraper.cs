using System;

namespace SmallShop.Utilities.Lib.AppLog
{
    public class LogWraper
    {
        private readonly string _moduleName;

        private readonly bool _enabled;

        public LogWraper(string moduleName, bool enabled = true)
        {
            _moduleName = moduleName;
            _enabled = enabled;
        }

        public void Info(string str, Exception ex = null)
        {
            if (_enabled)
                Logger.Info(_moduleName, str, ex);
        }

        public void Critical(string str, Exception ex = null)
        {
            if (_enabled)
                Logger.Critical(_moduleName, str, ex);
        }

        public void Error(string str, Exception ex = null)
        {
            if (_enabled)
                Logger.Error(_moduleName, str, ex);
        }

        public void Verbose(string str, Exception ex = null)
        {
            if (_enabled)
                Logger.Verbose(_moduleName, str, ex);
        }

        public void Warning(string str, Exception ex = null)
        {
            if (_enabled)
                Logger.Warning(_moduleName, str, ex);
        }
    }
} 
