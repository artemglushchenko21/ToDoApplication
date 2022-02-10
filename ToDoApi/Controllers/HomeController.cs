﻿using ExtensionsLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.ToDoTaskElements;
using ToDoApi.Models.ViewModels;
using ToDoMvc;
using ToDoMvc.Models.DTOs;
using ToDoMvc.Models.Helpers;
using ToDoMvc.Services.ToDoTaskService;

namespace ToDoApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IApiHelper _apiHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToDoTaskFilterService _toDoTaskFilterService;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext applicationDbContext,
            IApiHelper apiHelper,
            UserManager<ApplicationUser> userManager,
            IToDoTaskFilterService toDoTaskFilterService)
        {
            _context = applicationDbContext;
            _apiHelper = apiHelper;
            _logger = logger;
            _userManager = userManager;
            _toDoTaskFilterService = toDoTaskFilterService;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ShowTasks(string id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;
            ViewBag.Categories = _toDoTaskFilterService.Categories;
            ViewBag.Statuses = _toDoTaskFilterService.Statuses;
            ViewBag.DueFilters = Filters.DueFilterValues;

            HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"ToDoTask?filterId={id}");

            var tasks = await response.Content.ReadAsAsync<IEnumerable<ToDoTask>>();

            return View(tasks);
        }

        [Authorize]
        public IActionResult AddTask()
        {
            ViewBag.Categories = _toDoTaskFilterService.Categories;
            ViewBag.Statuses = _toDoTaskFilterService.Statuses;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveTask(ToDoTask task)
        {
            if (ModelState.IsValid)
            {
                int taskId = task.Id;

                if (taskId == 0)
                {
                    var result = await _apiHelper.ApiClient.PostAsJsonAsync("ToDoTask", task);
                }
                else
                {
                    var result = await _apiHelper.ApiClient.PutAsJsonAsync($"ToDoTask/{taskId}", task);
                }
                return RedirectToAction(nameof(ShowTasks));
            }
            else
            {
                ViewBag.Categories = _toDoTaskFilterService.Categories;
                ViewBag.Statuses = _toDoTaskFilterService.Statuses;

                return View(task);
            }
        }

        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction(nameof(ShowTasks), new { ID = id });
        }

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

        [HttpGet]
        public async Task<IActionResult> EditTask([FromRoute] string id, ToDoTask selected)
        {
            string taskId = selected.Id.ToString();

            HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"ToDoTask/{taskId}");

            //    return RedirectToAction(nameof(AddTask), new { ID = id });
            var result = await response.Content.ReadAsAsync<ToDoTask>();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();

            ModelState.Clear();

            return View(nameof(AddTask), result);
        }

        [HttpPost]
        public IActionResult DeleteTask([FromRoute] string id, ToDoTask selected)
        {
            string taskId = selected.Id.ToString();

            HttpResponseMessage response = _apiHelper.ApiClient.DeleteAsync($"ToDoTask/{taskId}").Result;

            return RedirectToAction(nameof(ShowTasks));
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            return View("UserProfile", user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}