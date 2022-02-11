using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ToDoMvc.Models.Data
{
    public class QueryOptions<T>
    {
        public Expression<Func<T, object>> OrderBy { get; set; }

        public WhereClauses<T> WhereClauses { get; set; }
        public Expression<Func<T, bool>> Where
        {
            set
            {
                if (WhereClauses == null)
                {
                    WhereClauses = new WhereClauses<T>();
                }
                WhereClauses.Add(value);
            }
        }
        private string[] includes;

        public string Include
        {
            set
            {
                includes = value.Replace(" ", "").Split(',');
            }
        }
        public string[] GetIncludes()
        {
           return includes ?? new string[0];
        }

        public bool HasWhere => WhereClauses != null;
        public bool HasOrderBy => OrderBy != null;
    }

    public class WhereClauses<T> : List<Expression<Func<T, bool>>> { }
}
