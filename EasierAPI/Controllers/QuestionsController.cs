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
    public class QuestionsController : ApiController
    {
        private easier_database db = new easier_database();

        [Route("api/questions/")]
        [HttpPost]
        public ResponseMessageModels GetQuestion(User mUser)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var question = (from u in db.Questions
                           orderby u.Id descending
                           select new
                           {
                               id = u.Id,
                               title = u.Title,
                               thumbnail = u.Thumbnail,
                               level = u.Level,
                               colorid = u.ColorId,
                               content = u.Content,
                               type = u.Type,
                               selectedchoice = (from answer in db.QuestionAnswers where answer.QuestionId == u.Id && answer.UserId == u.UserId select answer.ChoiceId).FirstOrDefault(),
                               isanswered = (from answer in db.QuestionAnswers where answer.QuestionId == u.Id && answer.UserId == u.UserId select answer.ChoiceId).Count() > 0,
                               ishaveanswer = u.isHaveAnswer,
                               commentcount = (from comment in db.QuestionComments where comment.QuestionId == u.Id select comment.UserId).Count(),
                               likecount = (from like in db.QuestionLikes where like.QuestionId == u.Id select like.UserId).Count(),
                               answercount = (from answer in db.QuestionAnswers where answer.QuestionId == u.Id select answer.UserId).Count(),
                               isliked = (from like in db.QuestionLikes where mUser.Id == like.UserId && like.QuestionId==u.Id select like.UserId).Count() > 0,
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
                           }).Take(10);
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
            question.AnswerCount = 0;
            for (int i = 0; i < question.Choices.Count; i++) {
                question.Choices.ElementAt(i).SelectedCount = 0;
            }
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