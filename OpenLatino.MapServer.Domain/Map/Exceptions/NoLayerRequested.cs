using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.MapServer.Domain.Map.Exceptions
{
    public class NoLayerRequested : BaseException
    {
        public NoLayerRequested() : base("Ninguna capa solicitada")
        {

        }

        public override IEnumerable<Tuple<string, Func<string>>> GetParsers()
        {
            throw new NotImplementedException();
        }
    }
}
