using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
using ToDoMvc.Models;
using ToDoMvc.Models.Helpers;

namespace ToDoApi.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<UserModel> _signInManager;
        private ApplicationDbContext _context;
        private readonly IApiHelper _apiHelper;

        public AccountController(SignInManager<UserModel> signInManager,
                                 ApplicationDbContext applicationDbContext,
                                 IApiHelper apiHelper)
        {
            _signInManager = signInManager;
            _context = applicationDbContext;
            _apiHelper = apiHelper;
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
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginData = new LoginData { Username = model.Username, Password = model.Password };

                using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("Token/authenticate", loginData))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var token = await response.Content.ReadAsAsync<AuthenticatedUser>();
                        GetLoggedInUserInfo(token.Access_Token);

                        HttpContext.Session.SetString("JWToken", token.Access_Token);

                        if (!string.IsNullOrEmpty(model.ReturnUrl) &&
                            Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("ShowTasks", "Home");
                        }
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }

            ModelState.AddModelError("", "Invalid username/password.");
            return View(model);
        }

        public void GetLoggedInUserInfo(string token)
        {
            _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
            _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
            _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiHelper.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //return _userData.GetUserById(userId).First();

            //using (HttpResponseMessage response = await GlobalVariables.WebApiClient.GetAsync("/api/User"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
            //        _loggedInUser.CreatedDate = result.CreatedDate;
            //        _loggedInUser.EmailAddress = result.EmailAddress;
            //        _loggedInUser.FirstName = result.FirstName;
            //        _loggedInUser.Id = result.Id;
            //        _loggedInUser.LastName = result.LastName;
            //        _loggedInUser.Token = token;
            //    }
            //    else
            //    {
            //        throw new Exception(response.ReasonPhrase);
            //    }
            //}
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            HttpContext.Session.Clear();

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
                HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("User", model);

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