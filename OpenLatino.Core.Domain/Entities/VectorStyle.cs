using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLatino.Core.Domain.Entities
{
    public class VectorStyle
    {
        public VectorStyle()
        {
            StyleConfiguration = new HashSet<StyleConfig>();
            Layers = new HashSet<LayerStyle>();
            LayerWorkspaces = new HashSet<LayerWorkspaces>();
        }

        public int Id { get; set; }
        public bool EnableOutline { get; set; }
        public string Fill { get; set; }
        public string Line { get; set; }
        [Display(Name = "Style Name")]
        public string Name { get; set; }
        public string OutlinePen { get; set; }
        public string PointFill { get; set; }
        public float PointSize { get; set; }
        public byte[] ImageContent { get; set; }
        public float ImageRotation { get; set; }
        public float ImageScale { get; set; }

        public virtual ICollection<LayerStyle> Layers { get; set; }
        public virtual ICollection<StyleConfig> StyleConfiguration { get; set; }
        public virtual ICollection<LayerWorkspaces> LayerWorkspaces { get; set; }
    }
}
