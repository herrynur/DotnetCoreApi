using Microsoft.AspNetCore.Authorization;

namespace BackendService.Helper.Authorization
{
    public class RequirePermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; set; }
        public RequirePermissionRequirement(string permission) => Permission = permission;
    }
}
