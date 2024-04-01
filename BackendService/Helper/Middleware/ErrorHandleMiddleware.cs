using BackendService.Helper.Exceptions;
using BackendService.Helper.Responses;
using Newtonsoft.Json;
using System.Net.Mime;

namespace BackendService.Helper.Middleware
{
    public class ErrorHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (AggregateException ex)
            {
                await ProcessExceptionMessage(ex.InnerException!, httpContext);
            }
            catch (Exception ex)
            {
                await ProcessExceptionMessage(ex, httpContext);
            }
        }

        private async Task ProcessExceptionMessage(Exception ex, HttpContext httpContext)
        {
            switch (ex)
            {
                case AppException appException:
                    httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    var responseMessage = new ResponseMessage
                    {
                        Header = ResponseMessageExtensions.FailHeader,
                        Detail = appException.Message,
                        Note = appException.ErrorNote!,
                        Data = appException.ErrorData
                    };
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(responseMessage));
                    break;
                case NotFoundException:
                    await httpContext.Response.NotFoundResponse(ex.Message);
                    break;
                case UnauthorizedAccessException:
                    await httpContext.Response.UnathourizedResponse(ex.Message);
                    break;
            }
        }
    }
    public static class ErrorHandlerMiddlewareExtensions
    {
        public static void UseErrorHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandleMiddleware>();
        }
    }
}
