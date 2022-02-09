using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApi.Models
{
    public class ApplicationUser: IdentityUser
    {
        [NotMapped]
        public IList<string> RoleNames { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}