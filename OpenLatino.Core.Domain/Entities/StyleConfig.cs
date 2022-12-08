using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLatino.Core.Domain.Entities
{
    [Table("StyleConfiguration")]
    public class StyleConfig
    {
        public int LayerId { get; set; }
        public int TematicLayerId { get; set; }
        public int TematicTypeId { get; set; }
        public int StyleId { get; set; }

        public virtual TematicType TematicType { get; set; }
        public virtual Layer Layer { get; set; }
        public virtual VectorStyle Style { get; set; }
        public virtual TematicLayer TematicLayer { get; set; }

    }
}