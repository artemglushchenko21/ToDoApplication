using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Models.Data.Repositories;
using ToDoMvc.Queries.ToDoTaskQueries;

namespace ToDoMvc.Handlers
    {
        public class GetToDoTaskHandler : IRequestHandler<GetToDoTaskQuery, ToDoTask>
        {
            private readonly IToDoRepository<ToDoTask> _toDoRepo;
 
            public GetToDoTaskHandler(IToDoRepository<ToDoTask> toDoRepo)
            {
                _toDoRepo = toDoRepo;
            }

            public async Task<ToDoTask> Handle(GetToDoTaskQuery request, CancellationToken cancellationToken)
            {
                var toDoTask = await _toDoRepo.Get(request.Id);

                return toDoTask;
            }

        }
    }
