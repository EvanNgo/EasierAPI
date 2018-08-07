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
    public class QuestionCommentsController : ApiController
    {
        private easier_database db = new easier_database();

        [Route("api/question/comment/delete")]
        [HttpPost]
        public ResponseMessageModels RemoveCommentQuestion(QuestionComment mComment)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            QuestionComment comment = db.QuestionComments.Find(mComment.Id);
            db.QuestionComments.Remove(comment);
            db.SaveChanges();
            result.status = 1;
            result.message = "Remove Comment Successfully";
            result.data = null;
            return result;
        }

        [Route("api/question/comment/add")]
        [HttpPost]
        public ResponseMessageModels AddCommentQuestion(QuestionComment mComment)
        {
            ResponseMessageModels result = new ResponseMessageModels();
           
            db.QuestionComments.Add(mComment);
            db.SaveChanges();
            result.status = 1;
            result.message = "Added Comment Successfully";
            result.data = null;
            return result;
        }

        [Route("api/question/comments/")]
        [HttpPost]
        public ResponseMessageModels GetQuestionComment(Question mQuestion)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var comments = from u in db.QuestionComments
                           where mQuestion.Id == u.QuestionId
                           select new
                           {
                               id = u.Id,
                               message = u.Message,
                               questionid = u.QuestionId,
                               createddate = u.CreatedDate,
                               user = (from user in db.Users where user.Id == u.UserId
                                       select new
                                       {
                                           id = u.Id,
                                           email = user.Email,
                                           username = user.UserName,
                                           thumbnail = user.Thumbnail
                                       }).FirstOrDefault()
        };
            result.status = 1;
            result.message = "Get Comment List Successfully";
            result.data = comments;
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

        private bool QuestionCommentExists(int id)
        {
            return db.QuestionComments.Count(e => e.Id == id) > 0;
        }
    }
}