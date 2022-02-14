using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Commands.ToDoTaskCommands;
using ToDoMvc.Models.Data.Repositories;

namespace ToDoMvc.Handlers.ToDoTaskHandlers
{
    public class ModifyTaskStatusHandler : IRequestHandler<ModifyTaskStatusCommand>
    {
        private readonly IToDoRepository<ToDoTask> _toDoRepo;


        public ModifyTaskStatusHandler(IToDoRepository<ToDoTask> toDoRepo)
        {
            _toDoRepo = toDoRepo;
        }
        public async Task<Unit> Handle(ModifyTaskStatusCommand request, CancellationToken cancellationToken)
        {
            await _toDoRepo.SetTaskStatus(request.TaskId, request.TaskStatusId);

            await _toDoRepo.Save();

            return Unit.Value;
        }
    }
}
