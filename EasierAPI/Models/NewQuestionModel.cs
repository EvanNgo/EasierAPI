using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasierAPI.Models
{
    public class NewQuestionModel
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        public string Title { get; set; }

        [Required(ErrorMessage = "Detail is required")]
        public string Detail { get; set; }
    }
}