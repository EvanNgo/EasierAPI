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
    public class QuestionAnswersController : ApiController
    {
        private easier_database db = new easier_database();

        // GET: api/QuestionAnswers
        public IQueryable<QuestionAnswer> GetQuestionAnswers()
        {
            return db.QuestionAnswers;
        }

        // GET: api/QuestionAnswers/5
        [ResponseType(typeof(QuestionAnswer))]
        public IHttpActionResult GetQuestionAnswer(int id)
        {
            QuestionAnswer questionAnswer = db.QuestionAnswers.Find(id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            return Ok(questionAnswer);
        }

        // PUT: api/QuestionAnswers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuestionAnswer(int id, QuestionAnswer questionAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questionAnswer.Id)
            {
                return BadRequest();
            }

            db.Entry(questionAnswer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionAnswerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/QuestionAnswers
        [ResponseType(typeof(QuestionAnswer))]
        public IHttpActionResult PostQuestionAnswer(QuestionAnswer questionAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.QuestionAnswers.Add(questionAnswer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = questionAnswer.Id }, questionAnswer);
        }

        // DELETE: api/QuestionAnswers/5
        [ResponseType(typeof(QuestionAnswer))]
        public IHttpActionResult DeleteQuestionAnswer(int id)
        {
            QuestionAnswer questionAnswer = db.QuestionAnswers.Find(id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            db.QuestionAnswers.Remove(questionAnswer);
            db.SaveChanges();

            return Ok(questionAnswer);
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