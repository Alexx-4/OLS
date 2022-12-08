using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenLatino.Admin.Application.Services;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Models;
using OpenLatino.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenLatino.Admin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientAppController : ControllerBase
    {
        private ClientService _clientAppService;

        public ClientAppController(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            this._clientAppService = new ClientService(unitOfWork, tokenService);
        }

        [HttpPost("user"), Route("userClientApps")]
        public IActionResult getClientApps(UserInfo user)
        {
            var clientApps = user.Id != null ? _clientAppService.GetAllClientsForUser(user.Id) :
                                               _clientAppService.GetAll();
            var result = clientApps.Select(c => new
            {
                Id = c.Id,
                Name = c.Name,

                UserName = c.User.UserName,
                UserId = c.ApplicationUserId,

                ApplicationType = c.ApplicationType == 0 ? "CommunApp" : "ConfidentialApp",

                Workspaces = c.ClientWorkSpaces.Select(w => new { Id = w.WorkSpaceId, Name = w.WorkSpace.Name }),
                Active = c.Active,

                AccessKey = c.AccessKey,
                UpdateKey = c.UpdateKey
            });

            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public IActionResult removeClientApp(string id)
        {
            _clientAppService.Remove(id);
            return Ok();
        }

        [HttpPost("model"), Route("create")]
        public IActionResult createClientApp(ClientModel model)
        {
            var result = _clientAppService.Register(model, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return new JsonResult(result.Item2);
        }

        [HttpPost("model"), Route("edit")]
        public IActionResult editClientApp(ClienFullInfo model)
        {
            _clientAppService.Update(model);
            return NoContent();
        }

    }
}
