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
using ToDoMvc.Models.Data;
using ToDoMvc.Models.Data.Repositories;

namespace ToDoMvc.Services.ToDoTaskService
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly Repository<ToDoTask> _toDoTaskRepo;

        public ToDoTaskService(ApplicationDbContext context)
        {
            _context = context;
            _toDoTaskRepo = new Repository<ToDoTask>(context);
        }

        public async Task<ActionResult<ToDoTask>> GetToDoTask(int id)
        {
            var toDoTask = await _toDoTaskRepo.Get(id);

            return toDoTask;
        }

        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTasks(string userId, string filterId)
        {
            var queryOptions = BuildQueryWithFilters(userId, filterId);
            var tasks = await _toDoTaskRepo.GetList(queryOptions);

            return tasks;
        }

        public async Task PostToDoTask(ToDoTask toDoTask)
        {
            await _toDoTaskRepo.Insert(toDoTask);

            await _toDoTaskRepo.Save();
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

        private bool ToDoTaskExists(int id)
        {
            return _context.ToDos.Any(e => e.Id == id);
        }

        public async Task ModifyTaskStatus(int taskId, string statusName)
        {
            var task = _context.ToDos.Find(taskId);
            task.StatusId = statusName;

            _context.ToDos.Update(task);

            await _context.SaveChangesAsync();
        }

        private static QueryOptions<ToDoTask> BuildQueryWithFilters(string userId, string filterId)
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
                    queryOptions.Where = w => w.DueDate < today;

                else if (filters.IsFuture)
                    queryOptions.Where = w => w.DueDate > today;
                else if (filters.IsToday)
                    queryOptions.Where = w => w.DueDate == today;
            }

            queryOptions.OrderBy = a => a.DueDate;
            return queryOptions;
        }
    }
}
