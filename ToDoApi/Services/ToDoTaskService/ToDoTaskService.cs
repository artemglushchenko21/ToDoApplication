using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.Models.Data;
using ToDoApi.Models.ToDoTaskElements;


namespace ToDoMvc.Services.ToDoTaskService
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly ApplicationDbContext _context;
        public ToDoTaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteToDoTask(int id)
        {
            var toDo = await _context.ToDos.FindAsync(id);

            if (toDo == null)
            {
                return;
            }

            _context.ToDos.Remove(toDo);

            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<ToDoTask>> GetToDoTask(int id)
        {
            var toDoTask = await _context.ToDos.FindAsync(id);

            return toDoTask;
        }

        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTasks(string userId, string filterId)
        {
            IQueryable<ToDoTask> query = _context.ToDos
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Where(t => t.UserId == userId);

            var filters = new Filters(filterId);

            if (filters.HasCategory)
            {
                query = query.Where(t => t.CategoryId == filters.CategoryId);
            }
            if (filters.HasStatus)
            {
                query = query.Where(t => t.StatusId == filters.StatusId);
            }
            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                    query = query.Where(t => t.DueDate < today);
                else if (filters.IsFuture)
                    query = query.Where(t => t.DueDate > today);
                else if (filters.IsToday)
                    query = query.Where(t => t.DueDate == today);
            }
            var toDoTasks = await query.OrderBy(t => t.DueDate).ToListAsync();

            return toDoTasks;
        }

        public async Task PostToDoTask(ToDoTask toDoTask)
        {
             _context.ToDos.Add(toDoTask);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> PutToDoTask(int id, ToDoTask toDoTask)
        {
            _context.Entry(toDoTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTaskExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        private bool ToDoTaskExists(int id)
        {
            return _context.ToDos.Any(e => e.Id == id);        
        }

        public async Task ModifyTaskStatus(int taskId, string statusName)
        {
           var task =  _context.ToDos.Find(taskId);
           task.StatusId = statusName;

           _context.ToDos.Update(task);

           await _context.SaveChangesAsync();
        }
    }
}
