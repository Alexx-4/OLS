using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore;
using System.Collections;
using OpenLatino.Core.Domain;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Internationalization;

namespace OpenLatino.Core.Domain.Models
{
    public class ProviderFullInfo : Translator
    {
        [Required]
        public ProviderInfo Provider { get; set; }

        [Required]
        public IList<ProviderTranslations> ProviderTranslation { get; set; }

        public override IEnumerable<string[]> GetTranslations()
        {
            foreach (var item in ProviderTranslation)
            {
                yield return new string[] { "name", item.EntityId.ToString(), item.LanguageId.ToString(), item.Name };
                yield return new string[] { "description", item.EntityId.ToString(), item.LanguageId.ToString(), item.Description };
            }
        }
    }
}