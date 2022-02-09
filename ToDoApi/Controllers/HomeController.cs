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
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.ToDoTaskElements;
using ToDoApi.Models.ViewModels;
using ToDoMvc;
using ToDoMvc.Models.Helpers;

namespace ToDoApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IApiHelper _apiHelper;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, 
            ApplicationDbContext applicationDbContext, 
            IApiHelper apiHelper,
            UserManager<ApplicationUser> userManager)
        {
            _context = applicationDbContext;
            _apiHelper = apiHelper;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult ShowTasks(string id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();
            ViewBag.DueFilters = Filters.DueFilterValues;
         //  _apiHelper.ApiClient.DefaultRequestHeaders.Add("UserId", User.Identity.GetUserId());

            HttpResponseMessage response = _apiHelper.ApiClient.GetAsync("ToDo?filterId=" + id).Result;

            IEnumerable<ToDo> tasks = response.Content.ReadAsAsync<IEnumerable<ToDo>>().Result;
            return View(tasks);
        }

        [Authorize]
        public IActionResult AddTask()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveTask(ToDo task)
        {
            if (ModelState.IsValid)
            {
                int taskId = task.Id;

                if (taskId == 0)
                {
                    var result = await _apiHelper.ApiClient.PostAsJsonAsync("ToDo", task);
                }
                else
                {
                    var result = await _apiHelper.ApiClient.PutAsJsonAsync("ToDo/" + taskId.ToString(), task);
                }               
                return RedirectToAction(nameof(ShowTasks));
            }
            else
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Statuses = _context.Statuses.ToList();
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
        public IActionResult CompleteTask(string statusName, [FromRoute] string id, ToDo selected)
        {
            //string newStatusId = selected.StatusId;
            selected = _context.ToDos.Find(selected.Id);
            selected.StatusId = statusName;
            _context.ToDos.Update(selected);

            _context.SaveChanges();

            return RedirectToAction(nameof(ShowTasks), new { ID = id });
        }

        [HttpGet]
        public IActionResult EditTask([FromRoute] string id, ToDo selected)
        {
            string taskId = selected.Id.ToString();

            HttpResponseMessage response = _apiHelper.ApiClient.GetAsync("ToDo/" + taskId).Result;

            //    return RedirectToAction(nameof(AddTask), new { ID = id });
            var result = response.Content.ReadAsAsync<ToDo>().Result;

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();

            ModelState.Clear();

            return View(nameof(AddTask),result);
        }

        [HttpPost]
        public IActionResult DeleteTask([FromRoute] string id, ToDo selected)
        {
            string taskId = selected.Id.ToString();

            HttpResponseMessage response = _apiHelper.ApiClient.DeleteAsync("ToDo/" + taskId).Result;

            return RedirectToAction(nameof(ShowTasks));
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            return View("UserProfile", user);
        }
    }
}