//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ToDoWebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//    }
//}


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
using ToDoWebApi.Data;
using ToDoWebApi.Models;
using ToDoWebApi.Models.ViewModels;

namespace ToDoWebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public UserController(ApplicationDbContext context, ILogger<UserController> logger, SignInManager<UserModel> signInManager, UserManager<UserModel> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<UserModel> GetAllUsers()
        {
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userList = _context.Users.ToList();

            return userList;
        }

        [HttpGet]
        public IEnumerable<UserModel> GetUserById(string id)
        {
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userList = _context.Users.ToList();

            return userList;
        }

        [HttpPost]
        public async Task AddAUser(RegisterViewModel model)
        {
            var list = _context.Users.ToList();

 
                var user = new UserModel
                {
                    UserName = model.Username
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
            
        }

        //[HttpGet]
        //public List<UserModel> GetAllUsers()
        //{
        //    // string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var userList = _context.Users.ToList();

        //    return userList;
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //[Route("Admin/GetAllUsers")]
        //public List<ApplicationUserModel> GetAllUsers()
        //{
        //    List<ApplicationUserModel> output = new List<ApplicationUserModel>();

        //    var users = _context.Users.ToList();
        //    var userRoles = from ur in _context.UserRoles
        //                    join r in _context.Roles on ur.RoleId equals r.Id
        //                    select new { ur.UserId, ur.RoleId, r.Name };

        //    foreach (var user in users)
        //    {
        //        ApplicationUserModel u = new ApplicationUserModel
        //        {
        //            Id = user.Id,
        //            Email = user.Email
        //        };

        //        u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, val => val.Name);

        //        output.Add(u);
        //    }


        //    return output;
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //[Route("Admin/GetAllRoles")]
        //public Dictionary<string, string> GetAllRoles()
        //{
        //    var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);

        //    return roles;

        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[Route("Admin/AddRole")]
        //public async Task AddARole(UserRolePairModel pairing)
        //{
        //    string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var user = await _userManager.FindByIdAsync(pairing.UserId);

        //    _logger.LogInformation("Admin {Admin} added user {User} to role {Role}",
        //        loggedInUserId, user.Id, pairing.RoleName);

        //    await _userManager.AddToRoleAsync(user, pairing.RoleName);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[Route("Admin/RemoveRole")]
        //public async Task RemoveARole(UserRolePairModel pairing)
        //{
        //    string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var user = await _userManager.FindByIdAsync(pairing.UserId);

        //    _logger.LogInformation("Admin {Admin} remove user {User} from role {Role}",
        //        loggedInUserId, user.Id, pairing.RoleName);

        //    await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
        //}

    }
}

