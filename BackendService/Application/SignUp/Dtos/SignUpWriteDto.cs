using BackendService.Helper.Swagger;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BackendService.Application.SignUp.Dtos
{
    public class SignUpWriteDto
    {
        [Required]
        [NotNull]
        [SwaggerSchemaExample("Marsha Lenathea")]
        public string? Fullname { get; set; }
        [Required]
        [NotNull]
        [SwaggerSchemaExample("marsha")]
        public string? Username { get; set; }
        [Required]
        [NotNull]
        [SwaggerSchemaExample("password123")]
        public string? Password { get; set; }
    }
}
