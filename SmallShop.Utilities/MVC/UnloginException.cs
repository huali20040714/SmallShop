using System;

namespace SmallShop.Utilities
{
    /// <summary>
    /// 未登录异常
    /// </summary>
    [Serializable]
    public class UnloginException : Exception
    {
        public UnloginException()
        {

        }

        public UnloginException(string message) : base(message)
        {
        }

        public UnloginException(string message,Exception exception) : base(message,exception)
        {
        }
    }
}
