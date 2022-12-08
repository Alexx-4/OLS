using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLatino.Core.Domain.Entities
{
    public class TematicQuery : TematicType
    {
        public TematicQuery()
        {
            StyleConfiguration = new HashSet<StyleConfig>();
        }

        
    }
}