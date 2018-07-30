using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasierAPI.Models
{
    public class ResponseMessageModels
    {
        public int status { get; set; }
        public string message { get; set; }
        public Object data { get; set; }
    }
}