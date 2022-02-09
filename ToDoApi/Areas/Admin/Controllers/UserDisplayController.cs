using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoMvc.Areas.Admin.Models;
using ToDoMvc.Models.Helpers;

namespace ToDoMvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]

    public class UserDisplayController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IApiHelper _apiHelper;

        public UserDisplayController(UserManager<ApplicationUser> userManager, 
                        RoleManager<IdentityRole> roleManager,
                        IApiHelper apiHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _apiHelper = apiHelper;
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response =  await _apiHelper.ApiClient.GetAsync("User/");
            var appUsers = response.Content.ReadAsAsync<List<ApplicationUser>>().Result;

            UserViewModel model = new()
            {
                ApplicationUsers = appUsers,
                Roles = _roleManager.Roles
            };
            return View(model);
        }


        public async Task<IActionResult> EditUser(string id)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("User/" + id);
            var user = response.Content.ReadAsAsync<ApplicationUser>().Result;

            return View("Edit", user);
        }
    }
}
