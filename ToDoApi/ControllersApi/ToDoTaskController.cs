﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Models.DTOs;
using ToDoMvc.Services.ToDoTaskService;

namespace ToDoMvc.ControllersApi
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
            return await _toDoTaskService.GetToDoTasks(filterId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetToDoTask(int id)
        {
            return await _toDoTaskService.GetToDoTask(id);
        }

        [HttpPost]
        public async Task<ActionResult<ToDoTask>> PostToDoTask(ToDoTask toDoTask)
        {
            await _toDoTaskService.PostToDoTask(toDoTask);

            return CreatedAtAction("GetToDoTask", new { id = toDoTask.Id }, toDoTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(int id, ToDoTask toDo)
        {
            if (id != toDo.Id) return BadRequest();

            await _toDoTaskService.PutToDoTask(id, toDo);

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