using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SmallShop.Utilities
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class ExtendMethod
    {
        #region Enum

        /// <summary>
        /// HasFlag（全匹配）
        /// </summary>
        public static bool HasFlag(this Enum eNum, params Enum[] enums)
        {
            foreach (var _enum in enums)
            {
                if (!eNum.HasFlag(_enum))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// HasFlag（Any）
        /// </summary>
        public static bool HasAnyFlag(this Enum eNum, params Enum[] enums)
        {
            foreach (var _enum in enums)
            {
                if (eNum.HasFlag(_enum))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 生存枚举类型对应的所有键值对
        /// </summary>
        /// <param name="type">枚举类型的type</param>
        /// <returns></returns>
        public static Dictionary<int, string> ToDictionary(this Type type)
        {
            var result = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(type))
            {
                var value = Convert.ToInt32(item);
                result.Add(value, item.ToString());
            }

            return result;
        }

        /// <summary>
        /// 通过枚举类型的type、获取枚举的所有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T[] ToEnums<T>(this Type type) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T 要求是枚举类型");
            }

            var list = new List<T>();
            foreach (var item in Enum.GetValues(type))
            {
                var value = (T)item;
                list.Add(value);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 通过枚举类型的单个值、获取整个枚举的所有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static T[] GetEnums<T>(this Enum @enum) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T 要求是枚举类型");
            }

            var type = @enum.GetType();
            return type.ToEnums<T>();
        }

        /// <summary>
        /// 获取枚举属性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        #endregion

        #region string

        /// <summary>
        /// 基础过滤，只保留汉字、数字、字母和下划线(空格也过滤)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterToBasic(this string str)
        {
            return new Regex(@"[^(a-zA-Z0-9_\u4e00-\u9fa5)]").Replace(str, "");
        }

        /// <summary>
        /// 默认加上单引号
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="widthSingleQuotes"></param>
        /// <returns></returns>
        public static string ToSafeSql(this string sqlstr, bool widthSingleQuotes = true)
        {
            return SqlHelper.SaftSqlString(sqlstr, widthSingleQuotes);
        }

        /// <summary>
        /// 返回一个查询的sql字符串，该字符串表示包含2种查询情况（模糊和精确）。
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="isVague"></param>
        /// <returns></returns>
        public static string ToSaftVagueSql(this string sqlstr, bool isVague)
        {
            if (isVague)
            {
                sqlstr = $"%{sqlstr}%";
                return $" like {sqlstr.ToSafeSql(true)}";
            }
            else
            {
                return $" = {sqlstr.ToSafeSql(true)}";
            }
        }

        #endregion

        #region string

        /// <summary>
        /// 返回一个指定的时间字段大于等于起始时间、小于结束时间的sql字符串
        /// </summary>
        /// <param name="needSecondOtherwiseDay">格式的日期时间比较精度(true:精确到时分秒,false:精确到天数)</param>
        /// <returns></returns>
        public static string ToSaftBetweenDateSql(this string columnsName, string dtStartStr, string dtEndStr, bool needSecondOtherwiseDay)
        {
            DateTime dtStart = SqlHelper.MinDate.TryParse(dtStartStr);
            DateTime dtEnd = SqlHelper.MaxDate.TryParse(dtEndStr);
            if (dtEnd == SqlHelper.MaxDate)
                dtEnd = dtEnd.AddDays(-1);

            if (needSecondOtherwiseDay)
            {
                //精确到时分秒
                return $"{columnsName} >= '{dtStart.ToDateTime()}' and {columnsName} < '{dtEnd.AddSeconds(1).ToDateTime()}'";
            }
            else
            {
                //精确到天
                return $"{columnsName} >= '{dtStart.ToDate()}' and {columnsName} < '{dtEnd.AddDays(1).ToDate()}'";
            }
        }

        #endregion

        #region date

        /// <summary>
        /// 日期是否超出范围
        /// </summary>
        public static bool IsOutOfRange(this DateTime dt)
        {
            return dt >= SqlDateTime.MaxValue.Value || dt <= SqlDateTime.MinValue.Value;
        }

        /// <summary>
        /// 日期是否超出范围
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="defaultValue"></param>
        /// <returns>未超出范围，返回自己，否则返回defaultValue</returns>
        public static DateTime IsOutOfRange(this DateTime dt, DateTime defaultValue)
        {
            return dt >= SqlDateTime.MaxValue.Value || dt <= SqlDateTime.MinValue.Value ? defaultValue : dt;
        }

        /// <summary>
        /// 显示有效日期数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ValidDate<T>(this DateTime dt, T defaultValue)
        {
            if (dt >= SqlDateTime.MaxValue.Value || dt <= SqlDateTime.MinValue.Value)
            {
                return defaultValue;
            }
            else
            {
                var t = typeof(T);
                if (t == typeof(string))
                {
                    return (T)(object)dt.ToString("yyyy-MM-dd");
                }
                else if (t == typeof(DateTime))
                {
                    return (T)(object)dt;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public static T ValidDate<T>(this DateTime? dt, T defaultValue)
        {
            if (!dt.HasValue)
                return defaultValue;

            return dt.Value.ValidDate(defaultValue);
        }

        public static T ValidDateTime<T>(this DateTime dt, T defaultValue)
        {
            if (dt >= SqlDateTime.MaxValue.Value || dt <= SqlDateTime.MinValue.Value)
            {
                return defaultValue;
            }
            else
            {
                var t = typeof(T);
                if (t == typeof(string))
                {
                    return (T)(object)dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (t == typeof(DateTime))
                {
                    return (T)(object)dt;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public static DateTime ToSafeDate(this DateTime date)
        {
            if (date < SqlDateTime.MinValue.Value)
                return SqlDateTime.MinValue.Value;
            else if (date > SqlDateTime.MaxValue.Value)
                return SqlDateTime.MaxValue.Value;
            else
                return date;
        }

        /// <summary>
        /// 返回与当前时间相差的天数
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDays(this DateTime date)
        {
            var d = date.ToSafeDate().Subtract(DateTime.Now);
            var t = $"{d.Days}天,{d.Hours}小时{d.Minutes}分钟{d.Seconds}秒";
            if (d.TotalMilliseconds < 0)
                return $@"<span style=""color:red;"">余：{t} </span>";
            else
                return $@"<span style=""color:gray;"">余：{t} </span>";
        }

        /// <summary>
        /// 默认格式为：yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate(this DateTime date, string dataFormat = "yyyy-MM-dd")
        {
            return date.ToSafeDate().ToString(dataFormat);
        }

        /// <summary>
        /// 默认格式为：yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateTime(this DateTime date, string dataFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return date.ToSafeDate().ToString(dataFormat);
        }

        /// <summary>
        /// 小票打印默认格式 yyyy年MM月dd日 HH时mm分ss秒
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dataFormat"></param>
        /// <returns></returns>
        public static string ToPrintDateTime(this DateTime date, string dataFormat = "yyyy年MM月dd日 HH时mm分ss秒")
        {
            return date.ToSafeDate().ToString(dataFormat);
        }

        #endregion

        #region TryParse Generic

        public static T1 TryParse<T1, T2>(this T1 value, T2 parseValue) where T1 : new()
        {
            T1 outValue = new T1();
            object[] obj = new object[2];
            obj[0] = parseValue; //TryParse的第1个参数
            obj[1] = outValue; //TryParse的第2个参数

            Type t = typeof(T1);
            MethodInfo[] mi = t.GetMethods();

            //TryParse有2个重载，一般我们使用x.TryParse(y, out z)这个，即只有2个参数的:pi.Length==2
            var method = (from m in mi let pi = m.GetParameters() where m.Name.Equals("TryParse", StringComparison.Ordinal) && pi.Length == 2 select m).FirstOrDefault();
            if (method != null)
            {
                var ret = (bool)method.Invoke(null, obj);
                if (ret == false)
                    return value;
            }
            else
            {
                return value; //没有实现TryParse的，返回它自己
            }
            try
            {
                var ret = (T1)obj[1];

                //特殊情况处理
                if (t == typeof(DateTime))
                {
                    #region 时间特殊处理

                    var dt = Convert.ToDateTime(ret);
                    var df = Convert.ToDateTime(value);
                    if (dt <= SqlDateTime.MinValue.Value)
                    {
                        if (df > SqlDateTime.MinValue.Value && df < SqlDateTime.MaxValue.Value)
                            return value;
                        else
                            return (T1)(object)SqlDateTime.MinValue.Value;
                    }
                    if (dt >= SqlDateTime.MaxValue.Value)
                    {
                        if (df > SqlDateTime.MinValue.Value && df < SqlDateTime.MaxValue.Value)
                            return value;
                        else
                            return (T1)(object)SqlDateTime.MaxValue.Value;
                    }

                    #endregion
                }

                return ret;
            }
            catch
            {
                //强转失败也返回它自己
                return value;
            }
        }

        #endregion

        #region In & NotIn

        /// <summary>
        /// EqualTo(Any)
        /// </summary>
        public static bool In<T>(this T value, params T[] paras)
        {
            return paras.Contains(value);
        }

        /// <summary>
        /// NotEqual(全匹配)
        /// </summary>
        public static bool NotIn<T>(this T value, params T[] paras)
        {
            return !paras.Contains(value);
        }

        #endregion

        #region RenderMvcPartialView or RenderMvcView

        public static string RenderView(this Controller ctl, string viewPath, object model = null, bool partial = true)
        {
            var context = ctl.ControllerContext;
            ViewEngineResult viewEngineResult;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("未找到模板.");

            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;
            string result = null;
            using (var sw = new StringWriter())
            {
                var viewContent = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
                view.Render(viewContent, sw);
                result = sw.ToString();
            }

            return result;
        }

        #endregion

        #region 金额显示格式化

        public static string ToDecimalString(this Decimal digit, string defaultValue = null)
        {
            if (digit == 0 && defaultValue != null)
                return defaultValue;

            return digit.ToString("#0.####");
        }

        #endregion

        #region 数字格式化显示

        public static string ToDecimalString(this int digit, string defaultValue)
        {
            if (digit == 0 && defaultValue != null)
                return defaultValue;
            return digit.ToString();
        }

        #endregion        
    }

    /// <summary>
    /// 枚举方法扩展
    /// </summary>
    public class Enum<T> : IEnumerable<T> where T : struct, IConvertible
    {
        #region Business Methods

        /// <summary>
        /// 返回类型为 IEnumerable&lt;T&gt; 的输入。
        /// </summary>
        /// <returns>类型为 IEnumerable&lt;T&gt; 的序列。</returns>
        public static IEnumerable<T> AsEnumerable()
        {
            return new Enum<T>();
        }

        #endregion

        #region IEnumerable<T> 成员

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>可用于循环访问集合的 IEnumerator&lt;T&gt; 。</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Enum.GetValues(typeof(T))
                .OfType<T>()
                .AsEnumerable()
                .GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>可用于循环访问集合的 IEnumerator 。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<T> 成员

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>
        /// <returns>可用于循环访问集合的 IEnumerator&lt;T&gt; 。</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

}
