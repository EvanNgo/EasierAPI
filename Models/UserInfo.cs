using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasierAPI.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Birthyear { get; set; }
        public string Thumbnail { get; set; }

        public UserInfo(User user) {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Birthyear = user.Birthyear;
            Thumbnail = user.Thumbnail;
        }
    }
}