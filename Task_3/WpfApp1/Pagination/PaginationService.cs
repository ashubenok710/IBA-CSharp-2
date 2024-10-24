using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WpfApp1.Pagination
{
    public static class PaginationService
    {

        public static async Task<Pagination<T>> GetPagination<T>(IQueryable<T> query, int page, string orderBy, bool orderByDesc, int pageSize) where T : class
        {
            Pagination<T> pagination = new Pagination<T>
            {
                TotalItems = query.Count(),
                PageSize = pageSize,
                CurrentPage = page,
                OrderBy = orderBy,
                OrderByDesc = orderByDesc
            };

            int skip = (page - 1) * pageSize;
            var props = typeof(T).GetProperties();
            var orderByProperty = props.FirstOrDefault(n => n.GetCustomAttribute<SortableAttribute>()?.OrderBy == orderBy);

             if (orderByProperty == null)
             {
                 throw new Exception($"Field: '{orderBy}' is not sortable");
             }

             if (orderByDesc)
             {
                 pagination.Result = await query
                     .OrderByDescending(x => orderByProperty.GetValue(x))
                     .Skip(skip)
                     .Take(pageSize)
                     .ToListAsync();

                 return pagination;
             }
            
            pagination.Result = await query
                .OrderBy(x => orderByProperty.GetValue(x))
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return pagination;
        }
    }
}
