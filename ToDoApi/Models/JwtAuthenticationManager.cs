using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using ToDoApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ToDoMvc.Models
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly IDictionary<string, string> users = new Dictionary<string, string>
         { {"test1", "password1"}, {"test2", "password2" } };

        private readonly string _key;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public JwtAuthenticationManager(string key)
        {
            _key = key;
        }

        public async Task<string> Authenticate(string userName, string password)
        {

            //if (await IsValidUserNameAndPassword(userName, password))
            //{
            //    return null;
            //}

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDesriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDesriptor);
            return tokenHandler.WriteToken(token);
        }

       public class Helper
        {
            private readonly UserManager<UserModel> _userManager;
            private readonly SignInManager<UserModel> _signInManager;

            public Helper(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager )
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }
            private async Task<bool> IsValidUserNameAndPassword([FromServices] SignInManager<UserModel> _signInManager, string username, string password)
            {
                var user = await _userManager.FindByEmailAsync(username);
                return await _userManager.CheckPasswordAsync(user, password);
            }
        }
    }
}
