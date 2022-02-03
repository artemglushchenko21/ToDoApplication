using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoWebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IList<string> RoleNames { get; set; }
    }
}
