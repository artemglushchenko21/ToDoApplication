using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.ViewModels;

namespace ToDoMvc.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task AddAdminRoleToUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> AddAUser(RegisterViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            List<ApplicationUser> appUsers = new();

            foreach (ApplicationUser user in _userManager.Users)
            {
                await GetUserRoles(user);
                appUsers.Add(user);
            }

            var userList = _context.Users.ToList();
            return userList;
        }

        private async Task GetUserRoles(ApplicationUser user)
        {
            user.RoleNames = await _userManager.GetRolesAsync(user);
        }

        public Task<ApplicationUser> GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAdminRoleFromUser(string id)
        {
            throw new NotImplementedException();
        }
    }
}
