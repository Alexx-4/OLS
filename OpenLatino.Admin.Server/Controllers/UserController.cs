using Microsoft.AspNetCore.Mvc;
using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Admin.Server.AuthViewModels;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Authorization;

namespace OpenLatino.Admin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpPost("{model}"), Route("Login")]
        public async Task<ActionResult<LoginViewModel>> Login(LoginViewModel model)
        {
            var tokenString = await _userService.Login(model.Username, model.Password, model.RememberMe);
            
            if(tokenString != null)
                return Ok(new AuthenticatedResponse { Token = tokenString });

            return BadRequest("User not registered");
        }

        [HttpPost("model"), Route("Register")]
        public async Task<ActionResult<RegisterViewModel>> Register(RegisterViewModel model)
        {
            var identityUser = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Username
            };

            if (await _userService.Register(identityUser, model.Password))
                return new JsonResult(true);

            throw new HttpResponseException(HttpStatusCode.BadRequest);

        }

        [Authorize]
        [HttpGet, Route("Logout")]
        public async Task<IActionResult> LogOff()
        {
            await _userService.Logout();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet, Route("Roles")]
        public IActionResult getRoles()
        {
            var result = _userService.getRoles();
            return new JsonResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet, Route("UserRoles")]
        public async Task<IActionResult> getUserRoles()
        {
            var result = await _userService.GetAllUsersInfo();
            return new JsonResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("user"), Route("updateRoles")]
        public async Task<IActionResult> updateRoles(UserInfo user)
        {
            await _userService.UpdateRol(user);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user"), Route("deleteUser")]
        public async Task<IActionResult> deleteUser(UserInfo user)
        {
            await _userService.DeleteUser(user);
            return Ok();
        }

    }
}
