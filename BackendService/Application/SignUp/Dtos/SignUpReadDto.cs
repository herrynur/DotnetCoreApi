using BackendService.Helper.Swagger;

namespace BackendService.Application.SignUp.Dtos
{
    public class SignUpReadDto
    {
        [SwaggerSchemaExample("cdf76df8-d15c-4a0b-9c77-ec50dead8dac")]
        public Guid Id { get; set; }
        [SwaggerSchemaExample("Marsha Lenathea")]
        public string? Fullname { get; set; }
        [SwaggerSchemaExample("marsha")]
        public string? Username { get; set; }
    }
}
