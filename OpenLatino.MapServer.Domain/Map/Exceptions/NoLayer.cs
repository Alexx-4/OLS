using System;
using System.Collections.Generic;

namespace OpenLatino.MapServer.Domain.Map.Exceptions
{
    public class NoLayer : BaseException
    {
        public NoLayer():base("No existen capas con el nombre dado")
        {

        }

        public override IEnumerable<Tuple<string, Func<string>>> GetParsers()
        {
            throw new NotImplementedException();
        }
    }
}
