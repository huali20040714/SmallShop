using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SmallShop.Utilities.DbProxy
{
    public class Query
    {
        private readonly string _queryString;
        private readonly string _connString;
        private readonly SqlCommand _cmd;
        private readonly SqlConnection _conn;
        private readonly Dictionary<string, object> _params = new Dictionary<string, object>();

        public Query(SqlConnection conn, string queryString, SqlTransaction trans)
        {
            _queryString = queryString;

            _conn = conn;
            _cmd = new SqlCommand(_queryString, _conn);

            if (trans != null)
                _cmd.Transaction = trans;
        }

        public Query(string queryString, string connString)
        {
            _queryString = queryString;
            _connString = connString;
            _cmd = new SqlCommand(_queryString, _conn);
        }

        public T FirstOrDefault<T>() where T : new ()
        {
            return ExecuteReader(r =>
            {
                if (!r.Read())
                    return default(T);

                return Helper.Populate<T>(r);
            });
        }

        public object FirstOrDefault()
        {
            return ExecuteReader(r =>
            {
                if (!r.Read())
                    return null;

                return Helper.ReaderConvertToObject(r);
            });
        }

        public T UniqueResult<T>() where T : new()
        {
            return ExecuteReader(r =>
            {
                if (!r.Read())
                    throw new NotUniqueResultException("Has no rows in DataReader.");

                var ret = Helper.Populate<T>(r);

                if (r.Read())
                    throw new NotUniqueResultException("Has more than one row in DataReader.");

                return ret;
            });
        }

        public object UniqueResult()
        {
            return ExecuteReader(r =>
            {
                if (!r.Read())
                    throw new NotUniqueResultException("Has no rows in DataReader.");

                var ret = Helper.ReaderConvertToObject(r);

                if (r.Read())
                    throw new NotUniqueResultException("Has more than one row in DataReader.");

                return ret;
            });
        }

        public List<T> List<T>() where T : new()
        {
            return ExecuteReader(r =>
            {
                List<T> ret = new List<T>();
                while (r.Read())
                {
                    var entity = Helper.Populate<T>(r);
                    ret.Add(entity);
                }
                return ret;
            });
        }

        public Query SetParameter(string name, object val)
        {
            AddInParameter(_cmd, name, val);
            return this;
        }

        public int ExecuteNonQuery()
        {
            try
            {
                if (_conn == null)
                {
                    using (var conn = new SqlConnection(_connString))
                    {
                        conn.Open();
                        _cmd.Connection = conn;
                        return _cmd.ExecuteNonQuery();
                    }
                }

                if (_conn.State == ConnectionState.Closed)
                    _conn.Open();
                return _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(GetQueryInfo(), ex);
            }
        }
        
        private void AddInParameter(SqlCommand cmd, string paramName, object value)
        {
            if (!paramName.StartsWith("@"))
                paramName = "@" + paramName;

            _params.Add(paramName, value);

            SqlParameter para = new SqlParameter();
            para.ParameterName = paramName;
            para.Direction = ParameterDirection.Input;
            if (value == null)
                para.Value = DBNull.Value;
            else
                para.Value = value;
            cmd.Parameters.Add(para);
        }

        private T ExecuteReader<T>(Func<IDataReader, T> func)
        {
            try
            {
                if (_conn == null)
                {
                    using (var conn = new SqlConnection(_connString))
                    {
                        _cmd.Connection = conn;
                        return Invoke(conn, func);
                    }
                }

                return Invoke(_conn, func);
            }
            catch (Exception ex)
            {
                throw new Exception(GetQueryInfo(), ex);
            }
        }

        private string GetQueryInfo()
        {
            string s = _queryString + "\r\n";
            foreach (var k in _params.Keys)
                s += $"{k}:{_params[k]}, ";

            if (s.EndsWith(", "))
                return s.Substring(0, s.Length - 2);

            return s;
        }

        private T Invoke<T>(SqlConnection conn, Func<IDataReader, T> func)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (IDataReader reader = _cmd.ExecuteReader())
            {
                return func.Invoke(reader);
            }
        }
    }
}