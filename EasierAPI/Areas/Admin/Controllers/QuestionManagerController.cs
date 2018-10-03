using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasierAPI.Models;
using EasierAPI.Utils;
using PagedList;

namespace EasierAPI.Areas.Admin.Controllers
{
    public class QuestionManagerController : Controller
    {
        private easier_database db = new easier_database();
        // GET: Admin/QuestionManager
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var questions = db.Questions.OrderByDescending(i => i.CreatedDate).ToPagedList(page, pageSize);
            ViewBag.Questions = questions;
            return View();
        }

        public ActionResult CreateQuestion()
        {
            return View("Index", new
            {
                page = 1,
                pageSize = 10
            });
        }
    }
}