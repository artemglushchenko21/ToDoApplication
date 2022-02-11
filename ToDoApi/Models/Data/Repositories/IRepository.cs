using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoMvc.Models.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> List(QueryOptions<T> options);

        T Get(QueryOptions<T> options);
        Task<T> Get(int id);
        Task<T> Get(string id);

        Task Insert(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task Save();
    }
}
