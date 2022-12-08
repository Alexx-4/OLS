using Microsoft.AspNetCore.Mvc;
using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Web.Http;

namespace OpenLatino.Admin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LayerController : ControllerBase
    {
        ILayerHelper _layerHelper;
        public LayerController(ILayerHelper layerHelper)
        {
            _layerHelper = layerHelper;
        }

        [Authorize]
        public IActionResult GetLayers()
        {
            List<LayerFullInfo> _layerList = _layerHelper.GetFullList().ToList();

            List<object> result = new List<object>();

            foreach (var item in _layerList)
            {
                var _layer = new
                {
                    id = item.Layer.Id,
                    providerInfoId = item.Layer.ProviderInfoId,
                    order = item.Layer.Order,

                    layerTranslations = item.LayerTranslations,

                    providerTranslations = item.Layer.ProviderInfo.ProviderTranslations.Select(t => t.Name),
                    styles = _layerHelper.GetLayerStyles(item.Layer.Id)
                };

                result.Add(_layer);
            }
            return new JsonResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult deleteLayer(int id)
        {
            _layerHelper.deleteLayer(id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("model"), Route("create")]
        public IActionResult CreateLayer((string name, string description, string[] styles, Layer layer) model)
        {
            bool result = _layerHelper.CreateLayer(model.layer, model.styles, model.name, model.description);

            if (result)
                return new JsonResult(result);

            return BadRequest("Cannot create");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("model"), Route("edit")]
        public IActionResult editLayer((string name, string description, string[] styles, Layer layer) model)
        {
            bool result = _layerHelper.EditLayer(model.layer, model.styles, model.name, model.description);

            if (result)
                return new JsonResult(result);

            return BadRequest("Cannot edit");
        }
    }
}
