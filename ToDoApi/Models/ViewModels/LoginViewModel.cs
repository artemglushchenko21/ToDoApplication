using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter an email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(255)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
