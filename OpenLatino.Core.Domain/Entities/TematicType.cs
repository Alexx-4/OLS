using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace OpenLatino.Core.Domain.Entities
{
    public class TematicType
    {
        public int Id { get; set; }
        [Display(Name = "TematicType Name")]
        public string Name { get; set; }
        public byte[] Function { get; set; }
        public virtual ICollection<StyleConfig> StyleConfiguration { get; set; }
    }
}
