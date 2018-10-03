using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasierAPI.Models
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string HashPass { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        public string ConfirmHashPass { get; set; }
    }
}