using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasierAPI.Models;
using EasierAPI.Utils;

namespace EasierAPI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private easier_database db = new easier_database();
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewBag.User = db.Users.ToList();
            ViewBag.Question = db.Questions.ToList();
            return View();
        }
    }
}