using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using OpenLatino.Core.Domain.Entities;

namespace OpenLatino.Core.Domain.Internationalization
{
    public class ProviderTranslations : ITranslation
    {
        public virtual ProviderInfo Entity { get; set; }
        public virtual Language Language { get; set; }
        public int LanguageId { get; set; }
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}