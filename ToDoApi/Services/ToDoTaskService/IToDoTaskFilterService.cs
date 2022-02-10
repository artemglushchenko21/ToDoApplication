using System.Collections.Generic;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoMvc.Services.ToDoTaskService
{
    public interface IToDoTaskFilterService
    {
        List<Category> Categories { get; }
        List<Status> Statuses { get; }
    }
}