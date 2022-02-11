﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.ViewModels;

namespace ToDoMvc.Services
{
    public interface IUserService
    {
        Task AddAdminRoleToUser(string id);
        Task<IdentityResult> AddUser(RegisterViewModel model);
        Task<IActionResult> DeleteUser(string id);
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(string id);
        Task RemoveAdminRoleFromUser(string id);
    }
}