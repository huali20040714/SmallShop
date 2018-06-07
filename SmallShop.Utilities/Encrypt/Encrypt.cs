using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SmallShop.Utilities
{
    /// <summary>
    /// 加密解密助手类
    /// </summary>
    public class EncryptHelper
    {
        #region ========MD5======== 

        /// <summary> 
        /// MD5 加密函数 
        /// </summary> 
        /// <param name="content"></param> 
        /// <returns></returns> 
        public static string MD5Encode(string content)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(content);
            bs = md5.ComputeHash(bs);
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2").ToUpper());
            }
            string md5Str = sb.ToString();

            return md5Str;
        }

        #endregion

        #region ========加密======== 

        /// <summary>
        /// 加密(默认sKey=litianping)
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "litianping");
        }

        /// <summary> 
        /// 加密 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            var md5Str = MD5Encode(sKey).Substring(0, 8);
            des.Key = ASCIIEncoding.ASCII.GetBytes(md5Str);
            des.IV = ASCIIEncoding.ASCII.GetBytes(md5Str);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            return ret.ToString();
        }

        #endregion

        #region ========解密======== 

        /// <summary>
        /// 解密(默认sKey=litianping)
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "litianping");
        }

        /// <summary> 
        /// 解密 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            var md5Str = MD5Encode(sKey).Substring(0, 8);
            des.Key = ASCIIEncoding.ASCII.GetBytes(md5Str);
            des.IV = ASCIIEncoding.ASCII.GetBytes(md5Str);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        #region   DES加密 解密

        private static byte[] Keys = { 0xEF, 0xAB, 0x56, 0xCE, 0x90, 0xBA, 0xCD, 0xAE };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDes(string encryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes("abc12345");
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();

                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return "0";
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDes(string decryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes("abc12345");
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();

                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "0";
            }
        }

        #endregion

        #region 非对称加密解密

        public static void RSACreateKey(ref string str_PublicKey, ref string str_PrivateKey)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(512);

            str_PublicKey = RSA.ToXmlString(false);
            str_PrivateKey = RSA.ToXmlString(true);
        }

        public static string RSAEncrypt(string source, string publicKey)
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSA.FromXmlString(publicKey);

                byte[] DataToEncrypt = Encoding.UTF8.GetBytes(source);

                byte[] bs = RSA.Encrypt(DataToEncrypt, false);
                string encrypttxt = Convert.ToBase64String(bs);

                return encrypttxt;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static string RSADecrypt(string strRSA, string privateKey)
        {
            try
            {
                byte[] DataToDecrypt = Convert.FromBase64String(strRSA);
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSA.FromXmlString(privateKey);

                byte[] bsdecrypt = RSA.Decrypt(DataToDecrypt, false);

                string strRE = Encoding.UTF8.GetString(bsdecrypt);
                return strRE;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        #endregion
    }
}
