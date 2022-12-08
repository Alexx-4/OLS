using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OpenLatino.Admin.Application.Services;
using OpenLatino.Core.Domain.Interfaces;
using System.Threading.Tasks;

namespace OpenLatino.Admin.Server.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InfoController: ControllerBase
    {
        private InfoService _infoService;
        public InfoController(IUnitOfWork unitOfWork)
        {
            _infoService = new InfoService(unitOfWork);
        }


        [Route("")]
        [HttpGet]
        [Authorize(Policy = "ClientCanAccessWorkspace")]        
        public async Task<IActionResult> GetAvailableFunctions()
        {
            if(!Request.Headers.TryGetValue("Workspace", out StringValues wid))
                return BadRequest(new { Error= "Workspace Id should be in request headers"});

            if (!int.TryParse(wid, out int workspaceId))
                return BadRequest(new { Error = "Workspace Id should be an integer" });

            return Ok(await _infoService.GetAbleFunctions(workspaceId));
        }
    }
}
