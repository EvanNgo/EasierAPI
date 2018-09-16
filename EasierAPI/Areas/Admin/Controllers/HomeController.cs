using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasierAPI.Models;

namespace EasierAPI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private easier_database db = new easier_database();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
    }
}