using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenLatino.Admin.Application.Services;
using OpenLatino.Core.Domain.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Security
{
    public class CanAccessToClient: IAuthorizationRequirement
    {}
    public class CanAccessToCLientHandler : AuthorizationHandler<CanAccessToClient>
    {
        private IHttpContextAccessor _contextAccessor;
        private SecurityService _securityService;
        public CanAccessToCLientHandler(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _securityService = new SecurityService(unitOfWork);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanAccessToClient requirement)
        {
            if (context.Resource is AuthorizationFilterContext authContext)
            {
                var User = _contextAccessor.HttpContext.User;
                var id = System.Net.WebUtility.UrlDecode(authContext.RouteData.Values["id"].ToString());
                var admin = User.FindFirstValue(ClaimTypes.Role) != "Admin";
                if (admin && !_securityService.UserCanAccess(id, User.FindFirstValue(ClaimTypes.NameIdentifier)))
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
