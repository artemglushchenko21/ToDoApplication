using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoMvc.Models.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> List(QueryOptions<T> options);
        Task<List<T>> GetList(QueryOptions<T> options);

        T Get(QueryOptions<T> options);
        Task<T> Get(int id);
        Task<T> Get(string id);

        Task Insert(T entity);
        Task Update(int id, T entity);
        Task Delete(int id);
        Task Delete(T entity);
        Task<int> Save();
    }
}
