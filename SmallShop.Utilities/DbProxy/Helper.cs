using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Reflection;

namespace SmallShop.Utilities.DbProxy
{
    internal static class Helper
    {
        private static readonly Type TypeOfInt = typeof(int);
        private static readonly Type TypeOfLong = typeof(long);
        private static readonly Type TypeOfString = typeof(string);
        private static readonly Type TypeOfFloat = typeof(float);
        private static readonly Type TypeOfDouble = typeof(double);
        private static readonly Type TypeOfDecimal = typeof(decimal);

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

        public static object ReaderConvertToObject(IDataReader reader)
        {
            if (reader.FieldCount == 1)
                return reader[0];

            object[] ret = new object[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
                ret[i] = reader[i];

            return ret;
        }

        public static Dictionary<string, string> GetDbColumnNames(IDataReader reader)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                ret.Add(name, name);
            }
            return ret;
        }

        public static void SetPropertyValue(object classInstance, PropertyInfo tartgetProperty, object propertyValue)
        {
            var type = classInstance.GetType();

            if (propertyValue == DBNull.Value || propertyValue == null)
            {
                type.InvokeMember(tartgetProperty.Name, BindingFlags.SetProperty, Type.DefaultBinder, classInstance, new object[] { null });
            }
            else if (tartgetProperty.PropertyType.IsGenericType && tartgetProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                //如果tartgetProperty.PropertyType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                NullableConverter nullableConverter = new NullableConverter(tartgetProperty.PropertyType);
                //获取nullable的基础基元类型
                var underlyingType = nullableConverter.UnderlyingType;
                type.InvokeMember(tartgetProperty.Name, BindingFlags.SetProperty, Type.DefaultBinder, classInstance,
                    new[] { Convert.ChangeType(propertyValue, underlyingType) });
            }
            else
            {
                //如果是枚举，propertyValue先转换成指定枚举成员
                if (tartgetProperty.PropertyType.IsEnum)
                    propertyValue = Enum.ToObject(tartgetProperty.PropertyType, propertyValue);

                type.InvokeMember(tartgetProperty.Name, BindingFlags.SetProperty, Type.DefaultBinder, classInstance,
                    new[] { Convert.ChangeType(propertyValue, tartgetProperty.PropertyType) });
            }
        }

        public static string ClearAllUnsafeString(string sqlstr)
        {
            sqlstr = SqlHelper.ClearUnsafeString(sqlstr);
            return sqlstr.ToSafeSql(false);
        }

        public static T Populate<T>(IDataReader reader) where T : new()
        {
            T t;
            if (IsSystemDataType<T>())
            {
                if (reader[0] == DBNull.Value)
                    return default(T);

                t = (T)reader[0];
                return t;
            }

            //reader.GetSchemaTable().TableName
            if (typeof(T) == typeof(object))
            {
                //根据查询字段创建对象
                dynamic dyn = new ExpandoObject();
                var dic = (IDictionary<string, object>)dyn;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    dic[name] = reader[i];
                }                
                t = (T)dyn;
            }
            else
            {
                //根据泛型类属性字段赋值
                var dbColumnNames = GetDbColumnNames(reader);
                t = new T();
                foreach (var p in typeof(T).GetProperties())
                {
                    if (dbColumnNames.ContainsKey(p.Name) && reader.HasColumn(p.Name))
                    {
                        var value = reader[p.Name];
                        SetPropertyValue(t, p, value);
                    }
                }
            }
            return t;
        }

        public static bool IsSystemDataType<T>()
        {
            var typeOfT = typeof(T);
            if (typeOfT == TypeOfInt ||
                typeOfT == TypeOfLong ||
                typeOfT == TypeOfFloat ||
                typeOfT == TypeOfDouble ||
                typeOfT == TypeOfString ||
                typeOfT == TypeOfDecimal)
                return true;
            return false;
        }
    }    
}