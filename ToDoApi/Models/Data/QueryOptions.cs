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
    }
}
