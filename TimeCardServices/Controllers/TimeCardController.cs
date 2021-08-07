using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCardServices.Domain;
using TimeCardServices.Services;
using TimeCardServices.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeCardServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TimeCardController : ControllerBase
    {

        private readonly ITimeCardService _service;

        public TimeCardController(ITimeCardService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin")]
        // GET api/<TimeCardController>/5
        [HttpGet("{weekStart}/{userName}")]
        public ObjectResult Get(DateTime weekStart,string userName)
        {
            return new OkObjectResult(_service.GetOneWeekData(userName,weekStart));
        }
        
        [HttpGet("{weekStart}")]
        public ObjectResult GetCurrentTimeCard(DateTime weekStart)
        {
            string userName = HttpContext.User.Identity.Name;
            return new OkObjectResult(_service.GetOneWeekData(userName, weekStart));
        }

        [HttpPost("AddOneWekkData")]
        public ObjectResult AddOneWeekData([FromBody] TimeCardViewModel weekData)
        {
            if (ModelState.IsValid)
            {
                if (_service.GetOneWeekData(weekData.UserName, weekData.WeekStart) != null)
                {
                    ModelState.AddModelError("WeekStart", "The data of this week has existed!");
                    return VaildateHelper.ReturnMessageObjectResult(ModelState);
                }
                else
                {
                    //string userName = HttpContext.User.Identity.Name;
                    //weekData.CreateDate = DateTime.Now;
                    //weekData.UpdateDate = DateTime.Now;
                    //weekData.CreateUser = userName;
                    //weekData.UpdateUser = userName;
                    int numerEffect = _service.AddOneWeekData(weekData);
                    return new OkObjectResult(numerEffect);
                }
            }
            else
            {
                return VaildateHelper.ReturnMessageObjectResult(ModelState);
            }
        }

        [HttpDelete("Delete/{weekStart}")]
        public ObjectResult Delete(DateTime weekStart)
        {
            string userName = HttpContext.User.Identity.Name;
            int flag = _service.DeleteOneWeekData(userName, weekStart);
            if (flag <= 0)
            {
                ModelState.AddModelError("UserName", "This week data is not exists!");
                return VaildateHelper.ReturnMessageObjectResult(ModelState);
            }
            else
                return new OkObjectResult(flag);
        }
        [HttpDelete("Delete/{weekStart}/{userName}")]
        public ObjectResult DeleteByUserName(DateTime weekStart, string userName)
        {
            int flag = _service.DeleteOneWeekData(userName, weekStart);
            if (flag <= 0)
            {
                ModelState.AddModelError("UserName", "This week data is not exists!");
                return VaildateHelper.ReturnMessageObjectResult(ModelState);
            }
            else
                return new OkObjectResult(flag);
        }

    }
}
