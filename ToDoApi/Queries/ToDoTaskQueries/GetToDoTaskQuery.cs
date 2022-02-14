using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoMvc.Queries.ToDoTaskQueries
{
    public class GetToDoTaskQuery : IRequest<ToDoTask>
    {
        public int Id { get; set; }

        public GetToDoTaskQuery(int id)
        {
            Id = id;
        }

    }
}
