using BackendService.Helper.Responses;
using BackendService.Helper.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendService.Helper.Model
{
    public class DefaultErrorResponse
    {
        public class BadRequest
        {
            [SwaggerSchema(Description = "Type of Error")]
            [SwaggerSchemaExample("https://tools.ietf.org/html/rfc9110#section-15.5.1")]
            public string? type { get; set; }

            [SwaggerSchema(Description = "Title of Error")]
            [SwaggerSchemaExample("Bad Request")]
            public string? title { get; set; }

            [SwaggerSchema(Description = "Status Code")]
            [SwaggerSchemaExample("400")]
            public int? status { get; set; } = 400;

            [SwaggerSchema(Description = "Detail of Error")]
            [SwaggerSchemaExample("Enter a minimum of 3 characters")]
            public string? detail { get; set; }

            [SwaggerSchema(Description = "Instance of Error")]
            [SwaggerSchemaExample("00-ffdb7c2180e8461a18be2a4996465b0c-c3fd5de3626b5e0c-00")]
            public string? traceId { get; set; }
        }
        public class NotFound
        {
            [SwaggerSchema(Description = "Type of Error")]
            [SwaggerSchemaExample("https://tools.ietf.org/html/rfc9110#section-15.5.5")]
            public string? type { get; set; }

            [SwaggerSchema(Description = "Title of Error")]
            [SwaggerSchemaExample("Not Found")]
            public string? title { get; set; }

            [SwaggerSchema(Description = "Status Code")]
            [SwaggerSchemaExample("404")]
            public int? status { get; set; } = 400;

            [SwaggerSchema(Description = "Detail of Error")]
            [SwaggerSchemaExample("Data not found")]
            public string? detail { get; set; }

            [SwaggerSchema(Description = "Instance of Error")]
            [SwaggerSchemaExample("00-ffdb7c2180e8461a18be2a4996465b0c-c3fd5de3626b5e0c-00")]
            public string? traceId { get; set; }
        }
        public class BadRequestLogin
        {
            [SwaggerSchema(Description = "Type of Error")]
            [SwaggerSchemaExample("https://tools.ietf.org/html/rfc9110#section-15.5.1")]
            public string? type { get; set; }

            [SwaggerSchema(Description = "Title of Error")]
            [SwaggerSchemaExample("Bad Request")]
            public string? title { get; set; }

            [SwaggerSchema(Description = "Status Code")]
            [SwaggerSchemaExample("400")]
            public int? status { get; set; } = 400;

            [SwaggerSchema(Description = "Detail of Error")]
            [SwaggerSchemaExample(ResponseMessageExtensions.Login.UsernameOrPasswordIncorrect)]
            public string? detail { get; set; }

            [SwaggerSchema(Description = "Instance of Error")]
            [SwaggerSchemaExample("00-ffdb7c2180e8461a18be2a4996465b0c-c3fd5de3626b5e0c-00")]
            public string? traceId { get; set; }
        }
    }
}
