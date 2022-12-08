using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenLatino.Admin.Application.Services;
using OpenLatino.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class CanEditWorkspace: IAuthorizationRequirement
    {}

    public class CarEditWorkspaceHandler : AuthorizationHandler<CanEditWorkspace>
    {
        private IHttpContextAccessor _contextAccessor;
        private SecurityService _securityService;
        public CarEditWorkspaceHandler(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _securityService = new SecurityService(unitOfWork);
            _contextAccessor = contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditWorkspace requirement)
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
                    id = int.Parse(_contextAccessor.HttpContext.Request.Query["Wid"].ToString());
                }

                bool notAdminOrCreator = User.FindFirstValue(ClaimTypes.Role) != "Admin" && !_securityService.UserCanAccess(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                bool commonAndNotAdmin = id == 1 && (User.FindFirstValue(ClaimTypes.Role) != "Admin");

                if (notAdminOrCreator || commonAndNotAdmin)
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
