using Microsoft.AspNetCore.Mvc;
using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using OpenLatino.MapServer.Domain.Map.Filters.Enums;
using System.Linq;

namespace OpenLatino.Admin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TematicController : ControllerBase
    {
        private ITematicLayerHelper tematicLayerHelper;

        public TematicController(ITematicLayerHelper tematicLayerHelper)
        {
            this.tematicLayerHelper = tematicLayerHelper;
        }


        [HttpPost("model"), Route("QueryTematic")]
        public IActionResult createQueryTematic(TematicViewModel model)
        {
            bool result = tematicLayerHelper.createTematic(model, TematicTypes.QueryTematic);
            return new JsonResult(result);
        }


        [HttpPost("model"), Route("CategoryTematic")]
        public IActionResult createClasificationTematic(TematicViewModel model)
        {
            bool result = tematicLayerHelper.createTematic(model, TematicTypes.CategoryTematic);
            return new JsonResult(result);
        }


        [HttpPost, Route("tablesColumns")]
        public IActionResult getLayersColumns(Layer layer)
        {
            return new JsonResult(tematicLayerHelper.getTablesColumns(layer));
        }

        [HttpPost("model"), Route("categories")]
        public IActionResult getCategories((string column, string table, int layerId) model)
        {
            var layers = tematicLayerHelper.GetAllLayers();
            Layer layer = new Layer();

            layer = layers.Where(l => l.AlfaInfoes.Count > 0 &&
                                      l.AlfaInfoes.First().Table == model.table &&
                                      l.AlfaInfoes.First().LayerId == model.layerId)
                          .FirstOrDefault();

            var result = tematicLayerHelper.GetGeometryValue(layer, model.column);

            return new JsonResult(new { Operator = result.Item1 ? "==" : "Equal", Data = result.Item2});
        }

        [HttpPost("model"), Route("operator")]
        public IActionResult getOperators((string column, string table, int layerId) model)
        {
            var layers = tematicLayerHelper.GetAllLayers();
            Layer layer = new Layer();
            bool result = false;

            layer = layers.Where(l => l.AlfaInfoes.Count > 0 && 
                                      l.AlfaInfoes.First().Table == model.table && 
                                      l.AlfaInfoes.First().LayerId == model.layerId)
                          .FirstOrDefault();

            result = tematicLayerHelper.GetGeometryValue(layer, model.column).Item1;

            if (result)
                return new JsonResult("<,<=,!=,==,>,>=".Split(","));

            return new JsonResult("Contains,EndsWith,Equal".Split(","));
        }


        [HttpGet, Route("getCategoryTematics")]
        public IActionResult getCategoryTematics()
        {
            var result = tematicLayerHelper.GetStyleConfigs("TematicTypesClasification");
            return new JsonResult(result);
        }

        [HttpGet, Route("getQueryTematics")]
        public IActionResult getQueryTematics()
        {
            var result = tematicLayerHelper.GetStyleConfigs("TematicQuery");
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public IActionResult deleteTematic(int id)
        {
            TematicLayer _tematic = tematicLayerHelper.GetTematicLayer(id);
            tematicLayerHelper.DeleteTematicType(_tematic);
            tematicLayerHelper.deleteNonUsableTematics();

            return Ok();
        }

        [HttpPut("model"), Route("editCategory")]
        public IActionResult editCategoryTematic(TematicViewModel model)
        {
            bool result = tematicLayerHelper.EditTematic(model, TematicTypes.CategoryTematic);
            tematicLayerHelper.deleteNonUsableTematics();

            return new JsonResult(result);
        }

        [HttpPut("model"), Route("editQuery")]
        public IActionResult editQueryTematic(TematicViewModel model)
        {
            bool result = tematicLayerHelper.EditTematic(model, TematicTypes.QueryTematic);
            tematicLayerHelper.deleteNonUsableTematics();

            return new JsonResult(result);
        }
    }


}
