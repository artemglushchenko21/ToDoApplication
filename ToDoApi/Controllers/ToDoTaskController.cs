using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Models.DTOs;
using ToDoMvc.Services.ToDoTaskService;

namespace ToDoWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTaskController : ControllerBase
    {
        private readonly IToDoTaskService _toDoTaskService;

        public ToDoTaskController(IToDoTaskService toDoTaskService)
        {
            _toDoTaskService = toDoTaskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetToDoTasks(string filterId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _toDoTaskService.GetToDoTasks(userId, filterId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetToDoTask(int id)
        {
            var toDoTask = await _toDoTaskService.GetToDoTask(id);

            return toDoTask;
        }

        [HttpPost]
        public async Task<ActionResult<ToDoTask>> PostToDoTask(ToDoTask toDoTask)
        {
            toDoTask.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _toDoTaskService.PostToDoTask(toDoTask);

            return CreatedAtAction("GetToDoTask", new { id = toDoTask.Id }, toDoTask);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(int id, ToDoTask toDo)
        {
            toDo.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != toDo.Id) return BadRequest();

            var result = await _toDoTaskService.PutToDoTask(id, toDo);

            if (result == false) return NotFound();

            return NoContent();
        }

        [HttpPost("ModifyToDoTaskStatus")]
        public async Task ModifyToDoTaskStatus(TaskStatusDTO taskStatus)
        {
           await _toDoTaskService.ModifyTaskStatus(taskStatus.TaskId, taskStatus.TaskStatusId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(int id)
        {
            await _toDoTaskService.DeleteToDoTask(id);

            return NoContent();
        }
    }
}