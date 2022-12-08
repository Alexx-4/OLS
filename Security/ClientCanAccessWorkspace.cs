using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using OpenLatino.Admin.Application.Services;
using OpenLatino.Core.Domain.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Security
{
    public class ClientCanAccessWorkspace : IAuthorizationRequirement
    { }

    public class ClientCanAccessWorkspaceHandler : AuthorizationHandler<ClientCanAccessWorkspace>
    {
        private IHttpContextAccessor _contextAccessor;
        private SecurityService _securityService;
        public ClientCanAccessWorkspaceHandler(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _securityService = new SecurityService(unitOfWork);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClientCanAccessWorkspace requirement)
        {
            if (context.Resource is AuthorizationFilterContext authContext)
            {
                var User = _contextAccessor.HttpContext.User;
                int id = -1;
                try
                {
                    if (!_contextAccessor.HttpContext.Request.Headers.TryGetValue("Workspace", out StringValues wid) ||
                            !int.TryParse(wid, out int intId))
                        context.Fail();
                    else
                    {
                        id = intId;
                    }
                }
                catch
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                if (id == -1 || !_securityService.ClientCanAccess(id, TokenService.GetRequestToken(_contextAccessor.HttpContext.Request)))
                    context.Fail();
                else
                    context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
