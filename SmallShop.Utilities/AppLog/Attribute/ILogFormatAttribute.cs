namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogFormatAttribute
    {
        /// <summary>
        /// get log info
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string GetLogInfo(object source);

        /// <summary>
        /// output format
        /// </summary>
        string Format { get; }
    }
}
