using System;
using System.Data;
using System.ComponentModel;
using CodeSmith.Engine;
using SchemaExplorer;
using Microsoft.CSharp;
using System.Text;
using System.Collections.Generic;

public class Helper : CodeTemplate
{
	#region Other
	public static string GetCType(ColumnSchema column)
    {
        if (column.Description.Trim().StartsWith("enum", System.StringComparison.OrdinalIgnoreCase) && column.Description.Trim().Length > 6)
		{
            if (column.AllowDBNull)
				return string.Format("{0}", column.Description.Trim().Substring(5));
			else
				return string.Format("{0}",  column.Description.Trim().Substring(5));
		}
		return GetCType(column.DataType, column.AllowDBNull);
    }
	
	public static string GetCType(ViewColumnSchema column, 
		SchemaExplorer.ExtendedPropertyCollection extendedProperties)
    {
		if (extendedProperties["enum"] != null)
		{
			string retEnum = GetEnumFromEp(extendedProperties["enum"].Value.ToString(), column.Name);
			if (retEnum != null)
				return column.AllowDBNull ? retEnum : retEnum;
		}
		
      	return GetCType(column.DataType, column.AllowDBNull);
    }
	
	public static string GetCType(ParameterSchema para, 
		SchemaExplorer.ExtendedPropertyCollection extendedProperties)
    {
		if (extendedProperties["enum"] != null)
		{
			string retEnum = GetEnumFromEp(extendedProperties["enum"].Value.ToString(), para.Name);
			if (retEnum != null)
				return retEnum;
		}
		
      	return GetCType(para.DataType, false);
    }
	
	public string GetPopulateType(ColumnSchema column)
	{
		string ret = string.Empty;
		if (column.Description.Trim().StartsWith("enum", System.StringComparison.OrdinalIgnoreCase) && column.Description.Trim().Length > 6)
		{
			ret = string.Format("{0}", column.Description.Trim().Substring(5));
			ret = string.Format("({0})({1})", ret, GetCType(column.DataType, false));
		}
		else
			ret = string.Format("({0})", GetCType(column.DataType, column.AllowDBNull));

		return ret;
	}
	
	public string GetPopulateType(ViewColumnSchema column,
		SchemaExplorer.ExtendedPropertyCollection viewExtendedProperties)
	{
		if (viewExtendedProperties["enum"] != null)
		{
			string ret = GetEnumFromEp(viewExtendedProperties["enum"].Value.ToString(), column.Name);
			if (ret != null)
			{
				ret = string.Format("({0})({1})", ret, GetCType(column.DataType, false));
				return ret;
			}
		}
		
      	return string.Format("({0})", GetCType(column.DataType, false));
	}
	
	private static string GetEnumFromEp(string ep, string columnName)
	{
		//eg: ep = "Type PictureType UserType UserType"
		//    ep = "[FiledsName1 EnumName1 FiledsName2 EnumName2 ...]"
		string[] strs = ep.Split(' ');
		
		for (int i = 0; i < strs.Length; i += 2)
		{
			if (strs[i] == columnName)
				return strs[i + 1];
		}
		
		return null;
	}
	
	public static int GetParamSize(ColumnSchema column)
	{
		return GetParamSize(column.NativeType, column.Size);
	}
	
	public static int GetParamSize(ViewColumnSchema column)
	{
		return GetParamSize(column.NativeType, column.Size);
	}
	
	public static string GetComma(ColumnSchema column,ColumnSchemaCollection columns)
	{
		if (column.Name != columns[columns.Count-1].Name)
			return ",";
		else
			return "";
	}
	
	public static string GetAnd(ColumnSchema column,ColumnSchemaCollection columns)
	{
		if (column.Name != columns[columns.Count-1].Name)
		{
			return "And";
		}
		else
		{
			return "";
		}
	}

    public static bool IsAutoIncreaseField(ColumnSchema column)
    { 
        return (bool)(column.ExtendedProperties["CS_IsIdentity"].Value);
    }

    public static bool IsAutoIncreaseField(TableSchema table)
    {
        foreach (ColumnSchema column in table.Columns)
        {
            if ((bool)(column.ExtendedProperties["CS_IsIdentity"].Value)== true)
            {
                return true;
            }
        }

        return false;
    }

    public static string GetPrimaryKeyType(TableSchema table)
    {
        if (table.PrimaryKey != null)
        {
            if (table.PrimaryKey.MemberColumns.Count == 1)
                return GetCType(table.PrimaryKey.MemberColumns[0]);
        }
        return "";
    }

    //是否是值类型PK
    public bool IsValuePK(ColumnSchema column)
    {
        string type = GetCType(column);

        return type == "int" 
            || type == "bool"
            || type == "decimal"
            || type == "double"
            || type == "short"
            || type == "float"
            || type == "ulong";
    }

    public bool IsStringPK(ColumnSchema column)
    { 
        string type = GetCType(column);

        return type == "string" 
            || type == "Guid"
            || type == "DateTime";
    }
	
	public static string GetIdType(ViewColumnSchema c)
	{
		if (c.DataType == DbType.Int32)
			return "int";
		else
			return "Guid";
	}
	
	public static string GetPKName(TableSchema table)
	{
		StringBuilder sb = new StringBuilder();
		foreach(ColumnSchema column in table.PrimaryKey.MemberColumns)
		{
			sb.Append("[");
			sb.Append(column.Name);
			sb.Append("]");
			sb.Append(GetComma(column, table.PrimaryKey.MemberColumns));
		}
		return sb.ToString();
	}
	
	public static string GetFirstPKName(TableSchema tv)
	{
		return tv.PrimaryKey.MemberColumns[0].Name;
	}

    public static string GetFirstPKDbType(TableSchema tv)
	{
        return GetSqlDbType(tv.PrimaryKey.MemberColumns[0].NativeType);
	}

    public static int GetFirstPKSize(TableSchema tv)
	{
        return GetParamSize(tv.PrimaryKey.MemberColumns[0]);
	}
	
	public static string GetClassName(TableSchema table)
	{
		return GetClassName(table.Name);
	}
	
	public static string GetClassNames(TableSchema table)
	{
		return GetClassNames(table.Name);
	}
	
	public static string GetClassName(ViewSchema view)
	{
		return string.Format("{0}", GetClassName(view.Name));
	}
	
	public static string GetClassNames(ViewSchema view)
	{
		return string.Format("{0}", GetClassNames(view.Name));
	}
	
	public static string GetClassNameLow(TableSchema table)
	{
		return ConvertFirstLowerStr(GetClassName(table));
	}
	
	public static string GetClassNameLow(ViewSchema view)
	{
		return ConvertFirstLowerStr(GetClassName(view));
	}
	
	public static string GetClassName(string tvName)
    {
		string ret = string.Empty;
		string className = tvName;
		
        className = SubLeft(className);
        className = SubLeft(className);
		
        //if (className.EndsWith("s"))
        //{
		//	if(className.EndsWith("ies"))
		//		className = className.Substring(0, className.Length - 3) + "y";
		//	else if(className.EndsWith("ses"))
		//		className = className.Substring(0, className.Length - 3) + "s";
		//	else
		//		className= className.Substring(0, className.Length - 1);
        //}

        ret = className.Substring(0,1).ToUpper()+className.Substring(1);
		return ret;
    }
	
	public static string GetClassNames(string tvName)
	{
		string ret = string.Empty;
		string className = GetClassName(tvName);
		
		if (className.EndsWith("y"))
			className = className.Substring(0, className.Length - 1) + "ies";
		else if (className.EndsWith("s"))
			className += "es";
		else
			className += "s";

        ret = className.Substring(0,1).ToUpper()+className.Substring(1);
		return ret;
	}
	
	public static string GetPropDes(ColumnSchema column)
	{
		if (column.Description == string.Empty)
			return column.Name;
	    else
			return column.Description;
	}
	
	public static string GetPropDes(ViewColumnSchema column)
	{
		if (column.Description == string.Empty)
			return column.Name;
	    else
			return column.Description;
	}
	
	public static string GetparameteryName(TableSchema table)
	{
		return table.Name.Substring(0,1).ToLower()+table.Name.Substring(1);
	}
	
	public static string GetFiledName(TableSchema table)
	{
		string ret;
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach(ColumnSchema column in table.Columns)
		{
			sb.Append(GetCType(column));
			sb.Append(" ");
			sb.Append(GetFiledName(column));
			sb.Append(", ");
		}
		ret = sb.ToString();
		if (!string.IsNullOrEmpty(ret))
		 	return ret.Substring(0, ret.Length - 2);
		
		return string.Empty;
	}
	
	public static string GetFiledName(ViewSchema view)
	{
		string ret;
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach(ViewColumnSchema column in view.Columns)
		{
			sb.Append(GetCType(column, view.ExtendedProperties));
			sb.Append(" ");
			sb.Append(GetFiledName(column));
			sb.Append(", ");
		}
		ret = sb.ToString();
		if (!string.IsNullOrEmpty(ret))
		 	return ret.Substring(0, ret.Length - 2);
		
		return string.Empty;
	}

    public static string GetFiledName(ColumnSchema column)
    {
        return column.Name.Substring(0, 1).ToLower() + column.Name.Substring(1);
    }
	
	public static string GetFiledName(ViewColumnSchema column)
    {
        return column.Name.Substring(0, 1).ToLower() + column.Name.Substring(1);
    }

    public static string GetPropertyName(ColumnSchema column)
    {
        return column.Name.Substring(0, 1).ToUpper() + column.Name.Substring(1);
    }
	
	public static string GetPropertyName(ViewColumnSchema column)
    {
        return column.Name.Substring(0, 1).ToUpper() + column.Name.Substring(1);
    }
	
    //added on 2017-05-05
    public static string GetSqlParaValue(ColumnSchema column)
    {
        string fieldName = "entity." + column.Name.Substring(0, 1).ToUpper() + column.Name.Substring(1);
        
        string dbType =  GetSqlDbType(column);
        if(dbType == "DateTime" || dbType == "Date")
			return string.Format("{0}.ToSafeDate()", fieldName);
            //return string.Format("SqlHelper.ToSafeDate({0})", fieldName);
        
        return fieldName;
    }
	
	public static string GetTVName(TableSchema table)
	{
		return table.Name.ToString();	
	}
	
	public static string GetTVName(ViewSchema view)
	{
		return view.Name.ToString();	
	}
	
	public static string GetTVRemark(TableSchema table)
	{
		if (table.ExtendedProperties["remark"] != null)
			return table.ExtendedProperties["remark"].Value.ToString();	
		else
			return GetClassName(table);
	}
	
	public static string GetSpRemark(CommandSchema comm)
	{
		if (comm.ExtendedProperties["remark"] != null)
			return comm.ExtendedProperties["remark"].Value.ToString();	
		else
			return string.Format("Genarated by stored procedure '{0}'", comm.Name);
	}
	
	public static string GetTVRemark(ViewSchema view)
	{
		if (view.ExtendedProperties["remark"] != null)
			return view.ExtendedProperties["remark"].Value.ToString();	
		else
			return GetClassName(view);
	}
	
	public static string GetViewObjStr(TableSchema tv)
	{
		if (GetCType(tv.Columns["Id"]) == "int")
		    return "ViewIntObj";
		else
			return "ViewObj";
	}
	
	public static string GetViewObjStr(ViewSchema tv)
	{
		if (GetCType(tv.Columns["Id"], tv.ExtendedProperties) == "int")
		   return "ViewIntObj";
		else
			return "ViewObj";
	}
	
	public static bool HasIdColumn(TableSchema tv)
	{
		if (tv.PrimaryKey.MemberColumns.Count != 1)
		    return false;
			
		foreach(ColumnSchema column in tv.Columns)
		{
			if (column.Name == "Id")
				return true;
		}
		return false;
	}
	
	public static bool HasIdColumn(ViewSchema tv)
	{
		foreach(ViewColumnSchema column in tv.Columns)
		{
			if (column.Name == "Id")
				return true;
		}
		return false;
	}
	
	public static bool HadBankId(TableSchema table)
	{
		foreach(ColumnSchema column in table.Columns)
		{
			if (column.Name == "BankId")
				return true;
		}
		return false;
	}
	
	public static bool HadBankId(ViewSchema view)
	{
		foreach(ViewColumnSchema column in view.Columns)
		{
			if (column.Name == "BankId")
				return true;
		}
		return false;
	}
	
	public static bool IsIntPk(TableSchema tv)
	{
		if (GetPrimaryKeyType(tv) == "int")
			return true;
		return false;
	}
	
	public static string GetMorePrimaryOutput(TableSchema Table,ColumnSchema column)
	{
		if(Table.PrimaryKey.MemberColumns.Count >1)
		{
			return "";
		}
		else
		{
			if(column.IsPrimaryKeyMember)
			{
				return "Output";	
			}
			else
			{
				return "";
			}
		}
	}
	
	public static string GetInsertParam(TableSchema Table,ColumnSchema column)
	{
		if(Table.PrimaryKey.MemberColumns.Count >1)
		{
			return "["+column.Name+"]";	
		}
		else
		{
			if(column.IsPrimaryKeyMember)
			{
				return "";
			}
			else
			{
				return "["+column.Name+"]";	
			}
		}
	}
	
    public static string GetInsertInOrOutParam(TableSchema Table,ColumnSchema column)
	{
		if(Table.PrimaryKey.MemberColumns.Count >1)
		{
			return  "@"+column.Name;	
		}
		else
		{
			if(column.IsPrimaryKeyMember)
			{
				return "";
			}
			else
			{
				return "@"+column.Name;	
			}
		}
	}
	
	public static string GetMorePrimaryAt(TableSchema Table,ColumnSchema column)
	{
		if(Table.PrimaryKey.MemberColumns.Count >1)
		{
			return "@";	
		}
		else
		{
			if(column.IsPrimaryKeyMember)
			{
				return "";
			}
			else
			{
				return "@";	
			}
		}	
	}
	
	public static string GetMorePrimaryComma(TableSchema Table,ColumnSchema column,ColumnSchemaCollection columns)
	{
		if(Table.PrimaryKey.MemberColumns.Count >1)
		{
			return GetComma(column,columns);	
		}
		else
		{
			if(column.IsPrimaryKeyMember)
			{
				return "";
			}
			else
			{
				return GetComma(column,columns);	
			}
		}
	}
	
	public static bool IsMorePrimary(TableSchema Table)
	{
		if(Table.PrimaryKey.MemberColumns.Count >1)
		{
			return true;	
		}
		else
		{
			return false;
		}
	}
	
	public static string GetMorePrimaryEqual(TableSchema Table,ColumnSchema column)
	{
		if(Table.PrimaryKey.MemberColumns.Count >1)
		{
			return "=";	
		}
		else
		{
			if(column.IsPrimaryKeyMember)
			{
				return "";
			}
			else
			{
				return "=";	
			}
		}		
	}
	
	public static string ConvertFirstLowerStr(string str)
	{
		if (!string.IsNullOrEmpty(str))
			return str.Substring(0, 1).ToLower() + str.Substring(1);
			
		return string.Empty;
	}
	
	public static string GetSortStr(TableSchema table)
	{
		if (HadColumnName(table, "Order"))
			return "[Order] asc";
		else if (HadColumnName(table, "CreateDate"))
			return "CreateDate DESC";
		else
			return string.Format("{0} DESC", table.PrimaryKey.MemberColumns[0].Name);
	}
	
	public static string GetColumnSqlValue(ColumnSchema column)
	{
		if (column.ExtendedProperties["enum"] != null)
			return string.Format("((long){0}).ToString()", GetFiledName(column));
		else
			return string.Format("{0}.ToString()", GetFiledName(column));
	}
	
	public static string GetColumnSqlValue(ViewColumnSchema column, 
			SchemaExplorer.ExtendedPropertyCollection extendedProperties)
	{
		if (extendedProperties["enum"] != null)
		{
			string retEnum = GetEnumFromEp(extendedProperties["enum"].Value.ToString(), column.Name);
			if (retEnum != null)
				return string.Format("((long){0}).ToString()", GetFiledName(column));
		}
        return string.Format("{0}.ToString()", GetFiledName(column));
	}
	
        public static string SubLeft(string str)
        {
            //shop_get_this => get_this
            //shop__get__this => get__this
            str = str.Substring(str.IndexOf("_") + 1);
            if (str.Substring(0, 1) == "_")
                str = str.Substring(1);
            return str;
        }

        public static string SubRight(string str)
        {
            //shop_get_this => shop_get
            //shop__get__this => shop__get
            str = str.Substring(0, str.LastIndexOf("_"));
            if (str.Substring(str.Length - 1) == "_")
                str = str.Substring(0, str.Length - 1);
            return str;
        }

        public static string SubFirst(string str)
        {
            //shop_get_this => Shop
            //shop__get__this => Shop
            str = str.Substring(0, str.IndexOf("_"));
            return str;
        }

        public static string SubLast(string str)
        {
            //shop_get_this => this
            //shop__get__this => this
            str = str.Substring(str.LastIndexOf("_") + 1);
            return str;
        }

        public static bool Had__(string spName)
        {
            if (spName.IndexOf("__") == -1)
                return false;
            else
                return true;
        }
	
	#endregion
	
	#region GetPkString
	public static string GetPKString(TableSchema tv)
	{
		//例如:id, id2
		StringBuilder sb = new StringBuilder();
		foreach(ColumnSchema column in tv.Columns)
		{
			if (column.IsPrimaryKeyMember)
			{
				sb.Append(ConvertFirstLowerStr(column.Name));
				sb.Append(", ");
			}
		}
		string ret = sb.ToString();
		if (ret != ", ")
			ret = ret.Substring(0, ret.Length-2);
		else
			ret = "";
		return ret;
	}

	public static string GetPKString2(TableSchema tv)
	{
		//例如:id:{0}, id2{1}
		StringBuilder sb = new StringBuilder();
		int p = 0;
		for (int i = 0; i < tv.Columns.Count; i++)
		{
			if (tv.Columns[i].IsPrimaryKeyMember)
			{
				sb.Append(tv.Columns[i].Name);
				sb.Append(":{" + p + "}, ");
				p++;
			}
		}
	
		string ret = sb.ToString();
		if (ret != ", ")
			ret = ret.Substring(0, ret.Length-2);
		else
			ret = "";
		return ret;
	}
	
	public static string GetPKString3(TableSchema tv)
	{
		//例如:Guid id, Guid id2
		StringBuilder sb = new StringBuilder();
		foreach(ColumnSchema column in tv.Columns)
		{
			if (column.IsPrimaryKeyMember)
			{
				sb.Append(string.Format("{0} ", GetCType(column)));
				sb.Append(ConvertFirstLowerStr(column.Name));
				sb.Append(", ");
			}
		}
		string ret = sb.ToString();
		if (ret != ", ")
			ret = ret.Substring(0, ret.Length-2);
		else
			ret = "";
		return ret;
	}
	
	public static string GetPKString4(TableSchema tv)
	{
		//例如:item.Id, item.Id2
		StringBuilder sb = new StringBuilder();
		foreach(ColumnSchema column in tv.Columns)
		{
			if (column.IsPrimaryKeyMember)
			{
				sb.Append("item.");
				sb.Append(column.Name);
				sb.Append(", ");
			}
		}
		string ret = sb.ToString();
		if (ret != ", ")
			ret = ret.Substring(0, ret.Length-2);
		else
			ret = "";
		return ret;
	}
	
	public static string GetPKString5(TableSchema tv)
	{
		//例如:user.Id, user.Id2
		StringBuilder sb = new StringBuilder();
		foreach(ColumnSchema column in tv.Columns)
		{
			if (column.IsPrimaryKeyMember)
			{
				sb.Append(GetClassNameLow(tv));
				sb.Append(".");
				sb.Append(column.Name);
				sb.Append(", ");
			}
		}
		string ret = sb.ToString();
		if (ret != ", ")
			ret = ret.Substring(0, ret.Length-2);
		else
			ret = "";
		return ret;
	}
	#endregion
	
	#region Custom Stored Procedure
	public static List<SpMethod> GetSpMethod(SchemaExplorer.DatabaseSchema database)
	{
		List<SpMethod> ret = new List<SpMethod>();
		foreach(CommandSchema comm in database.Commands)
		{
			if (IsCustomSp(comm.Name))
			{
				SpMethod item = new SpMethod();
				item.Name = GetSpMethodName(comm.Name);
				item.SpName = comm.Name;
				item.ReturnTypeStr = GetSpMethodTypeStr(comm.Name);
				item.ReturnType = GetSpMethodType(comm.Name);
				item.Remark = GetSpRemark(comm);
				item.EntityName = GetReturnValueName(comm.Name);
				item.Paras = GetSpParas(comm);
			    ret.Add(item);
			}
		}
		return ret;
	}
	
	public static List<Para> GetSpParas(CommandSchema command)
	{
		List<Para> ret = new List<Para>();
		foreach(SchemaExplorer.ParameterSchema i in command.Parameters)
		{
			if (i.Direction == System.Data.ParameterDirection.ReturnValue) 
				continue;
			Para item = new Para();
			item.MethodName = ConvertFirstLowerStr(i.Name.Substring(1));
			item.MethodType = GetCType(i, command.ExtendedProperties);
			item.SpName = i.Name;
			item.SpType = GetSqlDbType(i.NativeType);
			item.SpSize = GetParamSize(i).ToString();
			item.Remark = item.MethodName;
			item.Direction = i.Direction;
			
			ret.Add(item);
		}
		return ret;
	}
	
	public static string GetReturnValueName(string spName)
	{
		//pbcsms_Inspections_Inspection*_GetInspectionsByBankId => Inspection
		string ret = SubRight(spName);
		ret = SubLeft(ret);
		ret = SubLeft(ret);
		return ret;
	}
	
	public static string GetSpMethodName(string spName)
	{
		//pbcsms_Inspections_Inspection*_GetInspectionsByBankId => GetInspectionsByBankId
		return SubLast(spName);
	}
	
	public static string GetSpMethodTypeStr(string spName)
	{
		//pbcsms_Inspections_Inspection*_GetInspectionsByBankId => List<Inspection>
		//pbcsms_Inspections_Inspection_GetInspectionByBankId => Inspection
		string ret = GetReturnValueName(spName);
		if (Had__(spName))
			return string.Format("List<{0}>", ret);
		else
			return ret;
	}
	
	public static ReturnType GetSpMethodType(string spName)
	{
		//pbcsms_Inspections_Inspection*_GetInspectionsByBankId => ReturnType.EntityList
		//pbcsms_Inspections_Inspection_GetInspectionByBankId => ReturnType.Entity
		//pbcsms_Inspections_int_GetInspectionAmount => ReturnType.SingleValue
		//pbcsms_Inspections_void_GetInspectionAmount => ReturnType.Void
		string ret = GetReturnValueName(spName);
		if (Had__(spName))
			return ReturnType.EntityList;
		else if (ret == "int" || 
		         ret == "string" ||
				 ret == "guid" ||
				 ret == "datetime" ||
				 ret == "float" ||
				 ret == "decimal" ||
				 ret == "long" ||
				 ret == "ulong" ||
				 ret == "short" ||
				 ret == "ushort" ||
				 ret == "bool" ||
				 ret == "byte[]" ||
				 ret == "byte"
				)
			return ReturnType.SingleValue;
		else if (ret == "void")
		    return ReturnType.Void;
		else
			return ReturnType.Entity;
	}
	
	public static bool IsCustomSp(string spName)
	{
		string header = SubFirst(spName);
		if (header == "gen" || header == "aspnet" || header =="except")
			return false;
		else
			return true;
	}
	
	public class SpMethod
	{
		public string Name;
		public string SpName;
		public string ReturnTypeStr;
		public ReturnType ReturnType;
		public string Remark;
		public string EntityName;
		public List<Para> Paras = new List<Para>();
		public string ParasToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach(Para i in Paras)
			{
				if (i.Direction == System.Data.ParameterDirection.Output ||
					i.Direction == System.Data.ParameterDirection.InputOutput )
				{
					sb.Append("out ");
					sb.Append(i.MethodType);
					sb.Append(" ");
					sb.Append(i.MethodName);
					sb.Append(", ");
				}
				else
				{
					sb.Append(i.MethodType);
					sb.Append(" ");
					sb.Append(i.MethodName);
					sb.Append(", ");
				}
			}
			string sbStr = sb.ToString();
			string ret = sb.ToString();
			if (ret.Length > 0)
				ret = ret.Substring(0, ret.Length-2);
			else
				ret = "";
			return ret;
		}
	}
	
	public enum ReturnType
	{
		EntityList,
		Entity,
		SingleValue,
		Void
	}
	
	public class Para
	{
		public string MethodName;
		public string MethodType;
		public string SpName;
		public string SpType;
	    public string SpSize;
		public string Remark;
		public System.Data.ParameterDirection Direction;
	}
	#endregion
	
	#region RawType
	public static string GetCType(System.Data.DbType dataType, bool canNull)
	{
		switch (dataType)
        {
            case DbType.AnsiString: return "string";
            case DbType.AnsiStringFixedLength: return "string";
            case DbType.Binary: return "byte[]";
            case DbType.Boolean: return canNull ? "bool" : "bool";
            case DbType.Byte: return canNull ? "byte" : "byte";
            case DbType.Currency: return canNull ? "decimal" : "decimal";
            case DbType.Date: return canNull ? "DateTime" : "DateTime";
            case DbType.DateTime: return canNull ? "DateTime" : "DateTime";
            case DbType.Decimal: return canNull ? "decimal" : "decimal";
            case DbType.Double: return canNull ? "double" : "double";
            case DbType.Guid: return canNull ? "Guid" : "Guid";
            case DbType.Int16: return canNull ? "short" : "short";
            case DbType.Int32: return canNull ? "int" : "int";
            case DbType.Int64: return canNull ? "long" : "long";
            case DbType.Object: return canNull ? "object" : "object";
            case DbType.SByte: return canNull ? "bool" : "bool";
            case DbType.Single: return canNull ? "float" : "float";
            case DbType.String: return "string";
            case DbType.StringFixedLength: return "string";
            case DbType.Time: return canNull ? "TimeSpan" : "TimeSpan";
            case DbType.UInt16: return canNull ? "bool" : "bool";
            case DbType.UInt32: return canNull ? "int" : "int";
            case DbType.UInt64: return canNull ? "ulong" : "ulong";
            case DbType.VarNumeric: return canNull ? "decimal" : "decimal";
            default: return string.Empty;
        }
	}
	
	public static bool IsCanNullType(System.Data.DbType dataType)
	{
		switch (dataType)
        {
            case DbType.AnsiString: return true;
            case DbType.AnsiStringFixedLength: return true;
            case DbType.Binary: return true;
            case DbType.String: return true;
            case DbType.StringFixedLength: return true;
            default: return false;
        }
	}
	
    public static string GetSqlDbType(string nativeType)
	{
		switch (nativeType)
        {
            case "bigint": return "BigInt";
            case "binary": return "Binary";
            case "bit": return "Bit";
            case "char": return "Char";
            case "date": return "DateTime";
            case "datetime": return "DateTime";
            case "decimal": return "Decimal";
            case "float": return "Float";
            case "image": return "Image";
            case "int": return "Int";
            case "uint": return "Int";
            case "money": return "Money";
            case "nchar": return "NChar";
            case "ntext": return "NText";
            case "numeric": return "Float";
            case "nvarchar": return "NVarChar";
            case "real": return "Real";
            case "smalldatetime": return "SmallDateTime";
            case "smallint": return "SmallInt";
            case "smallmoney": return "SmallMoney";
            case "sql_variant": return "Variant";
            case "sysname": return "NChar";
            case "text": return "Text";
            case "tinytext": return "TinyText";
            case "longtext": return "LongText";
            case "timestamp": return "Timestamp";
            case "tinyint": return "Bit";
            case "uniqueidentifier": return "UniqueIdentifier";
            case "varbinary": return "VarBinary";
            case "varchar": return "VarChar";
            default: return "__UNKNOWN__";
        }
	}

    public static string GetSqlDbType(ColumnSchema column)
    {
 		return GetSqlDbType(column.NativeType);
    }
	
	public static int GetParamSize(string nativeType, int size)
	{
		switch (nativeType)
		{
			case "bigint": return 8;
			case "binary": return 8;
			case "bit": return 1;
			case "char": return 1;
			case "datetime": return 8;
			case "decimal": return 4;
			case "float": return 8;
			case "image": return 2147483647;
			case "int": return 4;
			case "money": return 8;
			case "nchar": return size;
			case "ntext": return 2000000;
			case "numeric": return 8;
			case "nvarchar": return size;
			case "real": return 8;
			case "smalldatetime": return 4;
			case "smallint": return 2;
			case "smallmoney": return 2;
			case "sql_variant": return size;
			case "sysname": return size;
			case "text": return 2000000;
			case "tinytext": return 2000000;
			case "longtext": return 2147483647;
			case "timestamp": return 8;
			case "tinyint": return 1;
			case "uniqueidentifier": return 16;
			case "varbinary": return 16;
			case "varchar": return size;
			default:
			{
				return -1;
			}
		}
	}
	
	public static int GetParamSize(ParameterSchema param)
	{
	    return GetParamSize(param.NativeType, param.Size);
	}
	
	public static string GetTypeAndSize(SchemaExplorer.ColumnSchema column)
	{
		string ret = String.Empty;
		ret += column.NativeType;
		if (column.NativeType == "varbinary" ||
			column.NativeType == "nvarchar" ||
			column.NativeType == "varchar" ||
			column.NativeType == "binary" ||
			column.NativeType == "char" || 
			column.NativeType == "nchar")
			ret +=  GetSize(column.Size);

        if (column.NativeType == "decimal")
            ret += "(" + column.Precision + ", " + column.Scale + ")";

		return ret;
	}
	
	public static string GetSize(int size)
	{
		switch (size)
		{
			case 0:
				return "";
			case 2147483647:
				return "";
			case 1073741823:
				return "";
			case -1:
				return "(MAX)";
			default:
				return "(" + size + ")";
		}
	}
	#endregion
	
	#region SortStr
	public static string GetSortStr(ViewSchema view)
	{
		if (HadColumnName(view, "Order"))
			return "[Order] asc";
		else if (HadColumnName(view, "CreateDate"))
			return "[CreateDate] DESC";
		else
			return view.Columns[0].Name;
	}
	
	public static string GetSortStr2(ViewSchema view)
	{
		if (HadColumnName(view, "Order"))
			return "[Order] asc";
		else if (HadColumnName(view, "CreateDate"))
			return "[CreateDate] DESC";
		return "";
	}
	
	public static bool HadColumnName(TableSchema table, string columnName)
	{
		foreach (ColumnSchema column in table.Columns)
		{
			if (column.Name.ToLower() == columnName)
				return true;
		}
		return false;
	}
	
	public static bool HadColumnName(ViewSchema view, string columnName)
	{
		foreach (ViewColumnSchema column in view.Columns)
		{
			if (column.Name.ToLower() == columnName)
				return true;
		}
		return false;
	}
	
	#endregion
    
    public static string GetNullStr(ColumnSchema c)
    {
        if (c.AllowDBNull)
        {
            if (c.NativeType == "nvarchar")
                return string.Format("Nulla,长度N{0}", c.Size);
            else if (c.NativeType == "varchar")
                return string.Format("可空,长度{0}", c.Size);
            else
                return "可空";
        }
        else
        {
            if (GetCType(c) == "string")
                return string.Format("不可空,长度{0}", c.Size);
            else
                return "不可空";
        }
    }
	
	#region Except
	public static readonly string[] ExceptList = {
		"aspnet_Applications",
		"aspnet_Membership",
		"aspnet_Paths",
		"aspnet_PersonalizationAllUsers",
		"aspnet_PersonalizationPerUser",
		"aspnet_Profile",
		"aspnet_SchemaVersions",
		"aspnet_Users",
		"aspnet_WebEvent_Events",
		
		//被排除无需生成的表
		//"RobotWorkQueue",
		
		"vw_aspnet_Applications",
		"vw_aspnet_MembershipUsers",
		"vw_aspnet_Profiles",
		"vw_aspnet_Users",
		"vw_aspnet_Roles",
		"vw_aspnet_UsersInRoles",
		"vw_aspnet_WebPartState_Paths",
		"vw_aspnet_WebPartState_Shared",
		"vw_aspnet_WebPartState_User",
		
		"vw_dp_Comments2",
		"vw_dp_CommentReplyCounts",
		"vw_dp_MComments2",
		"vw_dp_MCommentReplyCounts"
		};
		
	public static bool IsExceptTable(TableSchema table)
	{
		return IsExceptName(table.Name);
	}
	
	public static bool IsExceptName(string name)
	{		
		foreach(string s in ExceptList)
		{
			if (s == name)
				return true;
		}
		return false;
	}
	#endregion
}