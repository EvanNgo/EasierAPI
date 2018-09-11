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
        [HttpPost]
        public ResponseMessageModels GetQuestion(User mUser, [FromUri]int offset, [FromUri]DateTime dateTime)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var mAnswer = from answer
                                      in db.QuestionAnswers
                          where answer.UserId == mUser.Id
                          select new
                          {
                              AnswerQuestionId = answer.QuestionId,
                              AnswerUserId = answer.UserId
                          };
            var mQuestions = from mQuestion in db.Questions
                           join a in mAnswer on mQuestion.Id equals a.AnswerQuestionId into answerTemp
                           from a in answerTemp.DefaultIfEmpty()
                           select new
                           {
                               Id = mQuestion.Id,
                               Title = mQuestion.Title,
                               Thumbnail = mQuestion.Thumbnail,
                               Level = mQuestion.Level,
                               ColorId = mQuestion.ColorId,
                               Content = mQuestion.Content,
                               Type = mQuestion.Type,
                               isHaveAnswer = mQuestion.isHaveAnswer,
                               UserId = mQuestion.UserId,
                               AnswerCount = mQuestion.AnswerCount,
                               CommentCount = mQuestion.CommentCount,
                               LikeCount = mQuestion.LikeCount,
                               CreatedDate = mQuestion.CreatedDate,
                               AnswerUserID = a == null ? -1 : a.AnswerUserId
                           };
            var question = (from u in mQuestions
                            where u.AnswerUserID == -1 && u.CreatedDate < dateTime
                            orderby u.CreatedDate descending
                            select new
                            {
                                id = u.Id,
                                title = u.Title,
                                thumbnail = u.Thumbnail,
                                level = u.Level,
                                colorid = u.ColorId,
                                content = u.Content,
                                type = u.Type,
                                is_have_answer = u.isHaveAnswer,
                                comment_count = u.CommentCount,
                                like_count = u.LikeCount,
                                answer_count = u.AnswerCount,
                                created_date = u.CreatedDate,
                                is_liked = (from like in db.QuestionLikes where mUser.Id == like.UserId && like.QuestionId==u.Id select like.UserId).Count() > 0,
                                choices = from choice in db.Choices
                                            where choice.QuestionId == u.Id
                                            select new
                                            {
                                                id = choice.Id,
                                                message = choice.Message,
                                                istrue = choice.IsTrue,
                                                thumbnail = choice.Thumbnail,
                                                selectedcount = choice.SelectedCount
                                            },
                                user = (from user in db.Users
                                        where user.Id == u.UserId
                                        select new { username = user.UserName,
                                                    id = user.Id,
                                                    thumbnail = user.Thumbnail}).FirstOrDefault()
                            }).Take(offset);
            if (question == null)
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

        [Route("api/question/edit")]
        [HttpPost]
        public ResponseMessageModels EditQuestion(Question mQuestion)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            Question question = db.Questions.Find(mQuestion.Id);
            if (question == null || mQuestion.UserId != question.UserId)
            {
                result.status = 0;
                result.message = "Unknown Error";
                result.data = null;
                return result;
            }
            question.Title = question.Title;
            db.SaveChanges();
            result.status = 1;
            result.message = "Edit Question Successfully";
            result.data = null;
            return result;
        }

        [Route("api/created/questions")]
        [HttpPost]
        public ResponseMessageModels GetCreatedQuestion(User mUser, [FromUri]int offset, [FromUri]DateTime dateTime)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var question = (from u in db.Questions
                            where u.UserId == mUser.Id && u.CreatedDate < dateTime
                            orderby u.CreatedDate descending
                            select new
                            {
                                id = u.Id,
                                title = u.Title,
                                thumbnail = u.Thumbnail,
                                level = u.Level,
                                colorid = u.ColorId,
                                content = u.Content,
                                type = u.Type,
                                comment_count = u.CommentCount,
                                like_count = u.LikeCount,
                                answer_count = u.AnswerCount,
                                created_date = u.CreatedDate,
                                is_liked = (from like in db.QuestionLikes where mUser.Id == like.UserId && like.QuestionId == u.Id select like.UserId).Count() > 0,
                            }).Take(offset);
            result.status = 1;
            result.message = "";
            result.data = question;
            return result;
        }

        [Route("api/answered/questions")]
        [HttpPost]
        public ResponseMessageModels GetAnsweredQuestion(User mUser, [FromUri]int offset, [FromUri]DateTime dateTime)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var question = (from u in db.Questions
                            join a in (from temp in db.QuestionAnswers where temp.UserId == mUser.Id select temp) on u.Id equals a.QuestionId
                            where a.UserId == mUser.Id && u.CreatedDate < dateTime
                            orderby u.CreatedDate descending
                            select new
                            {
                                id = u.Id,
                                title = u.Title,
                                thumbnail = u.Thumbnail,
                                level = u.Level,
                                colorid = u.ColorId,
                                content = u.Content,
                                type = u.Type,
                                selected_choice_id = a.ChoiceId,
                                is_answered = true,
                                is_have_answer = u.isHaveAnswer,
                                comment_count = u.CommentCount,
                                like_count = u.LikeCount,
                                answer_count = u.AnswerCount,
                                created_date = u.CreatedDate,
                                is_liked = (from like in db.QuestionLikes where mUser.Id == like.UserId && like.QuestionId == u.Id select like.UserId).Count() > 0,
                                choices = from choice in db.Choices
                                          where choice.QuestionId == u.Id
                                          select new
                                          {
                                              id = choice.Id,
                                              message = choice.Message,
                                              istrue = choice.IsTrue,
                                              thumbnail = choice.Thumbnail,
                                              selectedcount = choice.SelectedCount
                                          },
                                user = (from user in db.Users
                                        where user.Id == u.UserId
                                        select new
                                        {
                                            username = user.UserName,
                                            id = user.Id,
                                            thumbnail = user.Thumbnail
                                        }).FirstOrDefault()
                            }).Take(offset);
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
            question.AnswerCount = 0;
            for (int i = 0; i < question.Choices.Count; i++) {
                question.Choices.ElementAt(i).SelectedCount = 0;
            }
            question.CreatedDate = DateTime.Now;
            db.Questions.Add(question);
            db.SaveChanges();           
            var mQuestion = from u in db.Questions
                            where u.Id == question.Id
                            select new
                            {
                                id = u.Id,
                                title = u.Title,
                                thumbnail = u.Thumbnail,
                                level = u.Level,
                                answercount = u.AnswerCount,
                                ishaveanswer = u.isHaveAnswer,
                                colorid = u.ColorId,
                                content = u.Content,
                                type = u.Type,
                                created_date = u.CreatedDate,
                                choices = from choice in db.Choices
                                          where choice.QuestionId == u.Id
                                          select new
                                          {
                                              id = choice.Id,
                                              message = choice.Message,
                                              istrue = choice.IsTrue,
                                              thumbnail = choice.Thumbnail,
                                              selectedcount = choice.SelectedCount
                                          },
                                user = (from user in db.Users
                                        where user.Id == u.UserId
                                        select new
                                        {
                                            username = user.UserName,
                                            id = user.Id,
                                            thumbnail = user.Thumbnail
                                        }).FirstOrDefault()};
            result.status = 1;
            result.message = "Question is Created";
            result.data = mQuestion;
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

        [Route("api/question/delete")]
        [HttpPost]
        public ResponseMessageModels Remove(Question mQuestion)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            Question question = db.Questions.Where(a => a.Id == mQuestion.Id).SingleOrDefault();
            
            if (question == null)
            {
                result.status = 0;
                result.message = "Not Found";
                result.data = null;
                return result;
            }
            var choices = db.Choices.Where(b => b.QuestionId == question.Id).AsEnumerable();
            foreach (var choice in choices)
            {
                db.Choices.Remove(choice);
            }

            var comments = db.QuestionComments.Where(b => b.QuestionId == question.Id).AsEnumerable();
            foreach (var comment in comments)
            {
                db.QuestionComments.Remove(comment);
            }

            var likes = db.QuestionLikes.Where(b => b.QuestionId == question.Id).AsEnumerable();
            foreach (var like in likes)
            {
                db.QuestionLikes.Remove(like);
            }

            db.SaveChanges();
            db.Questions.Remove(question);
            db.SaveChanges();
            result.status = 1;
            result.message = "Remove Successfuly";
            result.data = null;
            return result;
        }

        [Route("api/question")]
        [HttpPost]
        public ResponseMessageModels GetQuestionByID(Question mQuestion)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            Question question = db.Questions.Find(mQuestion.Id);
            if (question == null)
            {
                result.status = 0;
                result.message = "Not Found";
                result.data = null;
                return result;
            }
            result.status = 1;
            result.message = "Get Question Successfully";
            result.data = question;
            return result;
        }
    }
}