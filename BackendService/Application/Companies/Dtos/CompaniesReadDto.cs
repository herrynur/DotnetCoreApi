using BackendService.Helper.Swagger;

namespace BackendService.Application.Companies.Dtos
{
    public class CompaniesReadDto
    {
        [SwaggerSchemaExample("302f66f1-8632-4e69-8f7e-d3083907a70f")]
        public Guid Id { get; set; }
        [SwaggerSchemaExample("PT. Bank Mandiri Persero Tbk")]
        public string? Name { get; set; }
        [SwaggerSchemaExample("BMRI")]
        public string? Code { get; set; }
        [SwaggerSchemaExample("Cornelia Syafa Vanisa")]
        public string? PIC { get; set; }
        [SwaggerSchemaExample("08123456789")]
        public string? PhoneNumber { get; set; }
        [SwaggerSchemaExample("Jl. Gatot Subroto No.Kav 36-38, RT.7/RW.3, Senayan, Kec. Kby. Baru, Kota Jakarta Selatan, Daerah Khusus Ibukota Jakarta 12190")]
        public string? Address { get; set; }
    }
}
