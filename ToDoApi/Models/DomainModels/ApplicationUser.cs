using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Models.DomainModels
{
    public class ApplicationUser
    {
        public int ID { get; set; } 

        [Required(ErrorMessage = "Please enter a username.")]
        [RegularExpression("(?i)^[a-z0-9 ]+$",
            ErrorMessage = "Username may not contain special characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [Remote("CheckEmail", "Validation")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [Compare("ConfirmPassword")]
      
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "Password length should be 6 or more characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [Display(Name = "Confirm Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}
