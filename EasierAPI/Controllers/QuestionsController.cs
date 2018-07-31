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
    public class QuestionsController : ApiController
    {
        private easier_database db = new easier_database();

        [Route("api/questions")]
        [HttpGet]
        public ResponseMessageModels GetQuestion()
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var question = from u in db.Questions
                           select new
                           {
                               id = u.Id,
                               title = u.Title,
                               thumbnail = u.Thumbnail,
                               level = u.Level,
                               answercount = u.AnswerCount,
                               colorid = u.ColorId,
                               message = u.Message,
                               choices = from choice in db.Choices where choice.QuestionId == u.Id select new {
                                   id = choice.Id,
                                   message = choice.Message,
                                   istrue = choice.IsTrue,
                                   thumbnail = choice.Thumbnail,
                                   selectedcount = choice.SelectedCount
                               },
                               user = from user in db.Users
                                      where user.Id == u.UserId
                                      select new { userName = user.UserName,
                                                   id = user.Id,
                                                   thumbnail = user.Thumbnail}
                       };
            if (question == null || question.Count() <= 0)
            {
                result.status = 0;
                result.message = "Is Empty";
                result.data = null;
            }
            result.status = 1;
            result.message = "Get List User Successfully";
            result.data = question;
            return result;
        }

        [Route("api/question/create")]
        [HttpPost]
        public ResponseMessageModels CrateQuestion(Question question)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            db.Questions.Add(question);
            db.SaveChanges();
            for (int i = 0; i < 4; i++)
            {
                Choice choice = new Choice();
                choice.Message = "Choice " + i;
                choice.QuestionId = question.Id;
                db.Choices.Add(choice);
                db.SaveChanges();
                question.Choices.Add(choice);
            }
            result.status = 1;
            result.message = "Question is Created";
            result.data = question;
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

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.Id == id) > 0;
        }


    }
}