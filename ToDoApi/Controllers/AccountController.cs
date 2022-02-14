using ExtensionsLibrary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.ViewModels;
using ToDoMvc.Models;
using ToDoMvc.Models.Helpers;
using ToDoMvc.Queries.ToDoTaskQueries;
using ToDoMvc.Services.ToDoTaskService;

namespace ToDoApi.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IApiHelper _apiHelper;

        public AccountController(SignInManager<ApplicationUser> signInManager, IApiHelper apiHelper)
        {
            _signInManager = signInManager;
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
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginData = new LoginViewModel { Email = model.Email, Password = model.Password };

                HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("Token/authenticate", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var authenticatedUser = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    SetLoggedInUserInfo(authenticatedUser.Access_Token);

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("ShowTasks", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        private void SetLoggedInUserInfo(string token)
        {
            _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
            _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
            _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiHelper.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            HttpContext.Session.SetString("JWToken", token);
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
        public async Task<IActionResult> Register(RegisterViewModel model, [FromServices] IToDoTaskService toDoTaskService, [FromServices] IMediator _mediator)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("User", model);

                if (response.IsSuccessStatusCode == false)
                {
                    var errors = response.Content.ReadAsAsync<IEnumerable<IdentityError>>().Result;

                    foreach (var error in errors)
                    {
                        ModelState.AddModelError("", error.Description.Replace("Username", "Email"));
                    }

                    return View(model);
                }

                var loginData = new LoginViewModel { Email = model.Email, Password = model.Password };

                response = await _apiHelper.ApiClient.PostAsJsonAsync("Token/authenticate", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var authenticatedUser = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    SetLoggedInUserInfo(authenticatedUser.Access_Token);

                    var defaultTask = await _mediator.Send(new GetDefaultTaskQuery(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    await _apiHelper.ApiClient.PostAsJsonAsync("ToDoTask", defaultTask);

                    return RedirectToAction("ShowTasks", "Home");
                }
            }
            return View(model);
        }
    }
}