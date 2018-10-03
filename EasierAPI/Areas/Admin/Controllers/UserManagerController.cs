using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasierAPI.Models;
using EasierAPI.Utils;

namespace EasierAPI.Areas.Admin.Controllers
{
    public class UserManagerController : Controller
    {
        private easier_database db = new easier_database();
        // GET: Admin/UserManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register(UserRegisterModel userRegisterModel)
        {
            if (ModelState.IsValid)
            {
                if (CommonUtils.IsValidEmail(userRegisterModel.Email))
                {
                    if (userRegisterModel.HashPass.Equals(userRegisterModel.ConfirmHashPass))
                    {
                        if (!UserExists(userRegisterModel.Email))
                        {
                            User mUser = new User(userRegisterModel);
                            db.Users.Add(mUser);
                            db.SaveChanges();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Confirm Password is not correct!");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Confirm Password is not correct!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not match!");
                }
            }
            return View("Index");
        }

        private bool UserExists(string email)
        {
            return db.Users.Count(e => e.Email == email) > 0;
        }
    }
}