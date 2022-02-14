using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Models;
using ToDoMvc.Models.Data.Repositories;
using ToDoMvc.Queries.ToDoTaskQueries;
using ToDoMvc.Services;
using ToDoMvc.Services.ToDoTaskService;

namespace ToDoMvc.Handlers
{
    public class GetToDoTasksHandler : IRequestHandler<GetToDoTasksQuery, IEnumerable<ToDoTask>>
    {
        private readonly IToDoRepository<ToDoTask> _toDoRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetToDoTasksHandler(IToDoRepository<ToDoTask> toDoRepo, IHttpContextAccessor httpContextAccessor)
        {

            _toDoRepo = toDoRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ToDoTask>> Handle(GetToDoTasksQuery request, CancellationToken cancellationToken)
        {
            var queryOptions = QueryBuilder.BuildQueryWithFilters(GetUserId(), request.FilterId);
            var tasks = await _toDoRepo.GetList(queryOptions);

            return tasks;
        }

        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
