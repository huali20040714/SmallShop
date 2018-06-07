using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;

namespace SmallShop.Utilities
{
    public class JsonHelper
    {
        private static readonly Type TypeOfInt = typeof(int);
        private static readonly Type TypeOfLong = typeof(long);
        private static readonly Type TypeOfString = typeof(string);
        private static readonly Type TypeOfFloat = typeof(float);
        private static readonly Type TypeOfDouble = typeof(double);
        private static readonly Type TypeOfDecimal = typeof(decimal);

        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            if (null == obj)
                return null;

            if (IsSystemDataType(obj.GetType()))
                obj.ToString();

            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings _jsonSettings = new JsonSerializerSettings();

            _jsonSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            _jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            _jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            _jsonSettings.Converters.Add(datetimeConverter);

            return JsonConvert.SerializeObject(obj, Formatting.None, _jsonSettings);
        }

        public static string ToJsonByTimeFormat(object obj, string timeFormat)
        {
            if (null == obj)
                return null;

            IsoDateTimeConverter dc = new IsoDateTimeConverter();
            dc.DateTimeFormat = string.IsNullOrEmpty(timeFormat) ? "yyyy-MM-dd HH:mm:ss" : timeFormat;
            JsonSerializerSettings jss = new JsonSerializerSettings();
            string uimastr = JsonConvert.SerializeObject(obj, dc);

            return uimastr;
        }

        /// <summary>
        /// 将指定的 JSON 数据反序列化成指定对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">JSON 数据。</param>
        /// <returns></returns>
        public static T FromJson<T>(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json))
                    return default(T);

                IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
                datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                JsonSerializerSettings _jsonSettings = new JsonSerializerSettings();

                _jsonSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                _jsonSettings.NullValueHandling = NullValueHandling.Ignore;
                _jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                _jsonSettings.Converters.Add(datetimeConverter);
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Result<T> ToResult<T>(string json)
        {
            var jsonObj = FromJson<Result<T>>(json);
            if (jsonObj == null)
            {
                return Result<T>.Error("网络错误,稍后重试");
            }
            else
            {
                return jsonObj;
            }
        }

        public static Result<object> ToResult(string json)
        {
            return ToResult<object>(json);
        }

        /// <summary>
        /// datatable 转换为json数据
        /// </summary>
        /// <param name="jsonName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(string jsonName, DataTable dt)
        {
            var Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");

            return Json.ToString();
        }

        public static string DataTableToJson(DataTable dt)
        {
            return JsonConvert.SerializeObject(dt, new DataTableConverter());
        }

        /// <summary>
        /// List转换为joson数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonName"></param>
        /// <param name="IL"></param>
        /// <returns></returns>
        public static string ObjectToJson<T>(string jsonName, IList<T> IL)
        {
            var jsonSb = new StringBuilder();
            jsonSb.Append("{\"" + jsonName + "\":[");
            if (IL.Count > 0)
            {
                for (int i = 0; i < IL.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    Type type = obj.GetType();
                    System.Reflection.PropertyInfo[] pis = type.GetProperties();
                    jsonSb.Append("{");
                    for (int j = 0; j < pis.Length; j++)
                    {
                        jsonSb.Append("\"" + pis[j].Name.ToString() + "\":\"" + pis[j].GetValue(IL[i], null) + "\"");
                        if (j < pis.Length - 1)
                        {
                            jsonSb.Append(",");
                        }
                    }
                    jsonSb.Append("}");
                    if (i < IL.Count - 1)
                    {
                        jsonSb.Append(",");
                    }
                }
            }
            jsonSb.Append("]}");

            return jsonSb.ToString();
        }

        /// <summary>
        /// json字符串转为哈希表
        /// </summary>
        public static Hashtable ParseJson(string strJson)
        {
            var EscapeChar = "$E_C$"; //反斜杠转义 (\\)
            var DoubleQuote = "$D_Q$"; //双引号转义 (\")
            var SingleQuote = "$S_Q$"; //单引号转义 (\')
            bool isObject, isArray;
            var keyValues = new Hashtable();
            strJson = strJson.Trim();
            isObject = strJson.StartsWith("{");
            isArray = strJson.StartsWith("[");

            if (!isObject && !isArray)
            {
                keyValues.Add("", strJson);
                return keyValues;
            }

            strJson = strJson.Replace(@"\\", EscapeChar);    //转义特殊字符   //
            strJson = strJson.Replace(@"\""", DoubleQuote);  //               //
            strJson = strJson.Replace(@"\'", SingleQuote);   ///////////////////

            if (isArray)
            {
                int i = 0;     //数组成员索引
                int qflag = 0; //中括号计数
                int bflag = 0; //花括号计数

                bool inString = false;  //进入字符串内部
                char eos = '"';         //字符串闭合符号 （单引号或双引号）
                bool endItem = false;   //找到项的结尾
                bool beginItem = false;
                bool dataIsString = false;

                StringBuilder sbItem = new StringBuilder(10);
                foreach (char c in strJson)
                {
                    if (c == '\'' || c == '"')
                    {
                        if (!inString)
                        {
                            inString = true;
                            eos = c;
                            continue;
                        }
                        else
                        {
                            if (eos == c)
                            {
                                inString = false;
                                continue;
                            }
                        }
                    }

                    if (!inString)
                    {
                        if (c == '{')
                        {
                            bflag++;
                        }
                        else if (c == '}')
                        {
                            bflag--;
                        }

                        if (bflag == 0)
                        {
                            if (c == '[')
                            {
                                qflag++;
                                continue;
                            }
                            else if (c == ']')
                            {
                                qflag--;
                                if (qflag == 0)
                                    endItem = true;
                            }
                            else if (c == ',')
                            {
                                endItem = true;
                            }
                        }
                    }

                    if (beginItem == false)
                    {
                        if (c != ' ') beginItem = true;//去除开头的空格

                        if (inString)
                        {
                            beginItem = true;
                            dataIsString = true;
                        }
                    }

                    if (endItem)
                    {
                        string temp = sbItem.ToString();

                        temp = temp.Replace(DoubleQuote, @"\""");
                        temp = temp.Replace(SingleQuote, @"\'");
                        temp = temp.Replace(EscapeChar, @"\\");

                        keyValues.Add(i, temp);

                        sbItem.Remove(0, sbItem.Length);
                        i++;
                        endItem = false;
                        beginItem = false;
                        dataIsString = false;

                        continue;
                    }

                    if (beginItem)
                    {
                        if (!dataIsString && c == ' ')
                        {
                            continue;
                        }

                        sbItem.Append(c);
                    }
                }
            }
            else if (isObject)
            {
                int sIndex = strJson.IndexOf("{");

                strJson = strJson.Substring(sIndex + 1);

                sIndex = strJson.LastIndexOf("}");

                if (sIndex != -1)
                    strJson = strJson.Substring(0, sIndex);

                strJson = strJson.Trim();
                int a = strJson.Length;

                StringBuilder sProperty = new StringBuilder(20);
                StringBuilder sValue = new StringBuilder(50);

                int flag = 0, beginFlag = 0;           //
                int eop = 0;                           //是否已经到值的末尾
                char vBeginChar = ' ', vEndChar = ' '; //值结束符
                bool vIsNumber = false, vIsString = false
                    , vIsArray = false, vIsObject = false; //当前正在解析的数据类型

                bool inQuote = false; //是否进入引号
                char quote = ' ';
                int strlen = 0;
                bool isend = false;

                foreach (char c in strJson)
                {
                    strlen++;
                    if (strlen == strJson.Length)
                    {
                        isend = true;
                    }
                    if (flag == 0)
                    {
                        if (c != ':')
                            sProperty.Append(c);
                        else
                        {
                            flag = 1;
                            beginFlag = 0;
                            vIsNumber = false;
                            vIsString = false;
                            vIsArray = false;
                            vIsObject = false;
                            inQuote = false;
                            eop = 0;
                        }
                    }
                    else if (flag == 1)
                    {
                        if (beginFlag == 0)
                        {
                            if (c == ' ' || c == '\n' || c == '\r')
                            {
                                continue;
                            }
                            else if (c == '"' || c == '\'' || c == '[' || c == '{')
                            {
                                vBeginChar = c;
                                beginFlag = 1;
                                switch (vBeginChar)
                                {
                                    case '"':
                                    case '\'':
                                        vEndChar = vBeginChar;
                                        vIsString = true;
                                        eop = 1;
                                        continue;
                                    case '[':
                                        vEndChar = ']';
                                        vIsArray = true;
                                        break;
                                    case '{':
                                        vEndChar = '}';
                                        vIsObject = true;
                                        break;
                                }
                            }
                            else
                            {
                                vBeginChar = ',';
                                vEndChar = ',';
                                beginFlag = 1;
                                eop = 1;
                                vIsNumber = true;
                            }
                        }

                        sValue.Append(c);

                        if (vIsNumber)
                        {
                            if (c == '\n' || c == '\r' || c == ',' || c == '}')
                            {
                                eop = 0;
                            }
                        }
                        else if (vIsString)
                        {
                            if (c == vEndChar)
                            {
                                eop = 0;
                            }
                        }
                        else if (vIsArray || vIsObject)
                        {
                            if (c == '\'' || c == '"')
                            {
                                if (inQuote == false)
                                {
                                    quote = c;
                                    inQuote = true;
                                }
                                else
                                {
                                    if (c == quote) inQuote = false;
                                }
                            }

                            if (!inQuote)
                            {
                                if (c == vEndChar)
                                {
                                    eop--;
                                }
                                else if (c == vBeginChar)
                                {
                                    eop++;
                                }
                            }
                        }

                        if (eop == 0 || isend)
                        {
                            if ((vIsNumber && !isend) || vIsString)
                            {
                                sValue.Remove(sValue.Length - 1, 1);
                            }

                            if (vIsNumber && (c == ',' || c == '}'))
                            {
                                flag = 0;
                            }
                            else
                            {
                                flag = 2;
                            }

                            string temp = sProperty.ToString();
                            string vtemp = sValue.ToString();

                            temp = temp.Trim('\r', '\n', ' ', '"');

                            vtemp = vtemp.Replace(DoubleQuote, @"\""");
                            vtemp = vtemp.Replace(SingleQuote, @"\'");
                            vtemp = vtemp.Replace(EscapeChar, @"\\");

                            if (keyValues.ContainsKey(temp))
                            {
                                keyValues[temp] = vtemp;
                            }
                            else
                            {
                                keyValues.Add(temp, vtemp);
                            }

                            sProperty.Remove(0, sProperty.Length);
                            sValue.Remove(0, sValue.Length);
                        }
                    }
                    else if (flag == 2)
                    {
                        if (c == ',' || c == '}')
                            flag = 0;
                    }
                }
            }
            return keyValues;
        }

        public static bool IsSystemDataType(Type typeOfT)
        {
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