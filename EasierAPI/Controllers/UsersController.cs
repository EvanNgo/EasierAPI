using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using EasierAPI.Models;

namespace EasierAPI.Controllers
{
    public class UsersController : ApiController
    {
        private easier_database db = new easier_database();

        /*
        // GET: api/users
        [Route("api/users")]
        [HttpGet]
        public ResponseMessageModels GetUsers()
        {
            ResponseMessageModels result = new ResponseMessageModels();
            var user = from u in db.Users
                       select new
                       {
                           id = u.Id,
                           email = u.Email,
                           username = u.UserName,
                           thumbnail = u.Thumbnail
                       };
            if (user == null) {
                result.status = 0;
                result.message = "Is Empty";
                result.data = null;
            }
            result.status = 1;
            result.message = "Get List User Successfully";
            result.data = user;
            return result;
        }
        */

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
            String hash = getMd5Hash(mUser.HashPass);
            String mEmail = mUser.Email + ".com";
            var user = from u in db.Users
                        where u.Email == mEmail && u.HashPass == hash
                        select new {
                            id = u.Id,
                            email = u.Email,
                            username = u.UserName,
                            thumbnail = u.Thumbnail
                        };
            if (user == null || user.Count() <= 0)
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


    //Register
        [Route("api/user/register")]
        [HttpPost]
        public ResponseMessageModels Register(User mUser)
        {
            ResponseMessageModels result = new ResponseMessageModels();
            mUser.HashPass = getMd5Hash(mUser.HashPass);
            mUser.Email = mUser.Email + ".com";
            if (UserExists(mUser.Email)) {
                result.status = 0;
                result.message = "Register Fail, This is Email is used";
                result.data = null;
                return result;
            }
            db.Users.Add(mUser);
            db.SaveChanges();
            var id = mUser.Id;
            result.status = id;
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

        private string getMd5Hash(string input)
        { // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create(); // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes // and create a string.
            StringBuilder sBuilder = new StringBuilder(); // Loop through each byte of the hashed data // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

    }
}