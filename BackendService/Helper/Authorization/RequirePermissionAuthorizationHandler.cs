using Microsoft.AspNetCore.Authorization;

namespace BackendService.Helper.Authorization
{
    public class RequirePermissionAuthorizationHandler : AuthorizationHandler<RequirePermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequirePermissionRequirement requirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
