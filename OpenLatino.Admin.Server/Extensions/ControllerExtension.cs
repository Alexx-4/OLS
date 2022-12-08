using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Openlatino.Admin.Infrastucture.DataContexts;

namespace OpenLatino.Admin.Server.Extensions
{
    public static class ControllerExtension
    {
        public static void SetViewDataLanguages(this Controller controller, AdminDb context)
        {
            controller.ViewData["languages"] = context.Languages.OrderBy(l => l.LanguageName).ToList();
        }

        public static void SetNameEntity(this Controller controller, string nameEntity)
        {
            controller.ViewData["entityName"] = nameEntity;
        }
    }
}