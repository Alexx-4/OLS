using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Entities
{
    public class TematicTypesClasification: TematicType
    {
        public TematicTypesClasification()
        {
            StyleConfiguration = new HashSet<StyleConfig>();
        }
    }
}
