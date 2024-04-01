using System.Net;

namespace BackendService.Helper.Model
{
    public class ResponseBaseModel<T> where T : class
    {
        public bool IsError { get; set; }
        public string? Message { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public IEnumerable<T>? Data { get; set; }
    }
}
