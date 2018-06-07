using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Timers;

namespace SmallShop.Utilities.Lib.AppLog
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoggerManager
    {
        #region Fields

        private static Timer _tDeleteLogFileForDateTimeBefore;
        private static Timer _tDeleteLogFileForNumber;
        private static Timer _tDeleteLogFileForSize;

        #endregion Fields

        #region Methods Public

        public static void DeleteLogFileForDateTimeBeforeWhile(int days, int seconds)
        {
            _tDeleteLogFileForDateTimeBefore = new Timer(seconds * 1000);
            _tDeleteLogFileForDateTimeBefore.Elapsed += (s, e) => DeleteLogFileForDateTimeBefore(days);
            _tDeleteLogFileForDateTimeBefore.Start();
            //TaskManager.AddTask(hours * 60 * 60 * 1000, () => DeleteLogFileForDateTimeBefore(days));
        }

        public static void DeleteLogFileForNumberWhile(int fileNumber, int seconds)
        {
            _tDeleteLogFileForNumber = new Timer(seconds * 1000);
            _tDeleteLogFileForNumber.Elapsed += (s, e) => DeleteLogFileForNumber(fileNumber);
            _tDeleteLogFileForNumber.Start();
        }

        public static void DeleteLogFileForSize(string folderPath, long folderSize, int seconds, bool recursive = false)
        {
            _tDeleteLogFileForSize = new Timer(seconds * 1000);
            _tDeleteLogFileForSize.Elapsed += (s, e) => DeleteLogFileForSize(folderPath, folderSize, recursive);
            _tDeleteLogFileForSize.Start();
        }

        #endregion Methods Public

        #region Methods Private

        private static void DeleteLogFile(string logPath)
        {
            try
            {
                File.Delete(logPath);
            }
            catch (Exception ex)
            {
                Logger.Error("DeleteLogFile", "Delete LogFile failure", ex);
            }
        }

        private static void DeleteLogFileForDateTimeBefore(int days)
        {
            var logFiles = GetLogFiles();
            foreach (var logFile in logFiles)
            {
                if (IsLogExtensionFormat(logFile))
                    continue;
                var logName = Path.GetFileNameWithoutExtension(logFile);
                DateTime logDateTime;
                if (GetLogCreateTime(logName.Split('.'), out logDateTime))
                {
                    var ts = DateTime.Now - logDateTime;
                    if (ts.Days > days)
                        DeleteLogFile(logFile);
                }
            }
        }

        private static void DeleteLogFileForNumber(int fileNumber)
        {
            var logNameCategorys = GetLogNameCategorys(GetLogFiles());
            foreach (var logNameCategory in logNameCategorys)
            {
                if (logNameCategory.Value.Count > fileNumber)
                {
                    logNameCategory.Value.Sort(new Reverser<LogFileInfo>(typeof(LogFileInfo), "CreateTime", ReverserInfo.Direction.DESC));
                    for (var i = fileNumber; i < logNameCategory.Value.Count; i++)
                    {
                        DeleteLogFile(logNameCategory.Value[i].LogFilePath);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="folderSize">(B)</param>
        /// <param name="recursive"></param>
        private static void DeleteLogFileForSize(string folderPath, long folderSize, bool recursive = false)
        {
            var files = new List<string>();
            if (folderSize < 1024)
                return;
            var calcFolderSize = folderPath.GetDirectoryLength(ref files, recursive);
            if (calcFolderSize >= folderSize)
            {
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private static bool GetLogCreateTime(IEnumerable<string> splitNums, out DateTime createTime)
        {
            createTime = new DateTime();
            foreach (var splitNum in splitNums)
            {
                if (!splitNum.IsMatchLetter() || splitNum.Length == 16)
                {
                    if (DateTime.TryParseExact(splitNum, Logger.RollingFileDateTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out createTime))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static IEnumerable<string> GetLogFiles()
        {
            return Directory.Exists(Logger.RootLogPath) ? Directory.GetFiles(Logger.RootLogPath, "*.Log", SearchOption.TopDirectoryOnly) : new string[0];
        }

        private static Dictionary<string, List<LogFileInfo>> GetLogNameCategorys(IEnumerable<string> logFiles)
        {
            var logNameCategorys = new Dictionary<string, List<LogFileInfo>>();
            foreach (var logFile in logFiles)
            {
                if (IsLogExtensionFormat(logFile))
                    continue;
                var logName = Path.GetFileNameWithoutExtension(logFile);
                var splitNums = logName.Split('.');
                DateTime logDateTime;
                if (GetLogCreateTime(splitNums, out logDateTime))
                {
                    var logSplitName = splitNums[0];
                    var logFileInfo = new LogFileInfo { LogFilePath = logFile, CreateTime = logDateTime };
                    if (!logNameCategorys.ContainsKey(logSplitName))
                        logNameCategorys.Add(logSplitName, new List<LogFileInfo> { logFileInfo });
                    else
                        logNameCategorys[logSplitName].Add(logFileInfo);
                }
            }
            return logNameCategorys;
        }

        private static bool IsLogExtensionFormat(string logPath)
        {
            return Path.GetExtension(logPath) != ".Log";
        }

        #endregion Methods Private
    }
}