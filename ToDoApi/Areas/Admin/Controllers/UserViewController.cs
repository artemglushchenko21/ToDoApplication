using ExtensionsLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoMvc.Areas.Admin.Models;
using ToDoMvc.Models;
using ToDoMvc.Models.Helpers;

namespace ToDoMvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]

    public class UserViewController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IApiHelper _apiHelper;

        public UserViewController(RoleManager<IdentityRole> roleManager,
                        IApiHelper apiHelper)
        {
            _roleManager = roleManager;
            _apiHelper = apiHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response =  await _apiHelper.ApiClient.GetAsync("admin/User/");
           
            var appUsers = await response.Content.ReadAsAsync<List<ApplicationUser>>(); 

            UserViewModel model = new()
            {
                ApplicationUsers = appUsers,
                Roles = _roleManager.Roles
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("admin/User/" + id);

            var user = await response.Content.ReadAsAsync<ApplicationUser>();

            return View("Edit", user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync("admin/User/" + id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddAdminRoleToUser(string id)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync($"admin/User/AddAdminRoleToUser/{id}", null);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdminRoleFromUser(string id)
        {
            HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync($"admin/User/RemoveAdminRoleFromUser/{id}", null);

            return RedirectToAction("Index");
        }
    }
}
