using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.ViewModels;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]   
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

        [Authorize]
        [HttpGet("{id}")]   
        public async Task<ApplicationUser> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await GetUserRoles(user);

            return user;
        }

        [HttpPost]
        public async Task<IdentityResult> AddAUser(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = $"{ model.FirstName }{ model.LastName }" 
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                bool isPersistant = false;
                await _signInManager.SignInAsync(user, isPersistant);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return result;
        }
    }
}