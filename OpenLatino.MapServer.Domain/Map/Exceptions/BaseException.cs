using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace OpenLatino.MapServer.Domain.Map.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string message) : base(message)
        {
        }

        public BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string ConvertToFormat(string format)
        {
            return GetParsers().FirstOrDefault(tuple => string.Equals(tuple.Item1, format)).Item2() ?? "";
        }

        public abstract IEnumerable<Tuple<string, Func<string>>> GetParsers();
    }
}
