using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenLatino.Admin.Application.Services;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Models;
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
    public class WorkspaceController : ControllerBase
    {
        private WorkspaceService _workspaceService;
        private WorkspaceFunctionService _workspaceFunctionServices;

        public WorkspaceController(IUnitOfWork dbContext)
        {
            _workspaceService = new WorkspaceService(dbContext);
            _workspaceFunctionServices = new WorkspaceFunctionService(dbContext);
        }

     
        [HttpPost("user"), Route("userWorkspaces")]
        public IActionResult getWorkspaces(UserInfo user)
        {
            if (user.Id != null)
                return new JsonResult(_workspaceService.GetAll(user.Id));

            return new JsonResult(_workspaceService.GetAll());
        }

        
        [HttpDelete("{id}")]
        public IActionResult removeWorkspace(int id)
        {
            _workspaceService.Remove(id);
            return Ok();
        }

     
        [HttpGet, Route("functions")]
        public IActionResult getFunctions()
        {
            var result = _workspaceFunctionServices.GetServiceFuncions().Where(f=>f.ServiceId==1).ToList();
            return new JsonResult(result.Select(f => new
            {
                Id = f.Id,
                Name = f.Name
            }));
        }

     
        [HttpPost("model"), Route("create")]
        public IActionResult createWorkspace(WorkspaceModel model)
        {
            WorkSpace entity = new WorkSpace() { Name = model.Name };
            var result = _workspaceService.AddWorkspace(entity, User.FindFirstValue(ClaimTypes.NameIdentifier), model.LayerIds.ToList(), model.FunctionIds.ToList());
            return new JsonResult(result);
        }


        [HttpPost("model"), Route("edit")]
        public IActionResult editWorkspace(WorkspaceModel model)
        {
            _workspaceService.editWorkspace(model);
            return NoContent();
        }
    }
}
