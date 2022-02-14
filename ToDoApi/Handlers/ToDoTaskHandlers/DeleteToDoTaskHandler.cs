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
    public class DeleteToDoTaskHandler : IRequestHandler<DeleteToDoTaskCommand>
    {
        private readonly IToDoRepository<ToDoTask> _toDoRepo;

        public DeleteToDoTaskHandler(IToDoRepository<ToDoTask> toDoRepo)
        {
            _toDoRepo = toDoRepo;
        }

        public async Task<Unit> Handle(DeleteToDoTaskCommand request, CancellationToken cancellationToken)
        {
            var toDoTask = _toDoRepo.Get(request.Id);

            if (toDoTask == null)
            {
                throw new Exception("Task is not found");
            }

            await _toDoRepo.Delete(request.Id);

            await _toDoRepo.Save();

            return Unit.Value;
        }
    }
}
