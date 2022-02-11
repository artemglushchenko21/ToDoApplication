﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Models.Data;
using ToDoApi.Models.ViewModels;
using ToDoMvc.Services;

namespace ToDoMvc.ControllersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserService userService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            var result = await _userService.AddUser(model);

            if (result.Succeeded == false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

               return BadRequest(result.Errors);
            }
            //return result;

            //var user = new ApplicationUser
            //{
            //    FirstName = model.FirstName,
            //    LastName = model.LastName,
            //    Email = model.Email,
            //    UserName = $"{ model.FirstName }{ model.LastName }"
            //};
            //var result = await _userManager.CreateAsync(user, model.Password);

            //if (result.Succeeded)
            //{
            //    bool isPersistant = false;
            //    await _signInManager.SignInAsync(user, isPersistant);
            //}
            //else
            //{
            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError("", error.Description);
            //    }
            //}

            return Ok(model);
        }

    
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
