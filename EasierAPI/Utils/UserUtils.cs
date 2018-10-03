using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasierAPI.Models;

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

        public UserUtils() { }

        public UserUtils(User user)
        {
            this.UserId = user.Id;
            this.Email = user.Email;
            this.UserName = user.UserName;
            this.Thumbnail = user.Thumbnail;
            this.IsAdmin = true;
        }
    }
}