﻿using ExtensionsLibrary;
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
using System.Text.Json;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.ToDoTaskElements;
using ToDoApi.Models.ViewModels;
using ToDoMvc;
using ToDoMvc.Models;
using ToDoMvc.Models.Helpers;

namespace ToDoApi.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _context;
        private readonly IApiHelper _apiHelper;

        public AccountController(SignInManager<ApplicationUser> signInManager,
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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("User", model);

                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                var loginData = new LoginViewModel { Email = model.Email, Password = model.Password };

                response = await _apiHelper.ApiClient.PostAsJsonAsync("Token/authenticate", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var authenticatedUser = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    SetLoggedInUserInfo(authenticatedUser.Access_Token);

                    await CreateFirstDefaultTask();

                    return RedirectToAction("ShowTasks", "Home");
                }
            }
            return View(model);
        }

        private async Task CreateFirstDefaultTask()
        {
            var task = new ToDo
            {
                Description = "My first task",
                CategoryId = "home",
                DueDate = DateTime.Now,
                StatusId = "open"
            };

            await _apiHelper.ApiClient.PostAsJsonAsync("ToDo", task);
        }
    }
}