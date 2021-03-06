using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a username.")]
        [RegularExpression("(?i)^[a-z0-9 ]+$", ErrorMessage = "First name may not contain special characters.")]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a username.")]
        [RegularExpression("(?i)^[a-z0-9 ]+$", ErrorMessage = "Last name may not contain special characters.")]
        [StringLength(255)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter an email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "Password length should be 6 or more characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
