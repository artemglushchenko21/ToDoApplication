using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.Models.Data;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Models;
using ToDoMvc.Models.Data;
using ToDoMvc.Models.Data.Repositories;
using ToDoMvc.Queries.ToDoTaskQueries;

namespace ToDoMvc.Services.ToDoTaskService
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly IToDoRepository<ToDoTask> _toDoRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ToDoTaskService(IToDoRepository<ToDoTask> toDoRepo, IHttpContextAccessor httpContextAccessor)
        {
            _toDoRepo = toDoRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ActionResult<ToDoTask>> GetToDoTask(int id)
        {
            var toDoTask = await _toDoRepo.Get(id);

            return toDoTask;
        }

        public async Task<IEnumerable<ToDoTask>> GetToDoTasks(string filterId)
        {
            var queryOptions = QueryBuilder.BuildQueryWithFilters(GetUserId(), filterId);
            var tasks = await _toDoRepo.GetList(queryOptions);

            return tasks;
        }

        public async Task PostToDoTask(ToDoTask toDoTask)
        {
            toDoTask.UserId = GetUserId();

            await _toDoRepo.Insert(toDoTask);

            await _toDoRepo.Save();
        }

        public async Task PutToDoTask(int id, ToDoTask toDoTask)
        {
            toDoTask.UserId = GetUserId();

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

        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public ToDoTask GetDefaultTask()
        {
            var task = new ToDoTask
            {
                Description = "My first task",
                CategoryId = "home",
                DueDate = DateTime.Now,
                StatusId = "open",
                UserId = GetUserId()
            };

            return task;
         }

 
    }
}
