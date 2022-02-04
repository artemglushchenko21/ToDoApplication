
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoMvc.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoMvc.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public NameController(IJwtAuthenticationManager jwtAuthenticationManager, SignInManager<UserModel> signInManager, UserManager<UserModel> userManager)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: api/<NameController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<NameController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

      //  [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(string userName, string password, string grant_type)
        {
            string user = HttpContext.Request.Headers["username"].ToString();
            string pass = HttpContext.Request.Headers["password"].ToString();

            if(await IsValidUserNameAndPassword(user, pass) == false)
            {
                return Unauthorized();
            }


            var token = _jwtAuthenticationManager.Authenticate(user, pass);
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
