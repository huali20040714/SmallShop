using System;
using System.Data;

namespace SmallShop.Utilities
{
    /// <summary>
    /// 针对ADO.Net进行扩展
    /// </summary>
    public static class AdoNetExtensions
    {
        /// <summary>
        /// 判断列是否存在
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool HasColumn(this IDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (dr[columnName] != DBNull.Value)
                        return true;
                }
            }
            return false;
        }
    }
}
