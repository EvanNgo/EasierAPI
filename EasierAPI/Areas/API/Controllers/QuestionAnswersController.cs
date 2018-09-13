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
using EasierAPI.Areas.API.Models;

namespace EasierAPI.Controllers
{
    public class QuestionAnswersController : ApiController
    {
        private easier_database db = new easier_database();

        [Route("api/question/answer")]
        [HttpPost]
        public ResponseMessageModels AnswerQuestion(QuestionAnswer mQuestionAnswer)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            mQuestionAnswer.AnsweredDate = DateTime.Now;
            db.QuestionAnswers.Add(mQuestionAnswer);
            Choice choice = db.Choices.Find(mQuestionAnswer.ChoiceId);
            Question question = db.Questions.Find(mQuestionAnswer.QuestionId);
            choice.SelectedCount++;
            question.AnswerCount++;
            db.SaveChanges();
            result.status = 1;
            result.message = "Answered";
            result.data = question.AnswerCount;
            result.datetime = mQuestionAnswer.AnsweredDate;
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

        private bool QuestionAnswerExists(int id)
        {
            return db.QuestionAnswers.Count(e => e.Id == id) > 0;
        }
    }
}