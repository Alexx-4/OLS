using System;
using System.Collections.Generic;

namespace OpenLatino.MapServer.Domain.Map.Exceptions
{
    public class NoStyle : BaseException
    {
        public NoStyle(string styleName, string capa) : base($"No existe el estilo {styleName} para la capa {capa}")
        {

        }

        public override IEnumerable<Tuple<string, Func<string>>> GetParsers()
        {
            throw new NotImplementedException();
        }
    }
}
