using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.ViewModels;

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

        public UserController(ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

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