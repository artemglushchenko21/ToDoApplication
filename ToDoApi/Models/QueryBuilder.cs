using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Models.Data;

namespace ToDoMvc.Models
{
    public static class QueryBuilder
    {
        public static QueryOptions<ToDoTask> BuildQueryWithFilters(string userId, string filterId)
        {
            var queryOptions = new QueryOptions<ToDoTask>
            {
                Include = $"{nameof(ToDoTask.Category)},{nameof(ToDoTask.Status)}",
                Where = w => w.UserId == userId
            };

            var filters = new Filters(filterId);

            if (filters.HasCategory)
            {
                queryOptions.Where = w => w.CategoryId == filters.CategoryId;
            }

            if (filters.HasStatus)
            {
                queryOptions.Where = w => w.StatusId == filters.StatusId;
            }

            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                {
                    queryOptions.Where = w => w.DueDate < today;
                }
                else if (filters.IsFuture)
                {
                    queryOptions.Where = w => w.DueDate > today;
                }
                else if (filters.IsToday)
                {
                    queryOptions.Where = w => w.DueDate == today;
                }
            }

            queryOptions.OrderBy = a => a.DueDate;
            return queryOptions;
        }
    }
}
