using Microsoft.AspNetCore.Mvc;
using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace OpenLatino.Admin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProviderController : ControllerBase
    {
        IProviderHelper _providerHelper;
        public ProviderController(IProviderHelper providerHelper)
        {
           _providerHelper = providerHelper;
        }

        public IActionResult GetProviders()
        {
            var result = _providerHelper.FullInfoList().ToList();
            return new JsonResult(result);
        }

        [HttpPost("model"), Route("create")]
        public IActionResult CreateProvider((string name, string description, ProviderInfo provider) model)
        {
            bool result = _providerHelper.createProvider(model.name, model.description, model.provider);
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public IActionResult deleteProvider(int id)
        {
            _providerHelper.deleteProvider(id);
            return Ok();
        }

        [HttpPut("model"), Route("edit")]
        public IActionResult editProvider((string name, string description, ProviderInfo provider) model)
        {
            bool result = _providerHelper.editProvider(model.name, model.description, model.provider);
            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        public IActionResult getProviderById(int id)
        {
            ProviderInfo _providerInfo = _providerHelper.DetailedInfo(id);

            ProviderFullInfo result = new ProviderFullInfo() { ProviderTranslation = _providerInfo.ProviderTranslations.ToList() };
            _providerInfo.ProviderTranslations = null;
            result.Provider = _providerInfo;

            return new JsonResult(result);
        }

        [HttpPost("model"), Route("SQLProviderInfo")]
        public IActionResult getProviderInfoSQL((int layerId, string table, string connString) model)
        {
            try
            {
                var result = _providerHelper.getProviderInfo(model.layerId, model.table, model.connString);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Connection string invalid");
            }
            
        }

    }
}
