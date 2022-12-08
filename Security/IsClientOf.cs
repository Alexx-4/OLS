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
    public class IsClientOf: IAuthorizationRequirement
    {}

    public class IsClientOfHandler : AuthorizationHandler<IsClientOf>
    {
        private IHttpContextAccessor _contextAccessor;
        private SecurityService _securityService;
        public IsClientOfHandler(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _securityService = new SecurityService(unitOfWork);
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsClientOf requirement)
        {
            if (context.Resource is AuthorizationFilterContext authContext)
            {
                var User = _contextAccessor.HttpContext.User;
                var id = System.Net.WebUtility.UrlDecode(authContext.RouteData.Values["id"].ToString());
                if (!_securityService.IsClientOfUser(id, User.FindFirstValue(ClaimTypes.NameIdentifier)))
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
