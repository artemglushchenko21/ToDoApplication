using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.ViewModels;
using ToDoMvc.Models;
using ToDoMvc.Services;
using ToDoMvc.Services.Authentication;

namespace ToDoMvc.ControllersApi
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public TokenController(IJwtAuthenticationManager jwtAuthenticationManager, 
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IUserService userService)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _userManager = userManager;
            _context = context;
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginViewModel loginData)
        {
            if (await IsValidUserNameAndPassword(loginData.Email, loginData.Password) == false)
            {
                return Unauthorized();
            }

            ApplicationUser user = await _userManager.FindByEmailAsync(loginData.Email);

            user.RoleNames = await _userService.GetUserRoles(user);

            var token = _jwtAuthenticationManager.GenerateToken(user.FirstName, loginData.Email, user.Id, user.RoleNames);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        private async Task<bool> IsValidUserNameAndPassword(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}