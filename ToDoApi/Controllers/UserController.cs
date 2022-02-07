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
//using ToDoWebApi.Controllers;
//using ToDoWebApi.Data;
//using ToDoWebApi.Models;
//using ToDoWebApi.Models.ViewModels;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public UserController(ApplicationDbContext context,
            SignInManager<UserModel> signInManager,
            UserManager<UserModel> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<UserModel> GetAllUsers()
        {
            var userList = _context.Users.ToList();
            return userList;
        }

        [HttpGet]
        [Route("/GetUserById")]
        public async Task<UserModel> GetUserById(string id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _userManager.FindByIdAsync(userId);

            //var user = await _userManager.FindByIdAsync(id);
            //return user;
        }

        [HttpPost]
        public async Task<IdentityResult> AddAUser(RegisterViewModel model)
        {
            var user = new UserModel
            {
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                bool isPersistant = true;
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