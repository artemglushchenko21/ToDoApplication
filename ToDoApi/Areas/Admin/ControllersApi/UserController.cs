using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.ViewModels;
using ToDoMvc.Services;

namespace ToDoMvc.Areas.Admin.ControllersApi
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/admin/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }

        [HttpGet("{id}")]
        public async Task<ApplicationUser> GetUserById(string id)
        {
           return await _userService.GetUserById(id);
        }

        [HttpPost]
        public async Task<IdentityResult> AddUser(RegisterViewModel model)
        {
            var result = await _userService.AddUser(model);

            if (result.Succeeded == false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUser(id);

            if (result.Succeeded == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("AddAdminRoleToUser/{id}")]
        public async Task AddAdminRoleToUser(string id)
        {
           await _userService.AddAdminRoleToUser(id);
        }

        [HttpPost("RemoveAdminRoleFromUser/{id}")]
        public async Task RemoveAdminRoleFromUser(string id)
        {
            await _userService.RemoveAdminRoleFromUser(id);
        }
    }
}