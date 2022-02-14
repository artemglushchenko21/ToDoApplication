using ExtensionsLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.ToDoTaskElements;
using ToDoMvc.Models.DTOs;
using ToDoMvc.Models.Helpers;
using ToDoMvc.Services.ToDoTaskService;

namespace ToDoApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApiHelper _apiHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToDoTaskFilterService _toDoTaskFilterService;

        public HomeController(ILogger<HomeController> logger,
            IApiHelper apiHelper,
            UserManager<ApplicationUser> userManager,
            IToDoTaskFilterService toDoTaskFilterService)
        {
            _apiHelper = apiHelper;
            _logger = logger;
            _userManager = userManager;
            _toDoTaskFilterService = toDoTaskFilterService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ShowTasks(string id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;

            AddFiltersToViewBag();
           
            HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"ToDoTask?filterId={id}");
            var tasks = await response.Content.ReadAsAsync<IEnumerable<ToDoTask>>();

            return View(tasks);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddTask()
        {
            AddFiltersToViewBag();

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveTask(ToDoTask task)
        {
            if (ModelState.IsValid)
            {
                if (task.Id == 0)
                {
                    var result = await _apiHelper.ApiClient.PostAsJsonAsync("ToDoTask", task);
                }
                else
                {
                    var result = await _apiHelper.ApiClient.PutAsJsonAsync($"ToDoTask/{task.Id}", task);
                }
                return RedirectToAction(nameof(ShowTasks));
            }
            else
            {
                AddFiltersToViewBag();

                return View("AddTask", task);
            }
        }

        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction(nameof(ShowTasks), new { ID = id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CompleteTask(string statusName, [FromRoute] string id, ToDoTask selected)
        {
            var taskStatus = new TaskStatusDTO
            {
                TaskId = selected.Id,
                TaskStatusId = statusName
            };

            var result = await _apiHelper.ApiClient.PostAsJsonAsync($"ToDoTask/ModifyToDoTaskStatus/", taskStatus);

            return RedirectToAction(nameof(ShowTasks), new { ID = id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditTask([FromRoute] string id, ToDoTask selected)
        {
            string taskId = selected.Id.ToString();

            HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"ToDoTask/{taskId}");

            var result = await response.Content.ReadAsAsync<ToDoTask>();

            AddFiltersToViewBag();

           ModelState.Clear();

            return View(nameof(AddTask), result);
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteTask([FromRoute] string id, ToDoTask selected)
        {
            string taskId = selected.Id.ToString();

            HttpResponseMessage response = _apiHelper.ApiClient.DeleteAsync($"ToDoTask/{taskId}").Result;

            return RedirectToAction(nameof(ShowTasks));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            return View("UserProfile", user);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        private void AddFiltersToViewBag()
        {
            ViewBag.Categories = _toDoTaskFilterService.Categories;
            ViewBag.Statuses = _toDoTaskFilterService.Statuses;
            ViewBag.DueFilters = Filters.DueFilterValues;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}