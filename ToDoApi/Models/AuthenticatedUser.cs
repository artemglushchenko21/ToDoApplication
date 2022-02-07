
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoMvc.Models
{
    public class AuthenticatedUser
    {
        public string Access_Token { get; set; }

        public string UserName { get; set; }
    }
}