//using Planc.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Http;

//namespace Planc.Controllers
//{
//    //public class AccountApiController : ApiController
//    {
//        // GET api/<controller>
//        public void Get( )
//        {
            
//        }

//        // GET api/<controller>/5
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST api/<controller>
//        public async Task<IEnumerable<string>> Post(LoginViewModel model)
//        {
//            // This doen't count login failures towards lockout only two factor authentication
//            // To enable password failures to trigger lockout, change to shouldLockout: true
//            var result = await SignInHelper.PasswordSignIn(model.Email, model.Password, model.RememberMe, shouldLockout: false);
//            return result;
//        }
//        private SignInHelper SignInHelper
//        {
//            get
//            {
//                if (_helper == null)
//                {
//                    _helper = new SignInHelper(UserManager, AuthenticationManager);
//                }
//                return _helper;
//            }
//        }
//        private ApplicationUserManager _userManager;
//        public ApplicationUserManager UserManager
//        {
//            get
//            {
                
//                return _userManager ?? HttpContext.Current.GetOwinContext().Authentication GetUserManager<ApplicationUserManager>();
//            }
//            private set
//            {
//                _userManager = value;
//            }
//        }
//        // PUT api/<controller>/5
//        public void Put(int id, [FromBody]string value)
//        {
//        }

//        // DELETE api/<controller>/5
//        public void Delete(int id)
//        {
//        }
//    }
//}