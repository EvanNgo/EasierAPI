using System;
using System.Data;
using System.Linq;
using System.Web.Http;
using EasierAPI.Models;

namespace EasierAPI.Controllers
{
    public class UsersController : ApiController
    {
        private easier_database db = new easier_database();

        // GET: api/user
        [Route("api/user")]
        [HttpGet]
        public ResponseMessageModels GetUser(User mUser)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var user = (from u in db.Users
                       where u.Id == mUser.Id && u.Email == mUser.Email
                       select new
                       {
                           id = u.Id,
                           email = u.Email,
                           username = u.UserName,
                           thumbnail = u.Thumbnail
                       }).FirstOrDefault();
            if (user == null) {
                result.status = 0;
                result.message = "Login Session Time Out";
                result.data = null;
            }
            result.status = 1;
            result.message = "Get User Successfully";
            result.data = user;
            return result;
        }
       

        //Login
        [Route("api/user/login")]
        [HttpPost]
        public ResponseMessageModels Login(User mUser)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            if (mUser.Email == null || mUser.HashPass == null) {
                result.status = 0;
                result.message = "Email or Password is empty";
                result.data = null;
                return result;
            }
            String mEmail = mUser.Email + ".com";
            var user = (from u in db.Users
                       where u.Email == mEmail && u.HashPass == mUser.HashPass
                       select new {
                            id = u.Id,
                            email = u.Email,
                            username = u.UserName,
                            thumbnail = u.Thumbnail,
                            type = u.Type
                       }).FirstOrDefault();
            if (user == null)
            {
                result.status = 0;
                result.message = "Invailid Email or Passsword";
                result.data = null;
                return result;
            }
            result.status = 1;
            result.message = "Login Successfully";
            result.data = user;
            return result;
        }

        [Route("api/user/loginfb")]
        [HttpPost]
        public ResponseMessageModels LoginWithFacebook(User mUser)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            if (mUser.FbId == null)
            {
                result.status = 0;
                result.message = "Email or Password is empty";
                result.data = null;
                return result;
            }
            mUser.Email = mUser.Email + ".com";
            int id = FacebookIdExists(mUser.FbId);
            if (id == -1) {
                db.Users.Add(mUser);
                db.SaveChanges();
                var putBack = (from u in db.Users
                                   where u.Id == mUser.Id
                                   select new
                                   {
                                       id = u.Id,
                                       email = u.Email,
                                       username = u.UserName,
                                       thumbnail = u.Thumbnail,
                                       fbid = u.FbId,
                                       type = u.Type
                                   }).FirstOrDefault();
                result.status = 1;
                result.message = "Register With Facebook Success";
                result.data = putBack;
                return result;
            }
            mUser.Id = id;
            User user = db.Users.SingleOrDefault(u => u.Id == mUser.Id);
            if (user == null)
            {
                result.status = 0;
                result.message = "Error: User ID Invalid";
                result.data = null;
                return result;
            }
            user.Email = mUser.Email;
            user.UserName = mUser.UserName;
            user.Thumbnail = mUser.Thumbnail;
            db.SaveChanges();
            var putBackUser = (from u in db.Users
                               where u.Id == mUser.Id
                               select new
                               {
                                   id = u.Id,
                                   email = u.Email,
                                   username = u.UserName,
                                   thumbnail = u.Thumbnail,
                                   fbid = u.FbId,
                                   type = u.Type
                               }).FirstOrDefault();
            result.status = 1;
            result.message = "Login With Facebook Success";
            result.data = putBackUser;
            return result;
        }


        //Update User Profile
        [Route("api/user/edit")]
        [HttpPost]
        public ResponseMessageModels EditProfile(User mUser)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            User user = db.Users.Find(mUser.Id);
            if ( user == null )
            {
                result.status = 0;
                result.message = "Error: User ID Invalid";
                result.data = null;
                return result;
            }
            user.UserName = mUser.UserName;
            user.Thumbnail = mUser.Thumbnail;
            db.SaveChanges();
            var putBackUser = (from u in db.Users
                        where u.Id == user.Id && u.Email == user.Email
                        select new
                        {
                            id = u.Id,
                            email = u.Email,
                            username = u.UserName,
                            thumbnail = u.Thumbnail,
                            type = u.Type
                        }).FirstOrDefault();
            result.status = 1;
            result.message = "Get User Successfully";
            result.data = putBackUser;
            return result;
        }

        //Register
        [Route("api/user/register")]
        [HttpPost]
        public ResponseMessageModels Register(User mUser)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            mUser.Email = mUser.Email + ".com";
            if (UserExists(mUser.Email)) {
                result.status = 0;
                result.message = "Register Fail, This is Email is used";
                result.data = null;
                return result;
            }
            db.Users.Add(mUser);
            db.SaveChanges();
            result.status = mUser.Id;
            result.message = "Regsiter Successfully";
            result.data = null;
            return result;
        }

        /*
        [Route("api/user/delete")]
        [HttpPost]
        public ResponseMessageModels Remove(User mUser)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            User user = db.Users.Find(mUser.Id);
            if (user == null || ) {
                result.status = 0;
                result.message = "Not Found";
                result.data = null;
                return result;
            }
            db.Users.Remove(user);
            db.SaveChanges();
            result.status = 1;
            result.message = "Remove Successfuly";
            result.data = null;
            return result;
        }
        */

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string email)
        {
            return db.Users.Count(e => e.Email == email) > 0;
        }

        private int FacebookIdExists(string fbId)
        {
            User user = db.Users.Where(u => u.FbId.Equals(fbId)).FirstOrDefault();
            if (user == null) {
                return -1;
            }
            return user.Id;
        }
    }
}