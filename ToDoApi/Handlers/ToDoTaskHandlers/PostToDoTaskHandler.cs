using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Commands.ToDoTaskCommands;
using ToDoMvc.Models.Data.Repositories;

namespace ToDoMvc.Handlers
{

    public class PostToDoTaskHandler : IRequestHandler<PostToDoTaskCommand, ToDoTask>
    {
        private readonly IToDoRepository<ToDoTask> _toDoRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostToDoTaskHandler(IToDoRepository<ToDoTask> toDoRepository, IHttpContextAccessor httpContextAccessor)
        {
            _toDoRepo = toDoRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ToDoTask> Handle(PostToDoTaskCommand request, CancellationToken cancellationToken)
        {
            request.ToDoTask.UserId = GetUserId();

            await _toDoRepo.Insert(request.ToDoTask);

            await _toDoRepo.Save();

            return request.ToDoTask;
        }

        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
