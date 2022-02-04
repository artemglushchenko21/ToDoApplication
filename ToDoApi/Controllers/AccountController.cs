using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.DomainModels;
using ToDoApi.Models.ViewModels;
using ToDoMvc;

namespace ToDoApi.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<UserModel> _userManager;
        private SignInManager<UserModel> _signInManager;
        private ApplicationDbContext _context;

        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, ApplicationDbContext applicationDbContext)
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
                string username = model.Username;
                string password = model.Password;

                var data = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
                });


                GlobalVariables.WebApiClient.DefaultRequestHeaders.Add("username", username);
                GlobalVariables.WebApiClient.DefaultRequestHeaders.Add("password", password);

                using (HttpResponseMessage response = await GlobalVariables.WebApiClient.PostAsync("Name/authenticate", data))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var token = await response.Content.ReadAsAsync<string>();
                        //return result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
                //var result = await _signInManager.PasswordSignInAsync(
                //    model.Username, model.Password, isPersistent: true,
                //    lockoutOnFailure: false);

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

            ModelState.AddModelError("", "Invalid username/password.");
            return View(model);



        }

        //public async Task GetLoggedInUserInfo(string token)
        //{
        //    GlobalVariables.WebApiClient.DefaultRequestHeaders.Clear();
        //    GlobalVariables.WebApiClient.DefaultRequestHeaders.Accept.Clear();
        //    GlobalVariables.WebApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //    GlobalVariables.WebApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

        //    //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    //return _userData.GetUserById(userId).First();

        //    //using (HttpResponseMessage response = await GlobalVariables.WebApiClient.GetAsync("/api/User"))
        //    //{
        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
        //    //        _loggedInUser.CreatedDate = result.CreatedDate;
        //    //        _loggedInUser.EmailAddress = result.EmailAddress;
        //    //        _loggedInUser.FirstName = result.FirstName;
        //    //        _loggedInUser.Id = result.Id;
        //    //        _loggedInUser.LastName = result.LastName;
        //    //        _loggedInUser.Token = token;
        //    //    }
        //    //    else
        //    //    {
        //    //        throw new Exception(response.ReasonPhrase);
        //    //    }
        //    //}
        //}

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
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

