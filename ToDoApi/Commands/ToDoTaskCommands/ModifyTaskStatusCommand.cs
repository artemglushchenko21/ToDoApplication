using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoMvc.Commands.ToDoTaskCommands
{
    public class ModifyTaskStatusCommand : IRequest
    {
        public int TaskId { get; set; }
        public string TaskStatusId { get; set; }

        public ModifyTaskStatusCommand(int taskId, string taskStatusId)
        {
            TaskId = taskId;
            TaskStatusId = taskStatusId;
        }
    }
}


