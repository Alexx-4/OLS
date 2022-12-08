using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenLatino.Core.Domain.Entities
{
    public class TematicLayer
    {
        public TematicLayer()
        {
            StyleConfiguration = new HashSet<StyleConfig>();
        }
        public int Id { get; set; }
        [Display(Name = "Tematic Layer Name")]
        public string Name { get; set; }
        public virtual ICollection<StyleConfig> StyleConfiguration { get; set; }
    }
}
