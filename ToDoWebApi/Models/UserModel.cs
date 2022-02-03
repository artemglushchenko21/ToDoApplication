using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoWebApi.Models
{
    public class UserModel : IdentityUser
    {
        [NotMapped]
        public IList<string> RoleNames { get; set; }


    }
}
