using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using OpenLatino.Core.Domain.Entities;

namespace OpenLatino.Core.Domain.Internationalization
{
    public class AlfaInfoTranslation : ITranslation
    {
        public int LanguageId { get; set; }
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual AlfaInfo Entity { get; set; }
        public virtual Language Language { get; set; }
    }
}