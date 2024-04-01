namespace BackendService.Helper.Api
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 0 ? pageSize : 10;
        }

        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }
    }
}
