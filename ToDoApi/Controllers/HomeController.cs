using Microsoft.AspNet.Identity;
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


        public IActionResult Index(string id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();
            ViewBag.DueFilters = Filters.DueFilterValues;

            IEnumerable<ToDo> tasks;

           GlobalVariables.WebApiClient.DefaultRequestHeaders.Add("UserId", User.Identity.GetUserId());

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("ToDo").Result;

            tasks = response.Content.ReadAsAsync<IEnumerable<ToDo>>().Result;

            return View(tasks);
        }

        public IActionResult AddTask()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(ToDo task)
        {
            if (ModelState.IsValid)
            {
                await GlobalVariables.WebApiClient.PostAsJsonAsync("ToDo", task);
                return RedirectToAction("Index");

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
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] string id, ToDo selected)
        {
            if (selected.StatusId == null)
            {
                _context.ToDos.Remove(selected);
            }
            else
            {
                string newStatusId = selected.StatusId;
                selected = _context.ToDos.Find(selected.Id);
                selected.StatusId = newStatusId;
                _context.ToDos.Update(selected);
            }
            _context.SaveChanges();

            return RedirectToAction("Index", new { ID = id });
        }
    }
}
