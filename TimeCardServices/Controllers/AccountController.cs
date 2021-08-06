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

        // GET: api/<AccountController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [HttpPost]
        public ObjectResult Login([FromBody] UserViewModel user)
        {
            if ( ModelState.IsValid)
            //if (TryValidateModel(user) && ModelState.IsValid)
                {
                return getToken(user.UserName, user.Password, user.UserType?.ToString());
            }
            else
            {
                return VaildateHelper.ReturnMessageObjectResult(ModelState);
            }
        }
        [HttpGet("getToken")]
        public ObjectResult getToken(string userName,string password,string userType)
        {
            UserViewModel user = new UserViewModel() { UserName = userName, Password = password };

            UserType EnumUseType= UserType.Staff;
            if (!string.IsNullOrEmpty(userType) && Enum.TryParse<UserType>(userType, out EnumUseType))
            {
                user.UserType = EnumUseType;
                //if (TryValidateModel(user) && ModelState.IsValid)
             if (ModelState.IsValid)
                  {
                    User _user = _service.CheckLogin(user);
                    if (_user != null)
                    {
                        string token = SecurityUtity.GetToken(_user, userType);
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
                   int numerEffect= _service.AddUser(user);
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
