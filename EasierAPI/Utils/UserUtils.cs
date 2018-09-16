using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasierAPI.Utils
{
    [Serializable]
    public class UserUtils
    {
        public int UserId { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public bool IsAdmin { set; get; }
        public string Thumbnail { set; get; }
    }
}