using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pagination.Query.EntityFramework
{
    /// <summary>
    /// Provides a generic collection that supports paging operations and metadata for paged results.
    /// </summary>
    /// <remarks>PagingToolkit<T> extends List<T> to represent a single page of items along with information
    /// about the overall result set, such as total item count and page size. This class is commonly used to facilitate
    /// paginated queries in data-driven applications, allowing consumers to access both the current page of results and
    /// relevant paging metadata. Thread safety is not guaranteed; external synchronization is required if accessed
    /// concurrently.</remarks>
    /// <typeparam name="T">The type of elements contained in the paged collection.</typeparam>
    public class PagingWrap<T> : List<T>
    {
        public PagingWrap() { }

        public PagingWrap(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="PagingWrap{T}"/> containing a single page of items from the
        /// specified queryable source.
        /// </summary>
        /// <remarks>The method executes the query asynchronously and returns only the items for the
        /// requested page. The total count of items in the source is also included in the result. If the page number or
        /// page size is out of range, the returned page may contain fewer items than requested.</remarks>
        /// <param name="source">The queryable data source from which items are retrieved and paged.</param>
        /// <param name="pageNumber">The zero-based index of the page to retrieve. Must be greater than or equal to 0.</param>
        /// <param name="pageSize">The number of items to include in each page. Must be greater than 0.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PageSharp{T}"/>
        /// with the items for the specified page and the total item count.</returns>
        public static async Task<PagingWrap<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken ct)
        {
            var count = await source.CountAsync(ct);
            var items = await source.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);
            return new PagingWrap<T>(items, count, pageNumber, pageSize);
        }

        /// <summary>
        /// Creates a new instance of the paging toolkit for the specified queryable source, returning the items for the
        /// given page and page size.
        /// </summary>
        /// <remarks>The method executes the query against the data source to retrieve the total item
        /// count and the items for the requested page. If the page number or page size exceeds the bounds of the
        /// source, the returned items collection may be empty.</remarks>
        /// <param name="source">The queryable data source to paginate. Cannot be null.</param>
        /// <param name="pageNumber">The zero-based index of the page to retrieve. Must be greater than or equal to 0.</param>
        /// <param name="pageSize">The number of items to include in each page. Must be greater than 0.</param>
        /// <returns>A PagingToolkit<T> containing the items for the specified page, along with pagination metadata such as total
        /// item count, page number, and page size.</returns>
        public static PagingWrap<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            return new PagingWrap<T>(items, count, pageNumber, pageSize);
        }
    }
}
