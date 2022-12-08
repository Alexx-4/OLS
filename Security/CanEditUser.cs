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
    public class CanEditUser: IAuthorizationRequirement
    {}

    public class CanEditUserHandler : AuthorizationHandler<CanEditUser>
    {
        private IHttpContextAccessor _contextAccessor;
        private SecurityService _securityService;
        public CanEditUserHandler(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _securityService = new SecurityService(unitOfWork);
            _contextAccessor = contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditUser requirement)
        {
            if (context.Resource is AuthorizationFilterContext authContext)
            {
                var User = _contextAccessor.HttpContext.User;
                string id = "";
                try
                {
                    id = authContext.RouteData.Values["id"].ToString();
                }
                catch
                {
                    id = _contextAccessor.HttpContext.Request.Query["id"].ToString();
                }

                if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id && User.FindFirstValue(ClaimTypes.Role) != "Admin")
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
