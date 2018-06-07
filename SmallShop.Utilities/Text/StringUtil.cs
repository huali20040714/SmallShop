using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SmallShop.Utilities
{
    /// <summary>
    /// 表示字符实用工具类。
    /// </summary>
    public static class StringUtil
    {
        #region 版本号[主要用来解决js缓存问题用到，js?v=StringUtil.Version]

        private static readonly string _version = $"1.0.{DateTime.Now.Ticks}";
        public static string Version
        {
            get { return _version; }
        } 

        #endregion

        /// <summary>
        /// 将数据列表拼接成字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clips">拼接的各个部分。</param>
        /// <param name="spliter">分隔符。</param>
        /// <returns></returns>
        public static string CombineString<T>(List<T> clips, string spliter)
        {
            StringBuilder sb = new StringBuilder();
            string ret = string.Empty;
            foreach (var item in clips)
            {
                if (ret == string.Empty)
                    ret += item.ToString();
                else
                    ret += spliter + item.ToString();
            }

            return ret;
        }

        /// <summary>
        /// 将数据列表拼接成字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clips">拼接的各个部分。</param>
        /// <param name="spliter">分隔符。</param>
        /// <returns></returns>
        public static string CombineString(List<string> clips, string spliter)
        {
            return CombineString<string>(clips, spliter);
        }

        /// <summary>
        /// 将主键列表转换为SQL的IN子句。
        /// </summary>
        /// <param name="primaryKeys">主键列表。</param>
        /// <param name="primaryKeyName">主键列表</param>
        /// <returns>Where条件子句表达式。</returns>
        public static string PrimaryKeyToInClause(List<int> primaryKeys, string primaryKeyName)
        {
            if (primaryKeys.Count == 0)
                return " 1=2 ";

            string ret = primaryKeys[0].ToString();
            for (int i = 1; i < primaryKeys.Count; i++)
                ret += "," + primaryKeys[i].ToString();

            return string.Format(" {0} in ({1})", primaryKeyName, ret);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetPrefixText(string content, int length)
        {
            string result = "";

            if (!string.IsNullOrEmpty(content))
            {
                if (content.Length > length)
                    result = content.Substring(0, length) + "...";
                else
                    result = content;
            }
            return result;
        }

        public static string GetPrefix(this string content, int length)
        {
            return GetPrefixText(content, length);
        }

        public static string SubStringLeft(this string s, int leftLength)
        {
            if (s == null)
                return null;

            if (s.Length < leftLength)
                return s;

            return s.Substring(0, leftLength);
        }

        #region base64编码、解码

        /// <summary> 
        /// Base64加密 
        /// </summary> 
        /// <param name="codeName">加密采用的编码方式</param> 
        /// <param name="source">待加密的明文</param> 
        /// <returns></returns> 
        public static string EncodeBase64(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            try
            {
                source = Convert.ToBase64String(bytes);
            }
            catch { }

            return source;
        }

        /// <summary> 
        /// Base64加密，采用utf8编码方式加密 
        /// </summary> 
        /// <param name="source">待加密的明文</param> 
        /// <returns>加密后的字符串</returns> 
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary> 
        /// Base64解密 
        /// </summary> 
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param> 
        /// <param name="result">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64(Encoding encode, string result)
        {
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                result = encode.GetString(bytes);
            }
            catch { }
            return result;
        }

        /// <summary> 
        /// Base64解密，采用utf8编码方式解密 
        /// </summary> 
        /// <param name="result">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }

        #endregion

        #region 生成流水号

        private static object objLock = new object();
        public static string GenerateNumber(string format = "yy-MM-dd HH:mm:ss ff")
        {
            lock (objLock)
            {
                Thread.Sleep(10);
                string number = DateTime.Now.ToString(format).Replace(":", "").Replace("-", "").Replace(" ", "");

                return number;
            }
        }

        #endregion
    }
}
