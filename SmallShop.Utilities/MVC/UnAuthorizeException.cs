using System;

namespace SmallShop.Utilities
{
    /// <summary>
    /// 未授权异常
    /// </summary>
    [Serializable]
    public class UnAuthorizeException : Exception
    {
        public UnAuthorizeException()
            : base()
        {
        }

        public UnAuthorizeException(string message)
            : base(message)
        {

        }
    }
}
