using BackendService.Helper.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendService.Application.Login.Dtos
{
    public class LoginSuccessResponseReadDto
    {
        [SwaggerSchema(Description = "User Id")]
        [SwaggerSchemaExample("cf16f2c9-d0aa-4f74-8827-a6dbc4f29c85")]
        public Guid Id { get; set; }

        [SwaggerSchema(Description = "Username")]
        [SwaggerSchemaExample("marsha")]
        public string? Username { get; set; }

        [SwaggerSchema(Description = "Fullname")]
        [SwaggerSchemaExample("Marsha Lenathea")]
        public string? Fullname { get; set; }

        [SwaggerSchema(Description = "Token")]
        [SwaggerSchemaExample("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImNkZjc2ZGY4LWQxNWMtNGEwYi05Yzc")]
        public string? Token { get; set; }
    }
}
