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
    public class LayerFullInfo:Translator
    {
        [Required]
        public Layer Layer { get; set; }

        [Required]
        public IList<LayerTranslation> LayerTranslations { get; set; }

        public override IEnumerable<string[]> GetTranslations()
        {
            foreach (var item in LayerTranslations)
            {
                yield return new string[] { "name", item.EntityId.ToString(), item.LanguageId.ToString(), item.Name };
                yield return new string[] { "description", item.EntityId.ToString(), item.LanguageId.ToString(), item.Description };
            }
        }
    }
}