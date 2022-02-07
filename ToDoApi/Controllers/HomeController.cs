using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.ViewModels;
using ToDoMvc;


namespace ToDoApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _logger = logger;
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

            IEnumerable<ToDo> tasks = null;

            GlobalVariables.WebApiClient.DefaultRequestHeaders.Add("UserId", User.Identity.GetUserId());

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("ToDo").Result;

            tasks = response.Content.ReadAsAsync<IEnumerable<ToDo>>().Result;

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
                    var result = await GlobalVariables.WebApiClient.PostAsJsonAsync("ToDo", task);
                }
                else
                {
                    var result = await GlobalVariables.WebApiClient.PutAsJsonAsync("ToDo/" + taskId.ToString(), task);
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
        public IActionResult CompleteTask([FromRoute] string id, ToDo selected)
        {
            string newStatusId = selected.StatusId;
            selected = _context.ToDos.Find(selected.Id);
            selected.StatusId = newStatusId;
            _context.ToDos.Update(selected);

            _context.SaveChanges();

            return RedirectToAction(nameof(ShowTasks), new { ID = id });
        }

        [HttpGet]
        public IActionResult EditTask([FromRoute] string id, ToDo selected)
        {
            string taskId = selected.Id.ToString();

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("ToDo/" + taskId).Result;

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

            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("ToDo/" + taskId).Result;

            return RedirectToAction(nameof(ShowTasks));
        }

        [HttpGet]
        public IActionResult UserProfile()
        {
            UserModel user = new();
            user.UserName = User.Identity.Name;

            return View("UserProfile", user);
        }
    }
}