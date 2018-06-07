using System;
using System.Runtime.Serialization;

namespace SmallShop.Utilities.DbProxy
{
    public class NotUniqueResultException : Exception
    {
        public NotUniqueResultException()
        {
        }

        public NotUniqueResultException(string message) : base(message)
        {
        }

        public NotUniqueResultException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotUniqueResultException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}