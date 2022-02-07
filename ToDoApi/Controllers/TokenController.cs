using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models;
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
        private readonly UserManager<UserModel> _userManager;

        public TokenController(IJwtAuthenticationManager jwtAuthenticationManager, UserManager<UserModel> userManager)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _userManager = userManager;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginData loginData)
        {
            if (await IsValidUserNameAndPassword(loginData.Username, loginData.Password) == false)
            {
                return Unauthorized();
            }

            var token = _jwtAuthenticationManager.GenerateToken(loginData.Username, loginData.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        private async Task<bool> IsValidUserNameAndPassword(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}