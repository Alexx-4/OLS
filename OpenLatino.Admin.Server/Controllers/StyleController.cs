using Microsoft.AspNetCore.Mvc;
using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace OpenLatino.Admin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StyleController : ControllerBase
    {
        IStyleHelper _styleHelper;
        public StyleController(IStyleHelper styleHelper)
        {
            _styleHelper = styleHelper;
        }

        public IActionResult GetStyles()
        {
            var result = _styleHelper.GetCRUD().List();
            return new JsonResult(result);
        }

        [HttpPost("style"), Route("getImage")]
        public IActionResult getImage(VectorStyle style)
        {
            var result = _styleHelper.getImage(style);
            return new JsonResult(result);
        }

        [HttpPost("style"), Route("create")]
        public IActionResult createStyle(VectorStyle style)
        {
            style.ImageContent = _styleHelper.getImage(style);
            _styleHelper.GetCRUD().Create(style);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult deleteStyle(int id)
        {
            VectorStyle s = (VectorStyle)_styleHelper.GetCRUD().GetById(id);
            _styleHelper.GetCRUD().Remove(s);
            return Ok();
        }

        [HttpPut("model"), Route("edit")]
        public IActionResult editStyle(VectorStyle style)
        {
            style.ImageContent = _styleHelper.getImage(style);  
            _styleHelper.GetCRUD().Update(style);
            return Ok();
        }
    }
}
