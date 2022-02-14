using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoMvc.Commands.ToDoTaskCommands
{
    public class DeleteToDoTaskCommand : IRequest
    {
       public int Id { get; set; }

        public DeleteToDoTaskCommand(int id)
        {
            Id = id;
        }
    }
}
