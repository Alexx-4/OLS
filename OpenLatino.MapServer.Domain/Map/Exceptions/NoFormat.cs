using System;
using System.Collections.Generic;

namespace OpenLatino.MapServer.Domain.Map.Exceptions
{
    public class NoFormat : BaseException
    {
        public override IEnumerable<Tuple<string, Func<string>>> GetParsers()
        {
            throw new NotImplementedException();
        }
    }
}