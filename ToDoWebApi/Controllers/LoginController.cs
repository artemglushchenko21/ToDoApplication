using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Description;
using ToDoWebApi.Data;
using ToDoWebApi.Models;
using ToDoWebApi.Models.ViewModels;

namespace ToDoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public LoginController(ApplicationDbContext context,
            SignInManager<UserModel> signInManager,
            UserManager<UserModel> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpGet]
        public IEnumerable<UserModel> LogIn()
        {
            var userList = _context.Users.ToList();
            return userList;
        }

        [HttpPost]
        [ResponseType(typeof(bool))]
        public async Task<bool> LogIn(LoginViewModel model)
        {
            var userList = _context.Users.ToList();

            var result = await _signInManager.PasswordSignInAsync(
                model.Username, model.Password, isPersistent: false,
                lockoutOnFailure: false);

            return result.Succeeded;

            //if (result.Succeeded)
            //{
            //    if (!string.IsNullOrEmpty(model.ReturnUrl) &&
            //        Url.IsLocalUrl(model.ReturnUrl))
            //    {
            //        return Redirect(model.ReturnUrl);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }
            //}
        }
    }
}

