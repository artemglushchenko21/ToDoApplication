using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoMvc.Commands.ToDoTaskCommands
{
    public class PostToDoTaskCommand : IRequest<ToDoTask>
    {
        public ToDoTask ToDoTask { get; set; }

        public PostToDoTaskCommand(ToDoTask toDoTask)
        {
            ToDoTask = toDoTask;
        }
    }
}
