using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using SmallShop.Utilities;
using SmallShop.Entities;

namespace SmallShop.Data
{
   public partial class DataProvider
   {
		private string connectionString;
        private bool initialized = false;
        
        private DataProvider()
        {
            Initialize();
        }
        
        public static DataProvider Instance = new DataProvider();
				
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        public void Initialize(string connnStr = "")
        {
            if (!string.IsNullOrEmpty(connnStr))
            {
                initialized = true;
                connectionString = connnStr;
                SmallShop.Utilities.DbProxy.DbProxy.Init(connectionString);
            }
            else if (!initialized)
            {
                connectionString = ConfigHelper.ConnectionString;
                if (!string.IsNullOrEmpty(connectionString))
                {
                    SmallShop.Utilities.DbProxy.DbProxy.Init(connectionString);
                    initialized = true;
                }
            }
        }
		
		#region generated code here
		
    	#region ExceptionLog
		
        public void CreateExceptionLog(ExceptionLogInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetCreateExceptionLogCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    entity.Id = Convert.ToInt32(obj);
                }
			}
        }
        
        public SqlCommand GetCreateExceptionLogCommand(ExceptionLogInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_ExceptionLog_Create");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@LoginName", SqlDbType.NVarChar, 20, entity.LoginName);
			SqlHelper.AddInParameter(cmd, "@Url", SqlDbType.VarChar, 500, entity.Url);
			SqlHelper.AddInParameter(cmd, "@Message", SqlDbType.VarChar, 1000, entity.Message);
			SqlHelper.AddInParameter(cmd, "@StackTrace", SqlDbType.VarChar, -1, entity.StackTrace);
			SqlHelper.AddInParameter(cmd, "@Ip", SqlDbType.VarChar, 20, entity.Ip);
			SqlHelper.AddInParameter(cmd, "@CreateTime", SqlDbType.DateTime, 8, entity.CreateTime.ToSafeDate());
			
            return cmd;
        }        
	
        public void UpdateExceptionLog(ExceptionLogInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetUpdateExceptionLogCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                cmd.ExecuteNonQuery();
			}
     	}
        
        public void UpdateExceptionLog(string whereClause, params string[] fieldEquations)
        {
          	if (string.IsNullOrEmpty(whereClause) || fieldEquations.Length == 0)
                throw new ArgumentNullException();

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [ExceptionLog] SET ");
            for (int i = 0; i < fieldEquations.Length; i++)
			{
			    if(i == 0)
                    sb.Append(fieldEquations[i]);
                else
                    sb.Append("," + fieldEquations[i]);
			}

            sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
     	}
        
        public SqlCommand GetUpdateExceptionLogCommand(ExceptionLogInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_ExceptionLog_Update");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@LoginName", SqlDbType.NVarChar, 20, entity.LoginName);
			SqlHelper.AddInParameter(cmd, "@Url", SqlDbType.VarChar, 500, entity.Url);
			SqlHelper.AddInParameter(cmd, "@Message", SqlDbType.VarChar, 1000, entity.Message);
			SqlHelper.AddInParameter(cmd, "@StackTrace", SqlDbType.VarChar, -1, entity.StackTrace);
			SqlHelper.AddInParameter(cmd, "@Ip", SqlDbType.VarChar, 20, entity.Ip);
			SqlHelper.AddInParameter(cmd, "@CreateTime", SqlDbType.DateTime, 8, entity.CreateTime.ToSafeDate());
			
            return cmd;
        }
        
        public void DeleteExceptionLogs(string whereClause)
		{
		    StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM [ExceptionLog]");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
               	conn.Open();
				cmd.ExecuteNonQuery();
            }
		}
	
        public ExceptionLogInfo GetExceptionLog(int id)
     	{
			SqlBuilder filter = new SqlBuilder();
			filter.AppendEquals(ExceptionLogCols.Id, id.ToString(), false);
			List<ExceptionLogInfo> list = GetExceptionLogs(filter.ToString(), "");
			
            return list.Count > 0 ? list[0] : null;
      	}
	
        public List<ExceptionLogInfo> GetExceptionLogs()
        {
            return GetExceptionLogs(null, "");
        }
		
        public List<ExceptionLogInfo> GetExceptionLogs(string whereClause)
        {
            return GetExceptionLogs(whereClause, "");
        }

        public List<ExceptionLogInfo> GetExceptionLogs(string whereClause, string orderbyClause)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM [ExceptionLog] with(nolock)");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
            if (!string.IsNullOrEmpty(orderbyClause))
				sb.Append(string.Format(" order by {0}", SqlHelper.ClearUnsafeString(orderbyClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            List<ExceptionLogInfo> entities = new List<ExceptionLogInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ExceptionLogInfo entity = new ExceptionLogInfo();
                        PopulateExceptionLog(reader, entity);
                        entities.Add(entity);
                    }
					reader.Close();
                }
            }
            
            return entities;
        }
		
        public List<ExceptionLogInfo> GetExceptionLogs(IQuery queryParams)
        {
			return GetExceptionLogs(null, queryParams);
        }
        
        public List<ExceptionLogInfo> GetExceptionLogs(string whereClause, IQuery queryParams)
        {
			int rowCount = 0;
            List<ExceptionLogInfo> list = GetExceptionLogs(whereClause, queryParams.GetOrderByClause(), queryParams.PageIndex, queryParams.PageSize, out rowCount);
            queryParams.RecordCount = rowCount;
            
            return list;
        }
        
        public List<ExceptionLogInfo> GetExceptionLogs(string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = 0;
            if (!string.IsNullOrEmpty(whereClause))
				whereClause = " where " + SqlHelper.ClearUnsafeString(whereClause);
                
            if (string.IsNullOrEmpty(orderbyClause))
				orderbyClause = "(select 0)";
            else
                orderbyClause = SqlHelper.ClearUnsafeString(orderbyClause).ToSafeSql(false);
            
            int start = pageSize * (pageIndex - 1);            
            string sql = @"SELECT * FROM [ExceptionLog] with(nolock) {0} order by {1} offset {2} rows fetch next {3} rows only;
						   SELECT Count(1) As recordCount From [ExceptionLog] with(nolock) {0};";
            sql = string.Format(sql, whereClause, orderbyClause, start, pageSize);			
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<ExceptionLogInfo> entities = new List<ExceptionLogInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ExceptionLogInfo entity = new ExceptionLogInfo(); 
                        PopulateExceptionLog(reader, entity);
                        entities.Add(entity);
                    }
                    if (reader.NextResult() && reader.Read())
                        rowCount = (int)reader["recordCount"];
                }
            }
            
            return entities;
        }
                
		private static void PopulateExceptionLog(IDataReader reader, ExceptionLogInfo entity)
		{
			if (reader.HasColumn("Id"))
				entity.Id = (int)reader["Id"];
			
			if (reader.HasColumn("LoginName"))
				entity.LoginName = (string)reader["LoginName"];
			
			if (reader.HasColumn("Url"))
				entity.Url = (string)reader["Url"];
			
			if (reader.HasColumn("Message"))
				entity.Message = (string)reader["Message"];
			
			if (reader.HasColumn("StackTrace"))
				entity.StackTrace = (string)reader["StackTrace"];
			
			if (reader.HasColumn("Ip"))
				entity.Ip = (string)reader["Ip"];
			
			if (reader.HasColumn("CreateTime"))
				entity.CreateTime = (DateTime)reader["CreateTime"];
			
		}		
        
		#endregion
        
    	#region OperationLog
		
        public void CreateOperationLog(OperationLogInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetCreateOperationLogCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    entity.Id = Convert.ToInt32(obj);
                }
			}
        }
        
        public SqlCommand GetCreateOperationLogCommand(OperationLogInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_OperationLog_Create");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@LoginName", SqlDbType.NVarChar, 20, entity.LoginName);
			SqlHelper.AddInParameter(cmd, "@Type", SqlDbType.Int, 4, entity.Type);
			SqlHelper.AddInParameter(cmd, "@BusinessName", SqlDbType.NVarChar, 50, entity.BusinessName);
			SqlHelper.AddInParameter(cmd, "@Description", SqlDbType.NVarChar, 500, entity.Description);
			SqlHelper.AddInParameter(cmd, "@Ip", SqlDbType.VarChar, 20, entity.Ip);
			SqlHelper.AddInParameter(cmd, "@CreateTime", SqlDbType.DateTime, 8, entity.CreateTime.ToSafeDate());
			
            return cmd;
        }        
	
        public void UpdateOperationLog(OperationLogInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetUpdateOperationLogCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                cmd.ExecuteNonQuery();
			}
     	}
        
        public void UpdateOperationLog(string whereClause, params string[] fieldEquations)
        {
          	if (string.IsNullOrEmpty(whereClause) || fieldEquations.Length == 0)
                throw new ArgumentNullException();

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [OperationLog] SET ");
            for (int i = 0; i < fieldEquations.Length; i++)
			{
			    if(i == 0)
                    sb.Append(fieldEquations[i]);
                else
                    sb.Append("," + fieldEquations[i]);
			}

            sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
     	}
        
        public SqlCommand GetUpdateOperationLogCommand(OperationLogInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_OperationLog_Update");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@LoginName", SqlDbType.NVarChar, 20, entity.LoginName);
			SqlHelper.AddInParameter(cmd, "@Type", SqlDbType.Int, 4, entity.Type);
			SqlHelper.AddInParameter(cmd, "@BusinessName", SqlDbType.NVarChar, 50, entity.BusinessName);
			SqlHelper.AddInParameter(cmd, "@Description", SqlDbType.NVarChar, 500, entity.Description);
			SqlHelper.AddInParameter(cmd, "@Ip", SqlDbType.VarChar, 20, entity.Ip);
			SqlHelper.AddInParameter(cmd, "@CreateTime", SqlDbType.DateTime, 8, entity.CreateTime.ToSafeDate());
			
            return cmd;
        }
        
        public void DeleteOperationLogs(string whereClause)
		{
		    StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM [OperationLog]");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
               	conn.Open();
				cmd.ExecuteNonQuery();
            }
		}
	
        public OperationLogInfo GetOperationLog(int id)
     	{
			SqlBuilder filter = new SqlBuilder();
			filter.AppendEquals(OperationLogCols.Id, id.ToString(), false);
			List<OperationLogInfo> list = GetOperationLogs(filter.ToString(), "");
			
            return list.Count > 0 ? list[0] : null;
      	}
	
        public List<OperationLogInfo> GetOperationLogs()
        {
            return GetOperationLogs(null, "");
        }
		
        public List<OperationLogInfo> GetOperationLogs(string whereClause)
        {
            return GetOperationLogs(whereClause, "");
        }

        public List<OperationLogInfo> GetOperationLogs(string whereClause, string orderbyClause)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM [OperationLog] with(nolock)");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
            if (!string.IsNullOrEmpty(orderbyClause))
				sb.Append(string.Format(" order by {0}", SqlHelper.ClearUnsafeString(orderbyClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            List<OperationLogInfo> entities = new List<OperationLogInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OperationLogInfo entity = new OperationLogInfo();
                        PopulateOperationLog(reader, entity);
                        entities.Add(entity);
                    }
					reader.Close();
                }
            }
            
            return entities;
        }
		
        public List<OperationLogInfo> GetOperationLogs(IQuery queryParams)
        {
			return GetOperationLogs(null, queryParams);
        }
        
        public List<OperationLogInfo> GetOperationLogs(string whereClause, IQuery queryParams)
        {
			int rowCount = 0;
            List<OperationLogInfo> list = GetOperationLogs(whereClause, queryParams.GetOrderByClause(), queryParams.PageIndex, queryParams.PageSize, out rowCount);
            queryParams.RecordCount = rowCount;
            
            return list;
        }
        
        public List<OperationLogInfo> GetOperationLogs(string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = 0;
            if (!string.IsNullOrEmpty(whereClause))
				whereClause = " where " + SqlHelper.ClearUnsafeString(whereClause);
                
            if (string.IsNullOrEmpty(orderbyClause))
				orderbyClause = "(select 0)";
            else
                orderbyClause = SqlHelper.ClearUnsafeString(orderbyClause).ToSafeSql(false);
            
            int start = pageSize * (pageIndex - 1);            
            string sql = @"SELECT * FROM [OperationLog] with(nolock) {0} order by {1} offset {2} rows fetch next {3} rows only;
						   SELECT Count(1) As recordCount From [OperationLog] with(nolock) {0};";
            sql = string.Format(sql, whereClause, orderbyClause, start, pageSize);			
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<OperationLogInfo> entities = new List<OperationLogInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OperationLogInfo entity = new OperationLogInfo(); 
                        PopulateOperationLog(reader, entity);
                        entities.Add(entity);
                    }
                    if (reader.NextResult() && reader.Read())
                        rowCount = (int)reader["recordCount"];
                }
            }
            
            return entities;
        }
                
		private static void PopulateOperationLog(IDataReader reader, OperationLogInfo entity)
		{
			if (reader.HasColumn("Id"))
				entity.Id = (int)reader["Id"];
			
			if (reader.HasColumn("LoginName"))
				entity.LoginName = (string)reader["LoginName"];
			
			if (reader.HasColumn("Type"))
				entity.Type = (OperationType)(int)reader["Type"];
			
			if (reader.HasColumn("BusinessName"))
				entity.BusinessName = (string)reader["BusinessName"];
			
			if (reader.HasColumn("Description"))
				entity.Description = (string)reader["Description"];
			
			if (reader.HasColumn("Ip"))
				entity.Ip = (string)reader["Ip"];
			
			if (reader.HasColumn("CreateTime"))
				entity.CreateTime = (DateTime)reader["CreateTime"];
			
		}		
        
		#endregion
        
    	#region Order
		
        public void CreateOrder(OrderInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetCreateOrderCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    entity.Id = Convert.ToInt32(obj);
                }
			}
        }
        
        public SqlCommand GetCreateOrderCommand(OrderInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_Order_Create");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@UserId", SqlDbType.Int, 4, entity.UserId);
			SqlHelper.AddInParameter(cmd, "@Tel", SqlDbType.VarChar, 11, entity.Tel);
			SqlHelper.AddInParameter(cmd, "@Ip", SqlDbType.VarChar, 20, entity.Ip);
			SqlHelper.AddInParameter(cmd, "@CreateTime", SqlDbType.DateTime, 8, entity.CreateTime.ToSafeDate());
			
            return cmd;
        }        
	
        public void UpdateOrder(OrderInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetUpdateOrderCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                cmd.ExecuteNonQuery();
			}
     	}
        
        public void UpdateOrder(string whereClause, params string[] fieldEquations)
        {
          	if (string.IsNullOrEmpty(whereClause) || fieldEquations.Length == 0)
                throw new ArgumentNullException();

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [Order] SET ");
            for (int i = 0; i < fieldEquations.Length; i++)
			{
			    if(i == 0)
                    sb.Append(fieldEquations[i]);
                else
                    sb.Append("," + fieldEquations[i]);
			}

            sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
     	}
        
        public SqlCommand GetUpdateOrderCommand(OrderInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_Order_Update");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@UserId", SqlDbType.Int, 4, entity.UserId);
			SqlHelper.AddInParameter(cmd, "@Tel", SqlDbType.VarChar, 11, entity.Tel);
			SqlHelper.AddInParameter(cmd, "@Ip", SqlDbType.VarChar, 20, entity.Ip);
			SqlHelper.AddInParameter(cmd, "@CreateTime", SqlDbType.DateTime, 8, entity.CreateTime.ToSafeDate());
			
            return cmd;
        }
        
        public void DeleteOrders(string whereClause)
		{
		    StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM [Order]");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
               	conn.Open();
				cmd.ExecuteNonQuery();
            }
		}
	
        public OrderInfo GetOrder(int id)
     	{
			SqlBuilder filter = new SqlBuilder();
			filter.AppendEquals(OrderCols.Id, id.ToString(), false);
			List<OrderInfo> list = GetOrders(filter.ToString(), "");
			
            return list.Count > 0 ? list[0] : null;
      	}
	
        public List<OrderInfo> GetOrders()
        {
            return GetOrders(null, "");
        }
		
        public List<OrderInfo> GetOrders(string whereClause)
        {
            return GetOrders(whereClause, "");
        }

        public List<OrderInfo> GetOrders(string whereClause, string orderbyClause)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM [Order] with(nolock)");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
            if (!string.IsNullOrEmpty(orderbyClause))
				sb.Append(string.Format(" order by {0}", SqlHelper.ClearUnsafeString(orderbyClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            List<OrderInfo> entities = new List<OrderInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderInfo entity = new OrderInfo();
                        PopulateOrder(reader, entity);
                        entities.Add(entity);
                    }
					reader.Close();
                }
            }
            
            return entities;
        }
		
        public List<OrderInfo> GetOrders(IQuery queryParams)
        {
			return GetOrders(null, queryParams);
        }
        
        public List<OrderInfo> GetOrders(string whereClause, IQuery queryParams)
        {
			int rowCount = 0;
            List<OrderInfo> list = GetOrders(whereClause, queryParams.GetOrderByClause(), queryParams.PageIndex, queryParams.PageSize, out rowCount);
            queryParams.RecordCount = rowCount;
            
            return list;
        }
        
        public List<OrderInfo> GetOrders(string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = 0;
            if (!string.IsNullOrEmpty(whereClause))
				whereClause = " where " + SqlHelper.ClearUnsafeString(whereClause);
                
            if (string.IsNullOrEmpty(orderbyClause))
				orderbyClause = "(select 0)";
            else
                orderbyClause = SqlHelper.ClearUnsafeString(orderbyClause).ToSafeSql(false);
            
            int start = pageSize * (pageIndex - 1);            
            string sql = @"SELECT * FROM [Order] with(nolock) {0} order by {1} offset {2} rows fetch next {3} rows only;
						   SELECT Count(1) As recordCount From [Order] with(nolock) {0};";
            sql = string.Format(sql, whereClause, orderbyClause, start, pageSize);			
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<OrderInfo> entities = new List<OrderInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderInfo entity = new OrderInfo(); 
                        PopulateOrder(reader, entity);
                        entities.Add(entity);
                    }
                    if (reader.NextResult() && reader.Read())
                        rowCount = (int)reader["recordCount"];
                }
            }
            
            return entities;
        }
                
		private static void PopulateOrder(IDataReader reader, OrderInfo entity)
		{
			if (reader.HasColumn("Id"))
				entity.Id = (int)reader["Id"];
			
			if (reader.HasColumn("UserId"))
				entity.UserId = (int)reader["UserId"];
			
			if (reader.HasColumn("Tel"))
				entity.Tel = (string)reader["Tel"];
			
			if (reader.HasColumn("Ip"))
				entity.Ip = (string)reader["Ip"];
			
			if (reader.HasColumn("CreateTime"))
				entity.CreateTime = (DateTime)reader["CreateTime"];
			
		}		
        
		#endregion
        
    	#region OrderItem
		
        public void CreateOrderItem(OrderItemInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetCreateOrderItemCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    entity.Id = Convert.ToInt32(obj);
                }
			}
        }
        
        public SqlCommand GetCreateOrderItemCommand(OrderItemInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_OrderItem_Create");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@OrderId", SqlDbType.Int, 4, entity.OrderId);
			SqlHelper.AddInParameter(cmd, "@Number", SqlDbType.VarChar, 20, entity.Number);
			SqlHelper.AddInParameter(cmd, "@ProductId", SqlDbType.Int, 4, entity.ProductId);
			SqlHelper.AddInParameter(cmd, "@OnlinePrice", SqlDbType.Decimal, 4, entity.OnlinePrice);
			SqlHelper.AddInParameter(cmd, "@SettlementPrice", SqlDbType.Decimal, 4, entity.SettlementPrice);
			SqlHelper.AddInParameter(cmd, "@Copies", SqlDbType.Int, 4, entity.Copies);
			SqlHelper.AddInParameter(cmd, "@UsedCopies", SqlDbType.Int, 4, entity.UsedCopies);
			SqlHelper.AddInParameter(cmd, "@RefundCopies", SqlDbType.Int, 4, entity.RefundCopies);
			SqlHelper.AddInParameter(cmd, "@Barcode", SqlDbType.VarChar, 20, entity.Barcode);
			SqlHelper.AddInParameter(cmd, "@ExpendSolidStartDate", SqlDbType.DateTime, 8, entity.ExpendSolidStartDate.ToSafeDate());
			SqlHelper.AddInParameter(cmd, "@ExpendSolidEndDate", SqlDbType.DateTime, 8, entity.ExpendSolidEndDate.ToSafeDate());
			
            return cmd;
        }        
	
        public void UpdateOrderItem(OrderItemInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetUpdateOrderItemCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                cmd.ExecuteNonQuery();
			}
     	}
        
        public void UpdateOrderItem(string whereClause, params string[] fieldEquations)
        {
          	if (string.IsNullOrEmpty(whereClause) || fieldEquations.Length == 0)
                throw new ArgumentNullException();

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [OrderItem] SET ");
            for (int i = 0; i < fieldEquations.Length; i++)
			{
			    if(i == 0)
                    sb.Append(fieldEquations[i]);
                else
                    sb.Append("," + fieldEquations[i]);
			}

            sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
     	}
        
        public SqlCommand GetUpdateOrderItemCommand(OrderItemInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_OrderItem_Update");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@OrderId", SqlDbType.Int, 4, entity.OrderId);
			SqlHelper.AddInParameter(cmd, "@Number", SqlDbType.VarChar, 20, entity.Number);
			SqlHelper.AddInParameter(cmd, "@ProductId", SqlDbType.Int, 4, entity.ProductId);
			SqlHelper.AddInParameter(cmd, "@OnlinePrice", SqlDbType.Decimal, 4, entity.OnlinePrice);
			SqlHelper.AddInParameter(cmd, "@SettlementPrice", SqlDbType.Decimal, 4, entity.SettlementPrice);
			SqlHelper.AddInParameter(cmd, "@Copies", SqlDbType.Int, 4, entity.Copies);
			SqlHelper.AddInParameter(cmd, "@UsedCopies", SqlDbType.Int, 4, entity.UsedCopies);
			SqlHelper.AddInParameter(cmd, "@RefundCopies", SqlDbType.Int, 4, entity.RefundCopies);
			SqlHelper.AddInParameter(cmd, "@Barcode", SqlDbType.VarChar, 20, entity.Barcode);
			SqlHelper.AddInParameter(cmd, "@ExpendSolidStartDate", SqlDbType.DateTime, 8, entity.ExpendSolidStartDate.ToSafeDate());
			SqlHelper.AddInParameter(cmd, "@ExpendSolidEndDate", SqlDbType.DateTime, 8, entity.ExpendSolidEndDate.ToSafeDate());
			
            return cmd;
        }
        
        public void DeleteOrderItems(string whereClause)
		{
		    StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM [OrderItem]");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
               	conn.Open();
				cmd.ExecuteNonQuery();
            }
		}
	
        public OrderItemInfo GetOrderItem(int id)
     	{
			SqlBuilder filter = new SqlBuilder();
			filter.AppendEquals(OrderItemCols.Id, id.ToString(), false);
			List<OrderItemInfo> list = GetOrderItems(filter.ToString(), "");
			
            return list.Count > 0 ? list[0] : null;
      	}
	
        public List<OrderItemInfo> GetOrderItems()
        {
            return GetOrderItems(null, "");
        }
		
        public List<OrderItemInfo> GetOrderItems(string whereClause)
        {
            return GetOrderItems(whereClause, "");
        }

        public List<OrderItemInfo> GetOrderItems(string whereClause, string orderbyClause)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM [OrderItem] with(nolock)");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
            if (!string.IsNullOrEmpty(orderbyClause))
				sb.Append(string.Format(" order by {0}", SqlHelper.ClearUnsafeString(orderbyClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            List<OrderItemInfo> entities = new List<OrderItemInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderItemInfo entity = new OrderItemInfo();
                        PopulateOrderItem(reader, entity);
                        entities.Add(entity);
                    }
					reader.Close();
                }
            }
            
            return entities;
        }
		
        public List<OrderItemInfo> GetOrderItems(IQuery queryParams)
        {
			return GetOrderItems(null, queryParams);
        }
        
        public List<OrderItemInfo> GetOrderItems(string whereClause, IQuery queryParams)
        {
			int rowCount = 0;
            List<OrderItemInfo> list = GetOrderItems(whereClause, queryParams.GetOrderByClause(), queryParams.PageIndex, queryParams.PageSize, out rowCount);
            queryParams.RecordCount = rowCount;
            
            return list;
        }
        
        public List<OrderItemInfo> GetOrderItems(string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = 0;
            if (!string.IsNullOrEmpty(whereClause))
				whereClause = " where " + SqlHelper.ClearUnsafeString(whereClause);
                
            if (string.IsNullOrEmpty(orderbyClause))
				orderbyClause = "(select 0)";
            else
                orderbyClause = SqlHelper.ClearUnsafeString(orderbyClause).ToSafeSql(false);
            
            int start = pageSize * (pageIndex - 1);            
            string sql = @"SELECT * FROM [OrderItem] with(nolock) {0} order by {1} offset {2} rows fetch next {3} rows only;
						   SELECT Count(1) As recordCount From [OrderItem] with(nolock) {0};";
            sql = string.Format(sql, whereClause, orderbyClause, start, pageSize);			
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<OrderItemInfo> entities = new List<OrderItemInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderItemInfo entity = new OrderItemInfo(); 
                        PopulateOrderItem(reader, entity);
                        entities.Add(entity);
                    }
                    if (reader.NextResult() && reader.Read())
                        rowCount = (int)reader["recordCount"];
                }
            }
            
            return entities;
        }
                
		private static void PopulateOrderItem(IDataReader reader, OrderItemInfo entity)
		{
			if (reader.HasColumn("Id"))
				entity.Id = (int)reader["Id"];
			
			if (reader.HasColumn("OrderId"))
				entity.OrderId = (int)reader["OrderId"];
			
			if (reader.HasColumn("Number"))
				entity.Number = (string)reader["Number"];
			
			if (reader.HasColumn("ProductId"))
				entity.ProductId = (int)reader["ProductId"];
			
			if (reader.HasColumn("OnlinePrice"))
				entity.OnlinePrice = (decimal)reader["OnlinePrice"];
			
			if (reader.HasColumn("SettlementPrice"))
				entity.SettlementPrice = (decimal)reader["SettlementPrice"];
			
			if (reader.HasColumn("Copies"))
				entity.Copies = (int)reader["Copies"];
			
			if (reader.HasColumn("UsedCopies"))
				entity.UsedCopies = (int)reader["UsedCopies"];
			
			if (reader.HasColumn("RefundCopies"))
				entity.RefundCopies = (int)reader["RefundCopies"];
			
			if (reader.HasColumn("Barcode"))
				entity.Barcode = (string)reader["Barcode"];
			
			if (reader.HasColumn("ExpendSolidStartDate"))
				entity.ExpendSolidStartDate = (DateTime)reader["ExpendSolidStartDate"];
			
			if (reader.HasColumn("ExpendSolidEndDate"))
				entity.ExpendSolidEndDate = (DateTime)reader["ExpendSolidEndDate"];
			
		}		
        
		#endregion
        
    	#region Role
		
        public void CreateRole(RoleInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetCreateRoleCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    entity.Id = Convert.ToInt32(obj);
                }
			}
        }
        
        public SqlCommand GetCreateRoleCommand(RoleInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_Role_Create");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@Name", SqlDbType.NVarChar, 20, entity.Name);
			SqlHelper.AddInParameter(cmd, "@Permissions", SqlDbType.VarChar, 4000, entity.Permissions);
			SqlHelper.AddInParameter(cmd, "@IsInner", SqlDbType.Bit, 1, entity.IsInner);
			SqlHelper.AddInParameter(cmd, "@CreateTime", SqlDbType.DateTime, 8, entity.CreateTime.ToSafeDate());
			
            return cmd;
        }        
	
        public void UpdateRole(RoleInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetUpdateRoleCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                cmd.ExecuteNonQuery();
			}
     	}
        
        public void UpdateRole(string whereClause, params string[] fieldEquations)
        {
          	if (string.IsNullOrEmpty(whereClause) || fieldEquations.Length == 0)
                throw new ArgumentNullException();

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [Role] SET ");
            for (int i = 0; i < fieldEquations.Length; i++)
			{
			    if(i == 0)
                    sb.Append(fieldEquations[i]);
                else
                    sb.Append("," + fieldEquations[i]);
			}

            sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
     	}
        
        public SqlCommand GetUpdateRoleCommand(RoleInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_Role_Update");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@Name", SqlDbType.NVarChar, 20, entity.Name);
			SqlHelper.AddInParameter(cmd, "@Permissions", SqlDbType.VarChar, 4000, entity.Permissions);
			SqlHelper.AddInParameter(cmd, "@IsInner", SqlDbType.Bit, 1, entity.IsInner);
			SqlHelper.AddInParameter(cmd, "@CreateTime", SqlDbType.DateTime, 8, entity.CreateTime.ToSafeDate());
			
            return cmd;
        }
        
        public void DeleteRoles(string whereClause)
		{
		    StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM [Role]");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
               	conn.Open();
				cmd.ExecuteNonQuery();
            }
		}
	
        public RoleInfo GetRole(int id)
     	{
			SqlBuilder filter = new SqlBuilder();
			filter.AppendEquals(RoleCols.Id, id.ToString(), false);
			List<RoleInfo> list = GetRoles(filter.ToString(), "");
			
            return list.Count > 0 ? list[0] : null;
      	}
	
        public List<RoleInfo> GetRoles()
        {
            return GetRoles(null, "");
        }
		
        public List<RoleInfo> GetRoles(string whereClause)
        {
            return GetRoles(whereClause, "");
        }

        public List<RoleInfo> GetRoles(string whereClause, string orderbyClause)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM [Role] with(nolock)");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
            if (!string.IsNullOrEmpty(orderbyClause))
				sb.Append(string.Format(" order by {0}", SqlHelper.ClearUnsafeString(orderbyClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            List<RoleInfo> entities = new List<RoleInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RoleInfo entity = new RoleInfo();
                        PopulateRole(reader, entity);
                        entities.Add(entity);
                    }
					reader.Close();
                }
            }
            
            return entities;
        }
		
        public List<RoleInfo> GetRoles(IQuery queryParams)
        {
			return GetRoles(null, queryParams);
        }
        
        public List<RoleInfo> GetRoles(string whereClause, IQuery queryParams)
        {
			int rowCount = 0;
            List<RoleInfo> list = GetRoles(whereClause, queryParams.GetOrderByClause(), queryParams.PageIndex, queryParams.PageSize, out rowCount);
            queryParams.RecordCount = rowCount;
            
            return list;
        }
        
        public List<RoleInfo> GetRoles(string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = 0;
            if (!string.IsNullOrEmpty(whereClause))
				whereClause = " where " + SqlHelper.ClearUnsafeString(whereClause);
                
            if (string.IsNullOrEmpty(orderbyClause))
				orderbyClause = "(select 0)";
            else
                orderbyClause = SqlHelper.ClearUnsafeString(orderbyClause).ToSafeSql(false);
            
            int start = pageSize * (pageIndex - 1);            
            string sql = @"SELECT * FROM [Role] with(nolock) {0} order by {1} offset {2} rows fetch next {3} rows only;
						   SELECT Count(1) As recordCount From [Role] with(nolock) {0};";
            sql = string.Format(sql, whereClause, orderbyClause, start, pageSize);			
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<RoleInfo> entities = new List<RoleInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RoleInfo entity = new RoleInfo(); 
                        PopulateRole(reader, entity);
                        entities.Add(entity);
                    }
                    if (reader.NextResult() && reader.Read())
                        rowCount = (int)reader["recordCount"];
                }
            }
            
            return entities;
        }
                
		private static void PopulateRole(IDataReader reader, RoleInfo entity)
		{
			if (reader.HasColumn("Id"))
				entity.Id = (int)reader["Id"];
			
			if (reader.HasColumn("Name"))
				entity.Name = (string)reader["Name"];
			
			if (reader.HasColumn("Permissions"))
				entity.Permissions = (string)reader["Permissions"];
			
			if (reader.HasColumn("IsInner"))
				entity.IsInner = (bool)reader["IsInner"];
			
			if (reader.HasColumn("CreateTime"))
				entity.CreateTime = (DateTime)reader["CreateTime"];
			
		}		
        
		#endregion
        
    	#region RoleUser
		
        public RoleUserInfo CreateRoleUser(RoleUserInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = new SqlCommand("gen_RoleUser_Create", conn);
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@UserId", SqlDbType.Int, 4, entity.UserId);
			SqlHelper.AddInParameter(cmd, "@RoleId", SqlDbType.Int, 4, entity.RoleId);
			using (conn)
			{
				conn.Open();
                cmd.ExecuteNonQuery();
			}
            
			return entity;
        }
        
        public SqlCommand GetCreateRoleUserCommand(RoleUserInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_RoleUser_Create");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@UserId", SqlDbType.Int, 4, entity.UserId);
			SqlHelper.AddInParameter(cmd, "@RoleId", SqlDbType.Int, 4, entity.RoleId);
			
            return cmd;
        }
	
        public void UpdateRoleUser(RoleUserInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetUpdateRoleUserCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                cmd.ExecuteNonQuery();
			}
     	}
        
        public void UpdateRoleUser(string whereClause, params string[] fieldEquations)
        {
          	if (string.IsNullOrEmpty(whereClause) || fieldEquations.Length == 0)
                throw new ArgumentNullException();

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [RoleUser] SET ");
            for (int i = 0; i < fieldEquations.Length; i++)
			{
			    if(i == 0)
                    sb.Append(fieldEquations[i]);
                else
                    sb.Append("," + fieldEquations[i]);
			}

            sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
     	}
        
        public SqlCommand GetUpdateRoleUserCommand(RoleUserInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_RoleUser_Update");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@UserId", SqlDbType.Int, 4, entity.UserId);
			SqlHelper.AddInParameter(cmd, "@RoleId", SqlDbType.Int, 4, entity.RoleId);
			
            return cmd;
        }
        
        public void DeleteRoleUsers(string whereClause)
		{
		    StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM [RoleUser]");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
               	conn.Open();
				cmd.ExecuteNonQuery();
            }
		}
	
        public RoleUserInfo GetRoleUser(int userId, int roleId)
     	{
			SqlBuilder filter = new SqlBuilder();
			filter.AppendEquals(RoleUserCols.UserId, userId.ToString(), false);
			filter.AppendEquals(RoleUserCols.RoleId, roleId.ToString(), false);
			List<RoleUserInfo> list = GetRoleUsers(filter.ToString(), "");
			
            return list.Count > 0 ? list[0] : null;
      	}
	
        public List<RoleUserInfo> GetRoleUsers()
        {
            return GetRoleUsers(null, "");
        }
		
        public List<RoleUserInfo> GetRoleUsers(string whereClause)
        {
            return GetRoleUsers(whereClause, "");
        }

        public List<RoleUserInfo> GetRoleUsers(string whereClause, string orderbyClause)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM [RoleUser] with(nolock)");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
            if (!string.IsNullOrEmpty(orderbyClause))
				sb.Append(string.Format(" order by {0}", SqlHelper.ClearUnsafeString(orderbyClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            List<RoleUserInfo> entities = new List<RoleUserInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RoleUserInfo entity = new RoleUserInfo();
                        PopulateRoleUser(reader, entity);
                        entities.Add(entity);
                    }
					reader.Close();
                }
            }
            
            return entities;
        }
		
        public List<RoleUserInfo> GetRoleUsers(IQuery queryParams)
        {
			return GetRoleUsers(null, queryParams);
        }
        
        public List<RoleUserInfo> GetRoleUsers(string whereClause, IQuery queryParams)
        {
			int rowCount = 0;
            List<RoleUserInfo> list = GetRoleUsers(whereClause, queryParams.GetOrderByClause(), queryParams.PageIndex, queryParams.PageSize, out rowCount);
            queryParams.RecordCount = rowCount;
            
            return list;
        }
        
        public List<RoleUserInfo> GetRoleUsers(string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = 0;
            if (!string.IsNullOrEmpty(whereClause))
				whereClause = " where " + SqlHelper.ClearUnsafeString(whereClause);
                
            if (string.IsNullOrEmpty(orderbyClause))
				orderbyClause = "(select 0)";
            else
                orderbyClause = SqlHelper.ClearUnsafeString(orderbyClause).ToSafeSql(false);
            
            int start = pageSize * (pageIndex - 1);            
            string sql = @"SELECT * FROM [RoleUser] with(nolock) {0} order by {1} offset {2} rows fetch next {3} rows only;
						   SELECT Count(1) As recordCount From [RoleUser] with(nolock) {0};";
            sql = string.Format(sql, whereClause, orderbyClause, start, pageSize);			
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<RoleUserInfo> entities = new List<RoleUserInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RoleUserInfo entity = new RoleUserInfo(); 
                        PopulateRoleUser(reader, entity);
                        entities.Add(entity);
                    }
                    if (reader.NextResult() && reader.Read())
                        rowCount = (int)reader["recordCount"];
                }
            }
            
            return entities;
        }
                
		private static void PopulateRoleUser(IDataReader reader, RoleUserInfo entity)
		{
			if (reader.HasColumn("UserId"))
				entity.UserId = (int)reader["UserId"];
			
			if (reader.HasColumn("RoleId"))
				entity.RoleId = (int)reader["RoleId"];
			
		}		
        
		#endregion
        
    	#region User
		
        public void CreateUser(UserInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetCreateUserCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    entity.Id = Convert.ToInt32(obj);
                }
			}
        }
        
        public SqlCommand GetCreateUserCommand(UserInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_User_Create");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@LoginName", SqlDbType.NVarChar, 20, entity.LoginName);
			SqlHelper.AddInParameter(cmd, "@Password", SqlDbType.VarChar, 32, entity.Password);
			SqlHelper.AddInParameter(cmd, "@Type", SqlDbType.Int, 4, entity.Type);
			SqlHelper.AddInParameter(cmd, "@Balance", SqlDbType.Decimal, 4, entity.Balance);
			SqlHelper.AddInParameter(cmd, "@Depth", SqlDbType.Int, 4, entity.Depth);
			SqlHelper.AddInParameter(cmd, "@ParentId", SqlDbType.Int, 4, entity.ParentId);
			SqlHelper.AddInParameter(cmd, "@WeiXinOpenId", SqlDbType.VarChar, 32, entity.WeiXinOpenId);
			SqlHelper.AddInParameter(cmd, "@CreateTime", SqlDbType.DateTime, 8, entity.CreateTime.ToSafeDate());
			
            return cmd;
        }        
	
        public void UpdateUser(UserInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetUpdateUserCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                cmd.ExecuteNonQuery();
			}
     	}
        
        public void UpdateUser(string whereClause, params string[] fieldEquations)
        {
          	if (string.IsNullOrEmpty(whereClause) || fieldEquations.Length == 0)
                throw new ArgumentNullException();

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [User] SET ");
            for (int i = 0; i < fieldEquations.Length; i++)
			{
			    if(i == 0)
                    sb.Append(fieldEquations[i]);
                else
                    sb.Append("," + fieldEquations[i]);
			}

            sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
     	}
        
        public SqlCommand GetUpdateUserCommand(UserInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_User_Update");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@Id", SqlDbType.Int, 4, entity.Id);
			SqlHelper.AddInParameter(cmd, "@LoginName", SqlDbType.NVarChar, 20, entity.LoginName);
			SqlHelper.AddInParameter(cmd, "@Password", SqlDbType.VarChar, 32, entity.Password);
			SqlHelper.AddInParameter(cmd, "@Type", SqlDbType.Int, 4, entity.Type);
			SqlHelper.AddInParameter(cmd, "@Balance", SqlDbType.Decimal, 4, entity.Balance);
			SqlHelper.AddInParameter(cmd, "@Depth", SqlDbType.Int, 4, entity.Depth);
			SqlHelper.AddInParameter(cmd, "@ParentId", SqlDbType.Int, 4, entity.ParentId);
			SqlHelper.AddInParameter(cmd, "@WeiXinOpenId", SqlDbType.VarChar, 32, entity.WeiXinOpenId);
			SqlHelper.AddInParameter(cmd, "@CreateTime", SqlDbType.DateTime, 8, entity.CreateTime.ToSafeDate());
			
            return cmd;
        }
        
        public void DeleteUsers(string whereClause)
		{
		    StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM [User]");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
               	conn.Open();
				cmd.ExecuteNonQuery();
            }
		}
	
        public UserInfo GetUser(int id)
     	{
			SqlBuilder filter = new SqlBuilder();
			filter.AppendEquals(UserCols.Id, id.ToString(), false);
			List<UserInfo> list = GetUsers(filter.ToString(), "");
			
            return list.Count > 0 ? list[0] : null;
      	}
	
        public List<UserInfo> GetUsers()
        {
            return GetUsers(null, "");
        }
		
        public List<UserInfo> GetUsers(string whereClause)
        {
            return GetUsers(whereClause, "");
        }

        public List<UserInfo> GetUsers(string whereClause, string orderbyClause)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM [User] with(nolock)");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
            if (!string.IsNullOrEmpty(orderbyClause))
				sb.Append(string.Format(" order by {0}", SqlHelper.ClearUnsafeString(orderbyClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            List<UserInfo> entities = new List<UserInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserInfo entity = new UserInfo();
                        PopulateUser(reader, entity);
                        entities.Add(entity);
                    }
					reader.Close();
                }
            }
            
            return entities;
        }
		
        public List<UserInfo> GetUsers(IQuery queryParams)
        {
			return GetUsers(null, queryParams);
        }
        
        public List<UserInfo> GetUsers(string whereClause, IQuery queryParams)
        {
			int rowCount = 0;
            List<UserInfo> list = GetUsers(whereClause, queryParams.GetOrderByClause(), queryParams.PageIndex, queryParams.PageSize, out rowCount);
            queryParams.RecordCount = rowCount;
            
            return list;
        }
        
        public List<UserInfo> GetUsers(string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = 0;
            if (!string.IsNullOrEmpty(whereClause))
				whereClause = " where " + SqlHelper.ClearUnsafeString(whereClause);
                
            if (string.IsNullOrEmpty(orderbyClause))
				orderbyClause = "(select 0)";
            else
                orderbyClause = SqlHelper.ClearUnsafeString(orderbyClause).ToSafeSql(false);
            
            int start = pageSize * (pageIndex - 1);            
            string sql = @"SELECT * FROM [User] with(nolock) {0} order by {1} offset {2} rows fetch next {3} rows only;
						   SELECT Count(1) As recordCount From [User] with(nolock) {0};";
            sql = string.Format(sql, whereClause, orderbyClause, start, pageSize);			
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<UserInfo> entities = new List<UserInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserInfo entity = new UserInfo(); 
                        PopulateUser(reader, entity);
                        entities.Add(entity);
                    }
                    if (reader.NextResult() && reader.Read())
                        rowCount = (int)reader["recordCount"];
                }
            }
            
            return entities;
        }
                
		private static void PopulateUser(IDataReader reader, UserInfo entity)
		{
			if (reader.HasColumn("Id"))
				entity.Id = (int)reader["Id"];
			
			if (reader.HasColumn("LoginName"))
				entity.LoginName = (string)reader["LoginName"];
			
			if (reader.HasColumn("Password"))
				entity.Password = (string)reader["Password"];
			
			if (reader.HasColumn("Type"))
				entity.Type = (UserType)(int)reader["Type"];
			
			if (reader.HasColumn("Balance"))
				entity.Balance = (decimal)reader["Balance"];
			
			if (reader.HasColumn("Depth"))
				entity.Depth = (int)reader["Depth"];
			
			if (reader.HasColumn("ParentId"))
				entity.ParentId = (int)reader["ParentId"];
			
			if (reader.HasColumn("WeiXinOpenId"))
				entity.WeiXinOpenId = (string)reader["WeiXinOpenId"];
			
			if (reader.HasColumn("CreateTime"))
				entity.CreateTime = (DateTime)reader["CreateTime"];
			
		}		
        
		#endregion
        
    	#region UserParent
		
        public void CreateUserParent(UserParentInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetCreateUserParentCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    entity.UserId = Convert.ToInt32(obj);
                }
			}
        }
        
        public SqlCommand GetCreateUserParentCommand(UserParentInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_UserParent_Create");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@UserId", SqlDbType.Int, 4, entity.UserId);
			SqlHelper.AddInParameter(cmd, "@ParentId1", SqlDbType.Int, 4, entity.ParentId1);
			SqlHelper.AddInParameter(cmd, "@ParentId2", SqlDbType.Int, 4, entity.ParentId2);
			SqlHelper.AddInParameter(cmd, "@ParentId3", SqlDbType.Int, 4, entity.ParentId3);
			SqlHelper.AddInParameter(cmd, "@ParentId4", SqlDbType.Int, 4, entity.ParentId4);
			SqlHelper.AddInParameter(cmd, "@ParentId5", SqlDbType.Int, 4, entity.ParentId5);
			SqlHelper.AddInParameter(cmd, "@ParentId6", SqlDbType.Int, 4, entity.ParentId6);
			SqlHelper.AddInParameter(cmd, "@ParentId7", SqlDbType.Int, 4, entity.ParentId7);
			SqlHelper.AddInParameter(cmd, "@ParentId8", SqlDbType.Int, 4, entity.ParentId8);
			SqlHelper.AddInParameter(cmd, "@ParentId9", SqlDbType.Int, 4, entity.ParentId9);
			SqlHelper.AddInParameter(cmd, "@ParentId10", SqlDbType.Int, 4, entity.ParentId10);
			
            return cmd;
        }        
	
        public void UpdateUserParent(UserParentInfo entity)
        {
          	SqlConnection conn = new SqlConnection(ConnectionString);
          	SqlCommand cmd = GetUpdateUserParentCommand(entity);
            cmd.Connection = conn;
			using (conn)
			{
				conn.Open();
                cmd.ExecuteNonQuery();
			}
     	}
        
        public void UpdateUserParent(string whereClause, params string[] fieldEquations)
        {
          	if (string.IsNullOrEmpty(whereClause) || fieldEquations.Length == 0)
                throw new ArgumentNullException();

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [UserParent] SET ");
            for (int i = 0; i < fieldEquations.Length; i++)
			{
			    if(i == 0)
                    sb.Append(fieldEquations[i]);
                else
                    sb.Append("," + fieldEquations[i]);
			}

            sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
     	}
        
        public SqlCommand GetUpdateUserParentCommand(UserParentInfo entity)
        {
          	SqlCommand cmd = new SqlCommand("gen_UserParent_Update");
            cmd.CommandType = CommandType.StoredProcedure;
			SqlHelper.AddInParameter(cmd, "@UserId", SqlDbType.Int, 4, entity.UserId);
			SqlHelper.AddInParameter(cmd, "@ParentId1", SqlDbType.Int, 4, entity.ParentId1);
			SqlHelper.AddInParameter(cmd, "@ParentId2", SqlDbType.Int, 4, entity.ParentId2);
			SqlHelper.AddInParameter(cmd, "@ParentId3", SqlDbType.Int, 4, entity.ParentId3);
			SqlHelper.AddInParameter(cmd, "@ParentId4", SqlDbType.Int, 4, entity.ParentId4);
			SqlHelper.AddInParameter(cmd, "@ParentId5", SqlDbType.Int, 4, entity.ParentId5);
			SqlHelper.AddInParameter(cmd, "@ParentId6", SqlDbType.Int, 4, entity.ParentId6);
			SqlHelper.AddInParameter(cmd, "@ParentId7", SqlDbType.Int, 4, entity.ParentId7);
			SqlHelper.AddInParameter(cmd, "@ParentId8", SqlDbType.Int, 4, entity.ParentId8);
			SqlHelper.AddInParameter(cmd, "@ParentId9", SqlDbType.Int, 4, entity.ParentId9);
			SqlHelper.AddInParameter(cmd, "@ParentId10", SqlDbType.Int, 4, entity.ParentId10);
			
            return cmd;
        }
        
        public void DeleteUserParents(string whereClause)
		{
		    StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM [UserParent]");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            using (conn)
            {
               	conn.Open();
				cmd.ExecuteNonQuery();
            }
		}
	
        public UserParentInfo GetUserParent(int userId)
     	{
			SqlBuilder filter = new SqlBuilder();
			filter.AppendEquals(UserParentCols.UserId, userId.ToString(), false);
			List<UserParentInfo> list = GetUserParents(filter.ToString(), "");
			
            return list.Count > 0 ? list[0] : null;
      	}
	
        public List<UserParentInfo> GetUserParents()
        {
            return GetUserParents(null, "");
        }
		
        public List<UserParentInfo> GetUserParents(string whereClause)
        {
            return GetUserParents(whereClause, "");
        }

        public List<UserParentInfo> GetUserParents(string whereClause, string orderbyClause)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM [UserParent] with(nolock)");
			if (!string.IsNullOrEmpty(whereClause))
            	sb.Append(string.Format(" where {0}", SqlHelper.ClearUnsafeString(whereClause)));
                
            if (!string.IsNullOrEmpty(orderbyClause))
				sb.Append(string.Format(" order by {0}", SqlHelper.ClearUnsafeString(orderbyClause)));
                
			SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            List<UserParentInfo> entities = new List<UserParentInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserParentInfo entity = new UserParentInfo();
                        PopulateUserParent(reader, entity);
                        entities.Add(entity);
                    }
					reader.Close();
                }
            }
            
            return entities;
        }
		
        public List<UserParentInfo> GetUserParents(IQuery queryParams)
        {
			return GetUserParents(null, queryParams);
        }
        
        public List<UserParentInfo> GetUserParents(string whereClause, IQuery queryParams)
        {
			int rowCount = 0;
            List<UserParentInfo> list = GetUserParents(whereClause, queryParams.GetOrderByClause(), queryParams.PageIndex, queryParams.PageSize, out rowCount);
            queryParams.RecordCount = rowCount;
            
            return list;
        }
        
        public List<UserParentInfo> GetUserParents(string whereClause, string orderbyClause, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = 0;
            if (!string.IsNullOrEmpty(whereClause))
				whereClause = " where " + SqlHelper.ClearUnsafeString(whereClause);
                
            if (string.IsNullOrEmpty(orderbyClause))
				orderbyClause = "(select 0)";
            else
                orderbyClause = SqlHelper.ClearUnsafeString(orderbyClause).ToSafeSql(false);
            
            int start = pageSize * (pageIndex - 1);            
            string sql = @"SELECT * FROM [UserParent] with(nolock) {0} order by {1} offset {2} rows fetch next {3} rows only;
						   SELECT Count(1) As recordCount From [UserParent] with(nolock) {0};";
            sql = string.Format(sql, whereClause, orderbyClause, start, pageSize);			
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<UserParentInfo> entities = new List<UserParentInfo>();
            using (conn)
            {
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserParentInfo entity = new UserParentInfo(); 
                        PopulateUserParent(reader, entity);
                        entities.Add(entity);
                    }
                    if (reader.NextResult() && reader.Read())
                        rowCount = (int)reader["recordCount"];
                }
            }
            
            return entities;
        }
                
		private static void PopulateUserParent(IDataReader reader, UserParentInfo entity)
		{
			if (reader.HasColumn("UserId"))
				entity.UserId = (int)reader["UserId"];
			
			if (reader.HasColumn("ParentId1"))
				entity.ParentId1 = (int)reader["ParentId1"];
			
			if (reader.HasColumn("ParentId2"))
				entity.ParentId2 = (int)reader["ParentId2"];
			
			if (reader.HasColumn("ParentId3"))
				entity.ParentId3 = (int)reader["ParentId3"];
			
			if (reader.HasColumn("ParentId4"))
				entity.ParentId4 = (int)reader["ParentId4"];
			
			if (reader.HasColumn("ParentId5"))
				entity.ParentId5 = (int)reader["ParentId5"];
			
			if (reader.HasColumn("ParentId6"))
				entity.ParentId6 = (int)reader["ParentId6"];
			
			if (reader.HasColumn("ParentId7"))
				entity.ParentId7 = (int)reader["ParentId7"];
			
			if (reader.HasColumn("ParentId8"))
				entity.ParentId8 = (int)reader["ParentId8"];
			
			if (reader.HasColumn("ParentId9"))
				entity.ParentId9 = (int)reader["ParentId9"];
			
			if (reader.HasColumn("ParentId10"))
				entity.ParentId10 = (int)reader["ParentId10"];
			
		}		
        
		#endregion
        
		#endregion 
   }
}

