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
    public class ToDoRepository<T> : Repository<T> where T: class, IToDoRepository<T>
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbset;

        public ToDoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public async Task SetTaskStatus(int id, string statusValues, string taskStatusPropertyName)
        {
            T entity = await _dbset.FindAsync(id);

            entity.GetType().GetProperty(taskStatusPropertyName).SetValue(entity, statusValues, null);

            Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
