using System;

namespace IOIT.Identity.Application.Common.Exceptions
{
    public class GenericException : Exception
    {
        public GenericException()
        { }

        public GenericException(string message = "One or more validation failures have occurred")
            : base(message)
        { }

        public GenericException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
