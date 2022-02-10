using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoMvc.Models.DTOs
{
    public class TaskStatusDTO
    {
        public int TaskId { get; set; }
        public string TaskStatusId { get; set; }
    }
}
