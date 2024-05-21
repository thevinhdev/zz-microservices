using System;
using System.Collections.Generic;
using System.Reflection;

namespace IOIT.Identity.Application.Common.Exceptions
{
    public class DbUpdateConcurrencyException : Exception
    {
        public Dictionary<string, PropertyInfo> DbValues { get; set; }
        
        public DbUpdateConcurrencyException(Dictionary<string, PropertyInfo> dbValues) 
        { }

        public DbUpdateConcurrencyException(Dictionary<string, PropertyInfo> dbValues, string message)
            : base(message)
        { }

        public DbUpdateConcurrencyException(Dictionary<string, PropertyInfo> dbValues, string message, Exception innnerException) 
            : base(message, innnerException)
        { }
    }
}
