using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PagingPackage
{
    public class PagingToolkit<T> : List<T>
    {
        public PagingToolkit(IEnumerable<T> items, int count, int pageNumber, int pageSize)
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

        public static async Task<PagingToolkit<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken ct)
        {
            var count = await source.CountAsync(ct);
            var items = await source.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);
            return new PagingToolkit<T>(items, count, pageNumber, pageSize);
        }

        public static PagingToolkit<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            return new PagingToolkit<T>(items, count, pageNumber, pageSize);
        }
    }
}
