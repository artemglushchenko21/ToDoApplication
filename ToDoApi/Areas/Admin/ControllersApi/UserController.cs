using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;      
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;

        public UserController(ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserService userService)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
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
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    string errorMessage = "";
                    foreach (IdentityError error in result.Errors)
                    {
                        errorMessage += error.Description + " | ";
                    }
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("AddAdminRoleToUser/{id}")]
        public async Task AddAdminRoleToUser(string id)
        {
            IdentityRole adminRole = await _roleManager.FindByNameAsync("admin");

            if (adminRole != null)
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.AddToRoleAsync(user, adminRole.Name);
            }
        }

        [HttpPost("RemoveAdminRoleFromUser/{id}")]
        public async Task RemoveAdminRoleFromUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "admin");
        }
    }
}