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
    public class QuestionLikesController : ApiController
    {
        private easier_database db = new easier_database();

        [Route("api/question/like")]
        [HttpPost]
        public ResponseMessageModels LikeQuestion(QuestionLike mQuestionLike)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var likes = (from u in db.QuestionLikes
                       where u.UserId == mQuestionLike.UserId && u.QuestionId == mQuestionLike.QuestionId
                        select u).FirstOrDefault();
            Question question = db.Questions.Find(mQuestionLike.QuestionId);
            if (likes == null)
            {
                db.QuestionLikes.Add(mQuestionLike);
                question.LikeCount++;
                db.SaveChanges();
                result.status = 1;
                result.message = "Liked";
                result.data = (from like in db.QuestionLikes where like.QuestionId == mQuestionLike.QuestionId select mQuestionLike.Id).Count();
                return result;
            }
            db.QuestionLikes.Remove(likes);
            question.LikeCount--;
            question.LikeCount = question.LikeCount > 0 ? question.LikeCount-- : 0;
            db.SaveChanges();
            result.status = 2;
            result.message = "Dislike";
            result.data = question.LikeCount;
            return result;
        }

        [Route("api/likes/")]
        [HttpGet]
        public ResponseMessageModels GetLike()
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var likes = from u in db.QuestionLikes
                         select new
                         {
                             id = u.Id,
                             questionid = u.QuestionId,
                             userid = u.UserId
                         };
            if (likes == null)
            {
                result.status = 1;
                result.message = "NULL";
                result.data = null;
                return result;
            }
            result.status = 2;
            result.message = "Successfully";
            result.data = likes;
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

        private bool QuestionLikeExists(int id)
        {
            return db.QuestionLikes.Count(e => e.Id == id) > 0;
        }
    }
}