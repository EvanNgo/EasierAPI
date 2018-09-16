using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EasierAPI.Models;
using EasierAPI.Utils;

namespace EasierAPI.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private easier_database db = new easier_database();

        public ActionResult Index()
        {
            UserUtils userUtils = new UserUtils();
            userUtils = (UserUtils)Session[Const.USER_SESSION];
            if (userUtils != null){
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                string hashpass = getMd5Hash(loginModel.Password);
                User admin = db.Users.Where(u => u.Email == loginModel.Email && u.HashPass == hashpass).FirstOrDefault();
                if (admin != null)
                {
                    UserUtils userUtils = new UserUtils();
                    userUtils.UserId = admin.Id;
                    userUtils.Email = admin.Email;
                    userUtils.UserName = admin.UserName;
                    userUtils.Thumbnail = admin.Thumbnail;
                    userUtils.IsAdmin = true;
                    Session.Add(Const.USER_SESSION, userUtils);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("","Login Failed");
                }
            }
            return View("Index");
        }

        static string getMd5Hash(string input)
        { 
            MD5 md5Hasher = MD5.Create(); 
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder(); 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}