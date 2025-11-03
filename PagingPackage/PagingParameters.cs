namespace PagingPackage
{
    /// <summary>
    /// Represents a set of parameters used to control paging, filtering, and sorting of query results.
    /// </summary>
    /// <remarks>Use this class to specify the page number, page size, and optional filtering or sorting
    /// criteria when retrieving paged data from a source. The maximum allowed page size is 50; if a larger value is
    /// specified, it will be capped at this limit. This type is commonly used in APIs or data access layers to
    /// standardize pagination and query options.</remarks>
    public class PagingParameters
    {
        private const int MaxPageSize = 100;

        public int PageNumber { get; set; } = 1;
        //default page size
        private int _pageSize = 10;
        // if the user specifies a page size greater than my maxpagesize then i set it as the maxpagesize otherwise i set it as the value of the user
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string SortBy { get; set; }
        public bool SortDesc { get; set; } = false;
    }
}
