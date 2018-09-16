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
                            type = u.Type,
                           created_date = u.CreatedDate
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

        [Route("api/user/loginsns")]
        [HttpPost]
        public ResponseMessageModels LoginWithSNS(User mUser)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            if (mUser.SnsId == null)
            {
                result.status = 0;
                result.message = "Login Faild";
                result.data = null;
                return result;
            }
            mUser.Email = mUser.Email + ".com";
            int id = SnsIdExists(mUser);
            if (id == -1) {
                if (UserExists(mUser.Email))
                {
                    result.status = 3;
                    result.message = "Email is Exists";
                    result.data = null;
                    return result;
                }
                mUser.CreatedDate = DateTime.Now;
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
                                       snsid = u.SnsId,
                                       type = u.Type,
                                       created_date = u.CreatedDate
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
                result.status = 2;
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
                                   snsid = u.SnsId,
                                   type = u.Type,
                                   created_date = u.CreatedDate
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
            mUser.CreatedDate = DateTime.Now;
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

        private int SnsIdExists(User mUser)
        {
            var putBack = (from u in db.Users
                           where u.SnsId == mUser.SnsId && u.Type == mUser.Type
                           select new
                           {
                               Id = u.Id,
                           }).FirstOrDefault();
            if (putBack == null) {
                return -1;
            }
            return putBack.Id;
        }
    }
}