using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using OpenLatino.Admin.Application.Services;
using OpenLatino.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class CanAccessToWorkspace: IAuthorizationRequirement
    {}

    public class CanAccessToWorkspaceHandler : AuthorizationHandler<CanAccessToWorkspace>
    {
        private IHttpContextAccessor _contextAccessor;
        private SecurityService _securityService;
        public CanAccessToWorkspaceHandler(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _securityService = new SecurityService(unitOfWork);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanAccessToWorkspace requirement)
        {
            if (context.Resource is AuthorizationFilterContext authContext)
            {
                var User = _contextAccessor.HttpContext.User;
                int id = -1;
                try
                {
                    id = int.Parse(authContext.RouteData.Values["id"].ToString());
                }
                catch
                {
                    try
                    {
                        id = int.Parse(_contextAccessor.HttpContext.Request.Query["Wid"].ToString());
                    }
                    catch
                    {
                        context.Fail();
                        return Task.CompletedTask;                        
                    }
                    
                }

                if (id==-1 || (User.FindFirstValue(ClaimTypes.Role) != "Admin" && !_securityService.UserCanAccess(id, User.FindFirstValue(ClaimTypes.NameIdentifier))))
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
