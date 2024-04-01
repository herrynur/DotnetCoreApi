using BackendService.Helper.Responses;

namespace BackendService.Helper.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base(ResponseMessageExtensions.Database.DataNotFound)
        {

        }

        public NotFoundException(string dataName) : base($"{dataName} Tidak Ditemukan")
        {

        }
    }
}
