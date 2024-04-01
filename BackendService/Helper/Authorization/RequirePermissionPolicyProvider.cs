using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BackendService.Helper.Authorization
{
    public class RequirePermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "RequirePermission";
        private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }

        public RequirePermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => BackupPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => BackupPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var permission = policyName.Substring(POLICY_PREFIX.Length);
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new RequirePermissionRequirement(permission));
                return Task.FromResult(policy.Build())!;
            }

            return BackupPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
