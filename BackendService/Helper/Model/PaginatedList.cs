namespace BackendService.Helper.Model
{
    public class PaginatedList<T> where T : class
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public long Count { get; private set; }
        public IEnumerable<T> Data { get; private set; }
        public bool HasPreviousPage { get; private set; }
        public bool HasNextPage { get; private set; }
        public PaginatedList(int pageNumber, int pageSize, long count, IEnumerable<T> data, bool hasPreviousPage, bool hasNextPage)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Count = count;
            Data = data;
            HasNextPage = hasNextPage;
            HasPreviousPage = hasPreviousPage;
        }
    }
}
