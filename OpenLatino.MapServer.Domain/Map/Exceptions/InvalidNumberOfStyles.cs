using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.MapServer.Domain.Map.Exceptions
{
    public class InvalidNumberOfStyles:BaseException
    {
        public InvalidNumberOfStyles() : base("El número de estilos solicitados debe coincidir con el número de capas solicitadas")
        {

        }

        public override IEnumerable<Tuple<string, Func<string>>> GetParsers()
        {
            throw new NotImplementedException();
        }
    }
}
