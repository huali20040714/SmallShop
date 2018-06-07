using System;

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// type of logger
    /// </summary>
    [Flags]
    public enum LoggerListenerType
    {
        /// <summary>
        /// Debug
        /// </summary>
        Debug = 0x1,

        /// <summary>
        /// RollingFlatFile
        /// </summary>
        RollingFlatFile = 0x2,

        /// <summary>
        /// All
        /// </summary>
        All = Debug | RollingFlatFile
    }
}
