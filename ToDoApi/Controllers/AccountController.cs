using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDoApi.Models.Data;
using ToDoApi.Models.DomainModels;
using ToDoApi.Models.ViewModels;
using ToDoMvc;

namespace ToDoApi.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private ApplicationDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = applicationDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LogIn(string returnURL = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnURL };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await GlobalVariables.WebApiClient.PostAsJsonAsync("Login", model);

                var canLogIn = response.Content.ReadAsAsync<bool>().Result;

                if (canLogIn == false)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }          
            }

            var result = await _signInManager.PasswordSignInAsync(
        model.Username, model.Password, isPersistent: false,
        lockoutOnFailure: false);

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await GlobalVariables.WebApiClient.PostAsJsonAsync("User", model);

                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        } 
    }
}

