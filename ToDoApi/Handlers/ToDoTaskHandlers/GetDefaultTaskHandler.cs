using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Queries.ToDoTaskQueries;

namespace ToDoMvc.Handlers.ToDoTaskHandlers
{
    public class GetDefaultTaskHandler : IRequestHandler<GetDefaultTaskQuery, ToDoTask>
    {

        public Task<ToDoTask> Handle(GetDefaultTaskQuery request, CancellationToken cancellationToken)
        {
            var task = new ToDoTask
            {
                Description = "My first task",
                CategoryId = "home",
                DueDate = DateTime.Now,
                StatusId = "open",
                UserId = request.UserId
            };

            return Task.FromResult(task);
        }
    }
}
