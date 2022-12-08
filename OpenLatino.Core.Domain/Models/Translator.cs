using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore;

namespace OpenLatino.Core.Domain.Models
{ 
    public abstract class Translator
    {
        public abstract IEnumerable<string[]> GetTranslations();
    }
}