using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using ToDoApi;

namespace ToDoMvc.Extensions.ClaimsPrincipalExtensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAdmin (this ClaimsPrincipal user)
        {
            string adminRoleName = Startup.StaticConfig.GetValue<string>("RoleNames:AdminRole");
            return user.IsInRole(adminRoleName);
        }
    }
}
