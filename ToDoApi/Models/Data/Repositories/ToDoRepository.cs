using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ToDoApi.Models.Data;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoMvc.Models.Data.Repositories
{
    public class ToDoRepository : Repository<ToDoTask>, IToDoRepository<ToDoTask>
    {
        public ToDoRepository(ApplicationDbContext context) : base(context) { }

        public async Task SetTaskStatus(int id, string statusValue)
        {
            ToDoTask task = await _dbset.FindAsync(id);

            task.StatusId = statusValue;

            Update(task);
        }
    }
}
