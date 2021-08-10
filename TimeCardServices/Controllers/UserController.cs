using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCardServices.Domain;
using TimeCardServices.Model;
using TimeCardServices.Services;
using TimeCardServices.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeCardServices.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        //GET: api/User/GetAllUser
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUser")]
        public ObjectResult GetAll()
        {
            List<User> users = _service.GetAllUserInfo();
            string jsonUser = Newtonsoft.Json.JsonConvert.SerializeObject(users);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserInfoViewModel>>(jsonUser);
            return new OkObjectResult(result);
        }

        //GET: api/User/GetCurrentUserInfo
        [Authorize]
        [HttpGet("GetCurrentUserInfo")]
        public UserInfoViewModel GetCurrentUserInfo()
        {
            User user = _service.GetUserByUsername(HttpContext.User.Identity.Name);
            string jsonUser = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoViewModel>(jsonUser);
            return result;

        }

        // DELETE api/<UserController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{userName}")]
        public ObjectResult Delete(string userName)
        {
            int flag = _service.DeleteOneUser(userName);
            if (flag <= 0)
            {
                ModelState.AddModelError("UserName", "The deleting user is not exists!");
                return VaildateHelper.ReturnMessageObjectResult(ModelState);
            }
            else
                return new OkObjectResult(flag);
        }
    }
}
