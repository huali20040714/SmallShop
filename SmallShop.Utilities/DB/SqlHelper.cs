using System.Data;
using System.Data.SqlClient;
using System;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;

namespace SmallShop.Utilities
{
    /// <summary>
    /// Helper class that makes it easier to work with the provider.
    /// </summary>
    public sealed class SqlHelper
    {
        #region AddParameter

        public static void AddInParameter(SqlCommand cmd, string paraName, SqlDbType dbType, int size, object value)
        {
            AddParameter(cmd, paraName, ParameterDirection.Input, (SqlDbType?)dbType, (int?)size, value);
        }

        public static void AddInParameter(SqlCommand cmd, string paraName, object value)
        {
            AddParameter(cmd, paraName, ParameterDirection.Input, null, null, value);
        }

        public static void AddOutParameter(SqlCommand cmd, string paraName, SqlDbType dbType, int size)
        {
            AddParameter(cmd, paraName, ParameterDirection.Output, (SqlDbType?)dbType, (int?)size, null);
        }

        public static void AddOutParameter(SqlCommand cmd, string paraName)
        {
            AddParameter(cmd, paraName, ParameterDirection.Output, null, null, null);
        }

        public static void AddInOutParameter(SqlCommand cmd, string paraName, SqlDbType dbType, int size, object value)
        {
            AddParameter(cmd, paraName, ParameterDirection.InputOutput, (SqlDbType?)dbType, (int?)size, value);
        }

        private static void AddParameter(SqlCommand cmd, string paraName, ParameterDirection direction, SqlDbType? dbType, int? size, object value)
        {
            if (!paraName.StartsWith("@"))
                paraName = "@" + paraName;

            SqlParameter para = new SqlParameter();
            para.ParameterName = paraName;
            para.Direction = direction;
            if (dbType != null)
                para.SqlDbType = dbType.GetValueOrDefault();

            if (size != null)
                para.Size = size.GetValueOrDefault();

            if (direction == ParameterDirection.Input || direction == ParameterDirection.InputOutput)
            {
                if (value == null)
                    para.Value = DBNull.Value;
                else
                    para.Value = value;
            }

            cmd.Parameters.Add(para);
        }

        #endregion

        #region 主要过滤单引号字符

        public static string SaftSqlString(string val)
        {
            if (string.IsNullOrEmpty(val))
                return "''";

            return string.Format("'{0}'", val.Replace("'", "''"));
        }

        public static string SaftSqlString(string val, bool widthSingleQuotes)
        {
            val = val ?? "";
            if (widthSingleQuotes)
            {
                return string.Format("'{0}'", val.Replace("'", "''"));
            }
            else
            {
                return string.Format("{0}", val.Replace("'", "''"));
            }
        }

        public static string SaftSqlString(Guid val)
        {
            return SaftSqlString((val == Guid.Empty) ? null : val.ToString());
        }

        public static string SaftSqlString(DateTime val)
        {
            if (val < SqlDateTime.MinValue.Value)
            {
                val = SqlDateTime.MinValue.Value;
            }
            if (val > SqlDateTime.MaxValue.Value)
            {
                val = SqlDateTime.MaxValue.Value;
            }
            return string.Format("'{0}'", val.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public static string SaftSqlString(decimal val)
        {
            return val.ToString();
        }

        public static string SaftSqlString(int val)
        {
            return val.ToString();
        }

        public static string SaftSqlString(long val)
        {
            return val.ToString();
        }

        public static string SaftSqlString(bool val)
        {
            return val ? "1" : "0";
        }
                
        #endregion

        public static DateTime MaxDate
        {
            get
            {
                return SqlDateTime.MaxValue.Value;
            }
        }

        public static DateTime MinDate
        {
            get
            {
                return SqlDateTime.MinValue.Value;
            }
        }

        /// <summary>
        /// 过滤非安全字符(未做单引号处理)
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static string ClearUnsafeString(string sqlstr)
        {
            sqlstr = sqlstr ?? "";
            sqlstr = sqlstr.Replace("~", "");
            string unsafeRegex = "update[ \r]|delete[ \r]|insert[ \r]|drop[ \r]|truncate[ \r]|exec[ \r]|net[ \r]";
            bool isMatch = Regex.IsMatch(sqlstr, unsafeRegex, RegexOptions.IgnoreCase);
            if (isMatch)
                return Regex.Replace(sqlstr, "[ \r]*", "");

            return sqlstr;
        }
    }
}
