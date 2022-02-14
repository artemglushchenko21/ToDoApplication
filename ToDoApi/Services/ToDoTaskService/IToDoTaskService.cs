using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoMvc.Services.ToDoTaskService
{
    public interface IToDoTaskService
    {
        Task DeleteToDoTask(int id);
        Task<ActionResult<ToDoTask>> GetToDoTask(int id);
        Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTasks(string filterId);
        Task PostToDoTask(ToDoTask toDo);
        Task PutToDoTask(int id, ToDoTask toDo);
        Task ModifyTaskStatus(int taskId, string statusId);
        ToDoTask GetDefaultTask();
    }
}