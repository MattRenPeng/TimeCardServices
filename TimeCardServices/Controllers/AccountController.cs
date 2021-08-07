using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TimeCardServices.Domain;
using TimeCardServices.Model;
using TimeCardServices.Repository;
using TimeCardServices.Services;
using TimeCardServices.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeCardServices.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
      private readonly  IUserService _service;

        public AccountController(IUserService service)
        {
            _service = service;
        }

        // POST api/<AccountController>
        [HttpPost("Login")]
        public ObjectResult Login([FromBody] UserViewModel user)
        {
            if ( ModelState.IsValid)
            //if (TryValidateModel(user) && ModelState.IsValid)
             {
                return getToken(user.UserName, user.Password, (int)user.UserType);
            }
            else
            {
                return VaildateHelper.ReturnMessageObjectResult(ModelState);
            }
        }
        [HttpGet("getToken")]
        public ObjectResult getToken(string userName,string password,int userType)
        {
            UserViewModel user = new UserViewModel() { UserName = userName, Password = password };
            if (userType ==0 || userType == 1 )
            {
                user.UserType = (UserType)userType;
                //if (TryValidateModel(user) && ModelState.IsValid)
             if (ModelState.IsValid)
                  {
                    User _user = _service.CheckLogin(user);
                    if (_user != null)
                    {
                        string token = SecurityUtity.GetToken(_user, user.UserType.ToString());
                        return new OkObjectResult(token);
                    }
                    else
                    {
                        ModelState.AddModelError("", "User name or password is wrong!");
                        return VaildateHelper.ReturnMessageObjectResult(ModelState);
                    }
                }
                else
                {
                    return VaildateHelper.ReturnMessageObjectResult(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError("UserType", "User type is only for admin or staff!");
                return VaildateHelper.ReturnMessageObjectResult(ModelState);
            }

        }

        [HttpPost("signUp")]
        public ObjectResult signUp([FromBody] UserForSignUpViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (_service.GetUserByUsername(user.UserName) != null)
                {
                    ModelState.AddModelError("UserName", "User name has existed!");
                    return VaildateHelper.ReturnMessageObjectResult(ModelState);
                }
                else
                {
                   int numerEffect= _service.AddUser(user,user.UserName);
                    return new OkObjectResult(numerEffect);
                }
            }
            else
            {
                return VaildateHelper.ReturnMessageObjectResult(ModelState);
            }

        }


    }
}
