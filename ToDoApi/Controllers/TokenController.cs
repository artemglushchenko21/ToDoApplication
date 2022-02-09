using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.ViewModels;
using ToDoMvc.Models;
using ToDoMvc.Services.Authentication;

namespace ToDoMvc.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenController(IJwtAuthenticationManager jwtAuthenticationManager, UserManager<ApplicationUser> userManager)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _userManager = userManager;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginViewModel loginData)
        {
            if (await IsValidUserNameAndPassword(loginData.Email, loginData.Password) == false)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByEmailAsync(loginData.Email);

            var token = _jwtAuthenticationManager.GenerateToken(user.FirstName, loginData.Email, user.Id);
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