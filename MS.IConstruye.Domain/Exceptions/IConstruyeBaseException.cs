using System;

namespace MS.IConstruye.Domain
{
    public class IConstruyeBaseException : Exception
    {
        public IConstruyeBaseException()
        {
        }

        public IConstruyeBaseException(string message) : base(message)
        {
        }

        public IConstruyeBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
