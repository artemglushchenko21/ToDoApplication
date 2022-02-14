﻿using System.Collections.Generic;
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
        public async Task<IEnumerable<ToDoTask>> GetToDoTasks(string filterId)
        {
            var tasks = await _mediator.Send(new GetToDoTasksQuery(filterId));
            return tasks;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetToDoTask(int id)
        {
            return await _mediator.Send(new GetToDoTaskQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult<ToDoTask>> PostToDoTask(ToDoTask toDoTask)
        {
            await _mediator.Send(new PostToDoTaskCommand(toDoTask));

            return CreatedAtAction("GetToDoTask", new { id = toDoTask.Id }, toDoTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(int id, ToDoTask toDoTask)
        {
            if (id != toDoTask.Id) return BadRequest();

            await _mediator.Send(new PutToDoTaskCommand(id, toDoTask));

            return NoContent();
        }

        [HttpPost("ModifyToDoTaskStatus")]
        public async Task ModifyToDoTaskStatus(TaskStatusDTO taskStatus)
        {
            await _mediator.Send(new ModifyTaskStatusCommand(taskStatus.TaskId, taskStatus.TaskStatusId));         
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(int id)
        {
            await _mediator.Send(new DeleteToDoTaskCommand(id));

            return NoContent();
        }
    }
}