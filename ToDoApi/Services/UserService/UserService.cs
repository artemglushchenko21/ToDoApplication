using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public UserService(ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task AddAdminRoleToUser(string id)
        {
            IdentityRole adminRole = await _roleManager.FindByNameAsync(_config.GetValue<string>("RoleNames:AdminRole"));

            if (adminRole != null)
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.AddToRoleAsync(user, adminRole.Name);
            }
        }

        public async Task<IdentityResult> AddUser(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await AssignDefaultRoleToUser(user);
            }

            return result;
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return IdentityResult.Failed();

            IdentityResult result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                string errorMessage = "";
                foreach (IdentityError error in result.Errors)
                {
                    errorMessage += error.Description + " | ";
                }

                return result;
            }

            await _context.SaveChangesAsync();

            return result;
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

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await GetUserRoles(user);

            return user;
        }

        public async Task RemoveAdminRoleFromUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            string adminRoleName = _config.GetValue<string>("RoleNames:AdminRole");
            await _userManager.RemoveFromRoleAsync(user, adminRoleName);
        }

        private async Task AssignDefaultRoleToUser(ApplicationUser user)
        {
            string defaultRole = _config.GetValue<string>("RoleNames:DefaultRole");

            if (await _roleManager.FindByNameAsync(defaultRole) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(defaultRole));
            }

            await _userManager.AddToRoleAsync(user, defaultRole);
        }
    }
}
