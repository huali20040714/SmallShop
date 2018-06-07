using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SmallShop.Utilities.DbProxy
{
    public static class DbProxy
    {
        private static string _connString;

        public static void Init(string connString)
        {
            _connString = connString;
        }

        #region Generic ReadFristOrDefault

        public static T ReadFristOrDefault<T>(string queryString) where T : new()
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                return conn.ReadFristOrDefault<T>(queryString);
            }
        }

        public static T ReadFristOrDefault<T>(SqlCommand cmd) where T : new()
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                return conn.ReadFristOrDefault<T>(cmd);
            }
        }

        public static T ReadFristOrDefault<T>(this SqlConnection conn, string queryString, SqlTransaction trans = null) where T : new()
        {
            SqlCommand cmd = new SqlCommand(queryString, conn);
            return conn.ReadFristOrDefault<T>(cmd, trans);
        }

        public static T ReadFristOrDefault<T>(this SqlConnection conn, SqlCommand cmd, SqlTransaction trans = null) where T : new()
        {
            cmd.Connection = conn;
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            if (trans != null)
                cmd.Transaction = trans;

            try
            {
                using (IDataReader reader = cmd.ExecuteReader())
                    return reader.ReadFristOrDefault<T>();
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText, ex);
            }
        }

        public static T ReadFristOrDefault<T>(this IDataReader reader, bool nextResult = false) where T : new()
        {
            if (nextResult && !reader.NextResult())
                return default(T);

            if (!reader.Read())
                return default(T);

            return Populate<T>(reader);
        }

        public static T ReadTranFristOrDefault<T>(string queryString) where T : new()
        {
            T result = default(T);
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand(queryString, conn);
                        result = conn.ReadFristOrDefault<T>(cmd, tran);
                        tran.Commit();

                    }
                    catch
                    {
                        tran.Rollback();
                    }
                }
            }
            return result;
        }


        #endregion

        #region Generic ReadAll

        public static List<T1> ReadAll<T1, T2>(string queryString, out T2 result2) where T1 : new() where T2 : new()
        {
            object o;
            return ReadAll<T1, T2, object>(queryString, out result2, out o);
        }

        public static List<T1> ReadAll<T1, T2, T3>(string queryString, out T2 result2, out T3 result3)
            where T1 : new() where T2 : new() where T3 : new()
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                return conn.ReadAll<T1, T2, T3>(queryString, out result2, out result3, null);
            }
        }

        public static List<T> ReadAll<T>(string queryString, out int recordCount) where T : new()
        {
            object ret;
            return ReadAll<T, int, object>(queryString, out recordCount, out ret);
        }

        public static List<T> ReadAll<T>(string queryString) where T : new()
        {
            object ret;
            return ReadAll<T, object, object>(queryString, out ret, out ret);
        }

        public static List<T1> ReadAll<T1, T2>(this SqlConnection conn, string queryString, out T2 result2, SqlTransaction trans = null) where T1 : new() where T2 : new()
        {
            object o;
            return conn.ReadAll<T1, T2, object>(queryString, out result2, out o, trans);
        }

        public static List<T> ReadAll<T>(this SqlConnection conn, string queryString, out int recordCount, SqlTransaction trans = null) where T : new()
        {
            object ret;
            return conn.ReadAll<T, int, object>(queryString, out recordCount, out ret, trans);
        }

        public static List<T> ReadAll<T>(this SqlConnection conn, string queryString, SqlTransaction trans = null) where T : new()
        {
            object ret;
            int recordCount;
            return conn.ReadAll<T, int, object>(queryString, out recordCount, out ret, trans);
        }

        public static List<T1> ReadAll<T1, T2, T3>(this SqlConnection conn, string queryString, out T2 result2, out T3 result3, SqlTransaction trans = null)
            where T1 : new() where T2 : new() where T3 : new()
        {
            SqlCommand cmd = new SqlCommand(queryString, conn);
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            if (trans != null)
                cmd.Transaction = trans;

            try
            {
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    List<T1> ret = reader.ReadAll<T1>(false);
                    result2 = reader.ReadFristOrDefault<T2>(true);
                    result3 = reader.ReadFristOrDefault<T3>(true);
                    reader.Close();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(queryString, ex);
            }
        }

        public static T Populate<T>(IDataReader reader) where T : new()
        {
            return Helper.Populate<T>(reader);
        }

        public static List<T> ReadAll<T>(this IDataReader reader, bool nextResult = false) where T : new()
        {
            if (nextResult && !reader.NextResult())
                return new List<T>();

            List<T> ret = new List<T>();
            while (reader.Read())
            {
                var entity = Populate<T>(reader);
                ret.Add(entity);
            }

            return ret;
        }

        #endregion

        #region Object

        public static object ReadFristOrDefault(string queryString)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                conn.Open();
                try
                {
                    using (IDataReader reader = cmd.ExecuteReader())
                        return reader.ReadFristOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(queryString, ex);
                }
            }
        }

        public static object ReadFristOrDefault(this IDataReader reader, bool nextResult = false)
        {
            if (nextResult && !reader.NextResult())
                return null;

            if (!reader.Read())
                return null;

            return Helper.ReaderConvertToObject(reader);
        }

        #endregion

        #region Execute

        public static int ExecuteNonQuery(string queryString)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(queryString);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                return cmd.ExecuteNonQuery();
            }
        }

        public static int ExecuteNonQuery(this SqlConnection conn, string queryString, SqlTransaction trans = null)
        {
            SqlCommand cmd = new SqlCommand(queryString);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;

            return cmd.ExecuteNonQuery();
        }

        #endregion

        #region Query

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(_connString);
        }

        public static Query CreateQuery(string queryString)
        {
            return new Query(queryString, _connString);
        }

        public static Query CreateQuery(this SqlConnection conn, string queryString)
        {
            return new Query(conn, queryString, null);
        }

        public static Query CreateQuery(this SqlConnection conn, string queryString, SqlTransaction trans)
        {
            return new Query(conn, queryString, trans);
        }

        #endregion

        #region TableOrViewName

        public static List<T> ReadAllFromTableOrView<T>(string tableOrViewName, string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount)
            where T : new()
        {
            if (!string.IsNullOrEmpty(whereClause))
                whereClause = " where " + whereClause;

            if (string.IsNullOrEmpty(orderbyClause))
                orderbyClause = "(select 0)";
            else
                orderbyClause = Helper.ClearAllUnsafeString(orderbyClause);

            int start = pageSize * (pageIndex - 1) + 1;
            int end = pageSize * pageIndex;

            string sql =
                $@"
                SELECT * FROM (
                    SELECT ROW_NUMBER() Over(order by {orderbyClause}) AS rowNum, *
                    FROM {tableOrViewName}
                    {whereClause}
                ) AS newTable
                Where (rowNum >= {start} and rowNum <= {end});
                
                SELECT Count(1) As recordCount From [SmsTemplate] {whereClause}
            ";

            return ReadAll<T>(sql, out rowCount);
        }

        public static List<T> ReadAllFromTableOrView<T>(string tableOrViewName, string whereClause = null, string orderbyClause = null) where T : new()
        {
            if (!string.IsNullOrEmpty(whereClause))
                whereClause = " where " + whereClause;

            if (!string.IsNullOrEmpty(orderbyClause))
                orderbyClause = " order by " + Helper.ClearAllUnsafeString(orderbyClause);

            string sql = $@"SELECT * FROM {tableOrViewName} {whereClause} {orderbyClause}";

            return ReadAll<T>(sql);
        }

        public static T ReadFromTableOrView<T>(string tableOrViewName, int id, string idName = null) where T : new()
        {
            if (idName == null)
                idName = "id";

            string sql = $@"SELECT * FROM {tableOrViewName} where {idName}={id}";
            return ReadFristOrDefault<T>(sql);
        }

        public static bool ExistFromTableOrView(string tableOrViewName, string whereClause)
        {
            string where = "";
            if (!string.IsNullOrEmpty(whereClause))
                where = string.Format(" where {0}", whereClause);

            string sql = $@"SELECT count(1) FROM {tableOrViewName} {where}";
            return Exist(sql);
        }

        public static bool Exist(string queryString)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                bool ret = false;
                SqlCommand cmd = new SqlCommand(queryString);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != DBNull.Value && Convert.ToInt32(obj) > 0)
                {
                    ret = true;
                }
                return ret;
            }
        }

        #endregion

        #region 数据库表查询(单表分页)

        public static List<dynamic> GetPageRecords(IQuery queryParams)
        {
            return GetPageRecords<dynamic>(queryParams);
        }

        public static List<dynamic> GetPageRecords(string table, string selectCols, string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount)
        {
            return GetPageRecords<dynamic>(table, selectCols, whereClause, orderbyClause, pageIndex, pageSize, out rowCount);
        }

        public static List<T> GetPageRecords<T>(IQuery queryParams) where T : new()
        {
            int rowCount = 0;
            var list = GetPageRecords<T>(queryParams.Table, queryParams.SelectCols, queryParams.WhereClause, queryParams.GetOrderByClause(), queryParams.PageIndex, queryParams.PageSize, out rowCount);
            queryParams.RecordCount = rowCount;

            return list;
        }

        public static List<T> GetPageRecords<T>(string table, string selectCols, string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount) where T : new()
        {
            var list = GetPageRecords<T, int>(table, selectCols, string.Empty, whereClause, orderbyClause, pageIndex, pageSize, out rowCount);
            return list;
        }

        public static List<T> GetPageRecords<T>(string table, string whereClause, string selectCols = "*") where T : new()
        {
            table = table.ToSafeSql(false);
            selectCols = string.IsNullOrEmpty(selectCols) ? "*" : selectCols.ToSafeSql(false);
            if (!string.IsNullOrEmpty(whereClause))
                whereClause = " where " + SqlHelper.ClearUnsafeString(whereClause);

            string sql = $@"SELECT {selectCols} FROM [{table}] with(nolock) {whereClause};";

            return ReadAll<T>(sql);
        }



        public static List<T1> GetPageRecords<T1, T2>(IQuery queryParams, string statisticsCols, out T2 result2) where T1 : new() where T2 : new()
        {
            var list = GetPageRecords<T1, T2>(queryParams.Table, queryParams.SelectCols, statisticsCols, queryParams.WhereClause, queryParams.GetOrderByClause(), queryParams.PageIndex, queryParams.PageSize, out result2);

            return list;
        }

        public static List<T1> GetPageRecords<T1, T2>(string table, string selectCols, string statisticsCols, string whereClause, string orderbyClause, int pageIndex, int pageSize, out T2 result2) where T1 : new() where T2 : new()
        {
            table = table.ToSafeSql(false);
            selectCols = string.IsNullOrEmpty(selectCols) ? "*" : selectCols.ToSafeSql(false);
            statisticsCols = string.IsNullOrEmpty(statisticsCols) ? "Count(1) As RecordCount" : statisticsCols.ToSafeSql(false);
            if (!string.IsNullOrEmpty(whereClause))
                whereClause = " where " + SqlHelper.ClearUnsafeString(whereClause);

            if (string.IsNullOrEmpty(orderbyClause))
                orderbyClause = "(select 0)";
            else
                orderbyClause = SqlHelper.ClearUnsafeString(orderbyClause).ToSafeSql(false);

            int start = pageSize * (pageIndex - 1);
            string sql = $@"SELECT {selectCols} FROM [{table}] with(nolock) {whereClause} order by {orderbyClause} offset {start} rows fetch next {pageSize} rows only;
						   SELECT {statisticsCols} From [{table}] with(nolock) {whereClause};";

            return ReadAll<T1, T2>(sql, out result2);
        }

        #endregion

        #region SQL语句事务执行

        public static Result<object> ExecuteTranQuery(params string[] queryStrings)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var queryString in queryStrings)
                        {
                            SqlCommand cmd = new SqlCommand(queryString, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                        tran.Commit();

                        return Result.Default;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        return Result.Error(ex.Message);
                    }
                }
            }
        }

        #endregion

        #region 事务执行,遍历实体列表

        /// <summary>
        /// 事务执行,遍历实体列表
        /// </summary>
        public static Result<object> ExecuteTranEntities<T>(List<T> entities, Func<T, SqlCommand> func)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var entity in entities)
                        {
                            var cmd = func(entity);
                            cmd.Connection = conn;
                            cmd.Transaction = tran;
                            cmd.ExecuteNonQuery();
                        }
                        tran.Commit();

                        return Result.Default;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        return Result.Error(ex.Message);
                    }
                }
            }
        }

        #endregion

        #region 执行存储过程、并获取执行后的结果集合

        /// <summary>
        /// 执行存储过程、并获取执行后的结果集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="action">初始化参数、引用传导</param>
        public static List<T> ExecuteProcedure<T>(string procName, Action<SqlCommand> action) where T : new()
        {
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    var cmd = new SqlCommand(procName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    action(cmd);
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        List<T> ret = reader.ReadAll<T>(false);
                        reader.Close();

                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(procName, ex);
            }
        }

        #endregion

        #region 执行存储过程、并获取执行后的自增Id

        /// <summary>
        /// 执行存储过程、并获取执行后的自增Id
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="action">初始化参数、引用传导</param>
        public static int ExecuteProcedure(string procName, Action<SqlCommand> action)
        {
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    var cmd = new SqlCommand(procName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    action(cmd);
                    conn.Open();

                    object obj = cmd.ExecuteScalar();
                    var id = 0;
                    if (obj != null && obj != DBNull.Value)
                        id = Convert.ToInt32(obj);

                    return id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(procName, ex);
            }
        }

        #endregion
    }
}