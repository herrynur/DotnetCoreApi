namespace BackendService.Helper.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string? message, string? note = null, dynamic? data = null) : base(message)
        {
            ErrorNote = note;
            ErrorData = data;
        }

        public string? ErrorNote { get; private set; }
        public dynamic? ErrorData { get; private set; }
    }
}
