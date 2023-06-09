using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MvcVideoGames.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.CountAsync().Result;
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
        public static async Task<PaginatedList<T>> CreateAsyncSortDesc(IQueryable<T> source, int pageIndex, int pageSize, string sortOrder)
        {
            var count = source.CountAsync().Result;
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(v => EF.Property<object>(v, sortOrder)).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
        public static async Task<PaginatedList<T>> CreateAsyncSorted(IQueryable<T> source, int pageIndex, int pageSize, string sortOrder)
        {
            var count = source.CountAsync().Result;
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderBy(v => EF.Property<object>(v, sortOrder)).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
