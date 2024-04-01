using BackendService.Helper.Swagger;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Application.Companies.Dtos
{
    public class CompaniesWriteDto
    {
        [Required]
        [SwaggerSchemaExample("PT Perum Percetakan Uang Indonesia")]
        public string? Name { get; set; }
        [Required]
        [SwaggerSchemaExample("PPUI")]
        public string? Code { get; set; }
        [Required]
        [SwaggerSchemaExample("Marsha Lenathea")]
        public string? PIC { get; set; }
        [SwaggerSchemaExample("08123456789")]
        public string? PhoneNumber { get; set; }
        [SwaggerSchemaExample("Jl. Perum Percetakan Uang Indonesia No. 1")]
        public string? Address { get; set; }
    }
}
