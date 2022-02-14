using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoMvc.Queries.ToDoTaskQueries
{
    public class GetToDoTasksQuery : IRequest<IEnumerable<ToDoTask>>
    {
        public string FilterId { get; set; }

        public GetToDoTasksQuery(string filterId)
        {
            FilterId = filterId;
        }

    }
}
