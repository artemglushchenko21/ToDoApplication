using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoMvc.Commands.ToDoTaskCommands
{
    public class PutToDoTaskCommand : IRequest<ToDoTask>
    {
        public ToDoTask ToDoTask { get; set; }
        public int TaskId { get; set; }

        public PutToDoTaskCommand(int id, ToDoTask toDoTask)
        {
            TaskId = id;
            ToDoTask = toDoTask;
        }
    }
}
