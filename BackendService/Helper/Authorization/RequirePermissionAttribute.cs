using Microsoft.AspNetCore.Authorization;

namespace BackendService.Helper.Authorization
{
    public class RequirePermissionAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "RequirePermission";
        public RequirePermissionAttribute(string permission) => Permission = permission;
        public string Permission
        {
            get => Policy!.Substring(POLICY_PREFIX.Length);
            set => Policy = $"{POLICY_PREFIX}{value}";
        }
    }
}
