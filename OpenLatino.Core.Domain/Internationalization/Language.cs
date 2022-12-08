using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLatino.Core.Domain.Internationalization
{
    public class Language
    {
        public Language()
        {
            AlfaInfoTranslations = new HashSet<AlfaInfoTranslation>();
            LayerTranslations = new HashSet<LayerTranslation>();
            ProviderTranslations = new HashSet<ProviderTranslations>();
        }

        public int ID { get; set; }

        public string LanguageName { get; set; }

        public bool Default { get; set; }

        public virtual ICollection<AlfaInfoTranslation> AlfaInfoTranslations { get; set; }
        public virtual ICollection<LayerTranslation> LayerTranslations { get; set; }
        public virtual ICollection<ProviderTranslations> ProviderTranslations { get; set; }
    }
}