using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Entities;

namespace OpenLatino.Admin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="Admin")]
    public class AlphaInfoController : ControllerBase
    {
        IAlfaInfoHelper _alphaInfoHelper;

        public AlphaInfoController(IAlfaInfoHelper alphaInfoHelper)
        {
            _alphaInfoHelper = alphaInfoHelper;
        }

        [HttpGet]
        public IActionResult getAlphaInfos()
        {
            var result = _alphaInfoHelper.FullList();
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public IActionResult deleteAlphaInfo(int id)
        {
            _alphaInfoHelper.deleteAlphaInfo(id);
            return Ok();
        }

        [HttpPost("model"), Route("create")]
        public IActionResult CreateAlphaInfo((string name, string description, AlfaInfo alphaInfo) model)
        {
            bool result = _alphaInfoHelper.CreateAlphaInfo(model.alphaInfo, model.name, model.description);
            return new JsonResult(result);
        }

        [HttpPut("model"), Route("edit")]
        public IActionResult editAlphaInfo((string name, string description, AlfaInfo alphaInfo) model)
        {
            bool result = _alphaInfoHelper.EditAlphaInfo(model.alphaInfo, model.name, model.description);
            return new JsonResult(result);
        }
    }
}
