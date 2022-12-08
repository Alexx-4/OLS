using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using OpenLatino.Core.Domain.Internationalization;

namespace OpenLatino.Core.Domain.Entities
{
    public class Layer
    {
         public Layer()
        {
            AlfaInfoes = new HashSet<AlfaInfo>();
            LayerTranslations = new HashSet<LayerTranslation>();
            StyleConfiguration = new HashSet<StyleConfig>();
            VectorStyles = new HashSet<LayerStyle>();
            LayerWorkspaces = new HashSet<LayerWorkspaces>();
        }

        public int Id { get; set; }
        public int ProviderInfoId { get; set; }
        public int Order { get; set; }

        public virtual ProviderInfo ProviderInfo { get; set; }
        public virtual ICollection<AlfaInfo> AlfaInfoes { get; set; }
        public virtual ICollection<LayerTranslation> LayerTranslations { get; set; }
        public virtual ICollection<StyleConfig> StyleConfiguration { get; set; } //Para los estilos de tematicos
        public virtual ICollection<LayerStyle> VectorStyles { get; set; } //Estilos asociados (El primero es el default)
        public virtual ICollection<LayerWorkspaces> LayerWorkspaces { get; set; }
        [NotMapped]
        [Display(Name = "Layer Name")]
        public string Name
        {
            get
            {
                return LayerTranslations.FirstOrDefault()?.Name ?? null;
            }

            set
            {
            }
        }
    }
}