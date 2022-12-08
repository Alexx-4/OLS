using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using OpenLatino.Core.Domain.Models;

namespace OpenLatino.Admin.Server.Helpers
{
    public static class ModelsTranslator
    {
        public static HtmlString GetTraductions(this HtmlHelper helper, Translator translator)
        {
            var data = translator.GetTranslations();
            var idEntity = data.First()[1];

            var innerHtml = new StringBuilder();
            foreach (var item in data)
            {
                var nameField = item[0];
                var idLanguage = item[2];
                var value = item[3];

                //innerHtml.Append(new TagBuilder($"{nameField}") { Attributes = { ["class"] = "hidden", ["idLanguage"] = $"{idLanguage}" }, InnerHtml = $"{value}" }.ToString());
            }
            //return HtmlString.Create(new TagBuilder("div") { Attributes = { ["id"] = $"entityTranslator_{idEntity}" }, InnerHtml = innerHtml.ToString() }.ToString());
            throw new NotImplementedException();
        }

        public static HtmlString GetTraductions(this HtmlHelper helper, IEnumerable<Translator> collection)
        {
            var innerHtml = new StringBuilder();

            foreach (var item in collection)
                innerHtml.Append(helper.GetTraductions(item).ToString());

            //return MvcHtmlString.Create(new TagBuilder("div") { InnerHtml = innerHtml.ToString() }.ToString());
            throw new NotImplementedException();
        }
    }
}