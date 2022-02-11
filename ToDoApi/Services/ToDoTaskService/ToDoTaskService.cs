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
        private readonly IToDoRepository<ToDoTask> _toDoRepo;

        public ToDoTaskService( IToDoRepository<ToDoTask> toDoRepo)
        {
            _toDoRepo = toDoRepo;
        }

        public async Task<ActionResult<ToDoTask>> GetToDoTask(int id)
        {
            var toDoTask = await _toDoRepo.Get(id);

            return toDoTask;
        }

        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTasks(string userId, string filterId)
        {
            var queryOptions = BuildQueryWithFilters(userId, filterId);
            var tasks = await _toDoRepo.GetList(queryOptions);

            return tasks;
        }

        public async Task PostToDoTask(ToDoTask toDoTask)
        {
            await _toDoRepo.Insert(toDoTask);

            await _toDoRepo.Save();
        }

        public async Task PutToDoTask(int id, ToDoTask toDoTask)
        {
           await _toDoRepo.Update(id, toDoTask);

           await _toDoRepo.Save();
        }

        public async Task DeleteToDoTask(int id)
        {
            await _toDoRepo.Delete(id);

            await _toDoRepo.Save();
        }

        public async Task ModifyTaskStatus(int taskId, string statusName)
        {
            await _toDoRepo.SetTaskStatus(taskId, statusName);

            await _toDoRepo.Save();
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
                {
                    queryOptions.Where = w => w.DueDate < today;
                }
                else if (filters.IsFuture)
                {
                    queryOptions.Where = w => w.DueDate > today;
                }    
                else if (filters.IsToday)
                {
                    queryOptions.Where = w => w.DueDate == today;
                }                
            }

            queryOptions.OrderBy = a => a.DueDate;
            return queryOptions;
        }
    }
}
