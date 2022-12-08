using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using OpenLatino.Core.Domain.Internationalization;

namespace OpenLatino.Core.Domain.Entities
{
    public class AlfaInfo 
    {
        public AlfaInfo()
        {
            AlfaInfoTranslations = new HashSet<AlfaInfoTranslation>();
        }

        public int Id { get; set; }
        public int LayerId { get; set; }
        public string PkField { get; set; }
        public string Table { get; set; }
        public string ConnectionString { get; set; }
        public string Columns { get; set; }

        public virtual Layer Layer { get; set; }
        public virtual ICollection<AlfaInfoTranslation> AlfaInfoTranslations { get; set; }
    }
}