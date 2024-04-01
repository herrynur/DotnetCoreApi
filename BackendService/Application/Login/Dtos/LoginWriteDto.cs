using BackendService.Helper.Swagger;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BackendService.Application.Login.Dtos
{
    public class LoginWriteDto
    {
        [Required]
        [NotNull]
        [SwaggerSchemaExample("admin")]
        public string? Username { get; set; }
        [Required]
        [NotNull]
        [SwaggerSchemaExample("P@ssw0rd")]
        public string? Password { get; set; }
    }
}
