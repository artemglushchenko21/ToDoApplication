using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApi.Models
{
    public class UserModel: IdentityUser
    {
        [NotMapped]
        public IList<string> RoleNames { get; set; }
    }
}