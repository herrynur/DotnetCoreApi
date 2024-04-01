using BackendService.Helper.Swagger;

namespace BackendService.Application.Cities.Dtos
{
    public class CitiesReadDto
    {
        [SwaggerSchemaExample("ccd51331-ebd8-4ac1-8133-f24bdf3cb3b0")]
        public Guid Id { get; set; }
        public long ExternalId { get; set; }
        [SwaggerSchemaExample("Jakarta")]
        public string? Name { get; set; }
        [SwaggerSchemaExample("JKT")]
        public string? Code { get; set; }
        [SwaggerSchemaExample("DKI Jakarta")]
        public string? Province { get; set; }
    }
}
