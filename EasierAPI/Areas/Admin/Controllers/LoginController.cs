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
            if (Request.Cookies["email"] != null)
            {
                HttpCookie aCookie = Request.Cookies["email"];
                string email = Server.HtmlEncode(aCookie.Value);
                User admin = db.Users.Where(u => u.Email == email).FirstOrDefault();
                if (admin != null)
                {
                    UserUtils userUtils = new UserUtils(admin);
                    Session.Add(Const.USER_SESSION, userUtils);
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            return View();
        }

        public ActionResult Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                string hashpass = CommonUtils.getMd5Hash(user.HashPass);
                User admin = db.Users.Where(u => u.Email == user.Email && u.HashPass == hashpass).FirstOrDefault();
                if (admin != null)
                {
                    UserUtils userUtils = new UserUtils(admin);
                    Session.Add(Const.USER_SESSION, userUtils);
                    HttpCookie ck = new HttpCookie("email");
                    ck.Value = admin.Email;
                    ck.Expires = DateTime.Now.AddDays(15);
                    Response.Cookies.Add(ck);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("","Login Failed");
                }
            }
            return View("Index");
        }
    }
}