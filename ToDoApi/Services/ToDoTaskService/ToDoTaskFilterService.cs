using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models.Data;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoMvc.Services.ToDoTaskService
{
    public class ToDoTaskFilterService : IToDoTaskFilterService
    {
        private readonly ApplicationDbContext _context;

        public ToDoTaskFilterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Category> Categories
        {
            get
            {
                return _context.Categories.ToList();
            }
        }

        public List<Status> Statuses
        {
            get
            {
                return _context.Statuses.ToList();
            }
        }
    }
}
