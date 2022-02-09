using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoMvc.Services.Authentication
{
    public interface IJwtAuthenticationManager
    {
        dynamic GenerateToken(string userName, string email, string userId);
    }
}