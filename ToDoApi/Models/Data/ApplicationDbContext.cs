using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models.ToDoTaskElements;

namespace ToDoApi.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }


        public DbSet<ToDoTask> ToDos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = "work", Name = "Work" },
                new Category { CategoryId = "home", Name = "Home" },
                new Category { CategoryId = "ex", Name = "Exercise" },
                new Category { CategoryId = "shop", Name = "Shopping" },
                new Category { CategoryId = "call", Name = "Contact" }
            );
            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "open", Name = "Open" },
                new Status { StatusId = "closed", Name = "Completed" }
            );
        }

        public static async Task CreateAdminUser(IServiceProvider serviceProvider, IConfiguration config)
        {
            UserManager<ApplicationUser> userManager =
          serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager
                = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminEmail = config.GetValue<string>("AdminCredentials:Email");
            string adminPassword = config.GetValue<string>("AdminCredentials:Password");
            string adminRole = config.GetValue<string>("AdminCredentials:Role");
            string memberRole = "member";


            if (await roleManager.FindByNameAsync(adminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }


            if (await roleManager.FindByNameAsync(memberRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(memberRole));
            }

            //ApplicationUser appUser = new()
            //{
            //    Email = adminEmail,
            //    UserName = adminEmail
            //};

            //var result = await userManager.CreateAsync(appUser, adminPassword);

            //if (result.Succeeded)
            //{
            //    await userManager.AddToRoleAsync(appUser, adminRole);
            //}
        }
    }

}
