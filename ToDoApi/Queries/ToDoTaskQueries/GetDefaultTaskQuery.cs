using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoMvc.Queries.ToDoTaskQueries
{
    public class GetDefaultTaskQuery : IRequest<ToDoTask>
    {
        public string UserId { get; set; }

        public GetDefaultTaskQuery(string userId)
        {
            UserId = userId;
        }

    }

}
