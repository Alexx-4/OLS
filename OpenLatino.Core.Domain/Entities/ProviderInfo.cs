using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OpenLatino.Core.Domain.Internationalization;

namespace OpenLatino.Core.Domain.Entities
{
    public class ProviderInfo
    {
        public ProviderInfo()
        {
            Layers = new HashSet<Layer>();
            ProviderTranslations = new HashSet<ProviderTranslations>();
        }

        public int Id { get; set; }
        public string ConnectionString { get; set; }
        public string PkField { get; set; }
        public string Table { get; set; }
        public string GeoField { get; set; }
        public string BoundingBoxField { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Layer> Layers { get; set; }
        public virtual ICollection<ProviderTranslations> ProviderTranslations { get; set; }
    }
}
