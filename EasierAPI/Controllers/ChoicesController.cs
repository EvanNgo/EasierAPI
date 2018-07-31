using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EasierAPI.Models;

namespace EasierAPI.Controllers
{
    public class ChoicesController : ApiController
    {
        private easier_database db = new easier_database();

        [Route("api/choices")]
        [HttpGet]
        public ResponseMessageModels GetChoices()
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var choice = from u in db.Choices
                         select new
                         {
                             id = u.Id,
                             message = u.Message,
                             thumbnail = u.Thumbnail,
                             isTrue = u.IsTrue,
                             selectedCount = u.SelectedCount,
                             questionId = u.QuestionId };
            if (choice == null)
            {
                result.status = 0;
                result.message = "Is Empty";
                result.data = null;
            }
            result.status = 1;
            result.message = "Get List Choice Successfully";
            result.data = choice;
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChoiceExists(int id)
        {
            return db.Choices.Count(e => e.Id == id) > 0;
        }
    }
}