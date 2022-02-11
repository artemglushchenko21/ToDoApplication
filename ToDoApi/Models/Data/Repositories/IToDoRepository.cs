using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoMvc.Models.Data.Repositories
{
    public interface IToDoRepository<T> : IRepository<T> where T : class
    {
        Task SetTaskStatus(int id, string statusValues, string taskStatusPropertyName);
    }
}
