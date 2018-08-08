﻿using System;
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
    public class QuestionAnswersController : ApiController
    {
        private easier_database db = new easier_database();

        [Route("api/question/answer")]
        [HttpPost]
        public ResponseMessageModels AnswerQuestion(QuestionAnswer mQuestionAnswer)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            db.QuestionAnswers.Add(mQuestionAnswer);
            db.SaveChanges();
            result.status = 1;
            result.message = "Answered";
            result.data = null;
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