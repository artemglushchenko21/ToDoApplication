using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Models.DTOs;
using ToDoMvc.Services.ToDoTaskService;
using MediatR;
using ToDoMvc.Queries.ToDoTaskQueries;
using ToDoMvc.Commands.ToDoTaskCommands;
using Microsoft.AspNetCore.Http;

namespace ToDoMvc.ControllersApi
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoTaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetToDoTasks(string filterId)
        {
            var tasks = await _mediator.Send(new GetToDoTasksQuery(filterId));

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoTask(int id)
        {
            var task = await _mediator.Send(new GetToDoTaskQuery(id));

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> PostToDoTask(ToDoTask toDoTask)
        {
           var result =  await _mediator.Send(new PostToDoTaskCommand(toDoTask));

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(int id, ToDoTask toDoTask)
        {
            if (id != toDoTask.Id) return BadRequest();

           await _mediator.Send(new PutToDoTaskCommand(id, toDoTask));

            return NoContent();
        }

        [HttpPost("ModifyToDoTaskStatus")]
        public async Task<IActionResult> ModifyToDoTaskStatus(TaskStatusDTO taskStatus)
        {
            var result = await _mediator.Send(new ModifyTaskStatusCommand(taskStatus.TaskId, taskStatus.TaskStatusId));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(int id)
        {
            await _mediator.Send(new DeleteToDoTaskCommand(id));

            return NoContent();
        }
    }
}