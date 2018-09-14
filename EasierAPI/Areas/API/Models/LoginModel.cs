using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EasierAPI.Areas.API.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}