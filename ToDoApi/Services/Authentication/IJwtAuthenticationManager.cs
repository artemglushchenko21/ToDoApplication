using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoMvc.Models;

namespace ToDoMvc.Services.Authentication
{
    public interface IJwtAuthenticationManager
    {
        AuthenticatedUser GenerateToken(string userName, string email, string userId, IList<string> roles);
    }
}