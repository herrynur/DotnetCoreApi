namespace BackendService.Application.Common.Settings
{
    public class JwtAuthSetting
    {
        public string? Secret { get; set; }
        public string? Issuer { get; set; }
    }
}
