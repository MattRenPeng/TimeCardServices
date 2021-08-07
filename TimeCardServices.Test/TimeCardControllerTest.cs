using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TimeCardServices.Controllers;
using TimeCardServices.Domain;
using TimeCardServices.Model;
using TimeCardServices.Repository;
using TimeCardServices.Services;
using Xunit;

namespace TimeCardServices.Test
{
    public class TimeCardControllerTest : IDisposable
    {
        TimeCardDBContext DB;
        public void Dispose()
        {
            DB?.Dispose();
        }
        private TimeCardController controller;
        private static String user_name = "zhangsan";
        private static String user_name2 = "lisi";
        private static DateTime current_week = new DateTime(2021, 8, 2);
        TimeCard NewOneWeekData = new TimeCard()
        {
            UserName = user_name,
            WeekStart = current_week,
            Day1 = 8,
            Day2 = 8,
            Day3 = 8,
            Day4 = 8,
            Day5 = 8,
            Day6 = null,
            Day7 = null,
            Notes = "Remarks",
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            CreateUser = user_name,
            UpdateUser = user_name
        };
        public TimeCardControllerTest()
        {
            //var builder = new DbContextOptionsBuilder<TimeCardDBContext>()
            //   .UseSqlServer("");

            var builder = new DbContextOptionsBuilder<TimeCardDBContext>()
              .UseInMemoryDatabase("TimeCardDB3");

            DB = new TimeCardDBContext(builder.Options);
            //init test data, if not exists then add zhangsan count
            if (DB.Users.Count(f => f.UserName == user_name) == 0)
            {
                User NewUser = new Model.User() { UserName = user_name, Password = "12345678" };
                NewUser.Password = Utility.SecurityUtity.HashPassword(NewUser.Password, NewUser.UserName);
                DB.Users.Add(NewUser);
                DB.SaveChanges();
            }
            if (DB.Users.Count(f => f.UserName == user_name2) == 0)
            {
                User NewUser2 = new Model.User() { UserName = user_name2, Password = "7876543" };
                NewUser2.Password = Utility.SecurityUtity.HashPassword(NewUser2.Password, NewUser2.UserName);
                DB.Users.Add(NewUser2);
                DB.SaveChanges();
            }
            if (DB.TimeCards.Count(f => f.UserName == user_name && f.WeekStart == current_week) == 0)
            {
                DB.TimeCards.Add(NewOneWeekData);
                DB.SaveChanges();
            }
            TimeCardServices.Repository.Repository<TimeCard> repository = new TimeCardServices.Repository.Repository<TimeCard>(DB);
            TimeCardService timeCardService = new TimeCardService(repository);
            controller = new TimeCardController(timeCardService);
        }
        [Fact]
        public void GetOneWeekData()
        {
            int respected = 200;
            string userName = user_name;
           // Claim[] claims = new[]
           //{
           //    new Claim(ClaimTypes.Name, userName)
           // };
           // ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
           // controller.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(claimsIdentity);
            ObjectResult result = controller.Get(current_week,userName);
            TimeCardViewModel tcViewModel = (TimeCardViewModel)result.Value;
            Assert.Equal(respected, result.StatusCode);
            Assert.Equal(userName, tcViewModel.UserName);
            Assert.Equal(current_week, tcViewModel.WeekStart);
            Assert.Equal(NewOneWeekData.Day1, tcViewModel.Day1);
            Assert.Equal(NewOneWeekData.Day2, tcViewModel.Day2);
            Assert.Equal(NewOneWeekData.Day3, tcViewModel.Day3);
            Assert.Equal(NewOneWeekData.Day4, tcViewModel.Day4);
            Assert.Equal(NewOneWeekData.Day5, tcViewModel.Day5);
            Assert.Equal(NewOneWeekData.Day6, tcViewModel.Day6);
            Assert.Equal(NewOneWeekData.Day7, tcViewModel.Day7);
            Assert.Equal(NewOneWeekData.Notes, tcViewModel.Notes);
            Assert.Equal(NewOneWeekData.CreateUser, tcViewModel.CreateUser);
            Assert.Equal(NewOneWeekData.UpdateUser, tcViewModel.UpdateUser);
            Assert.True(Math.Abs((NewOneWeekData.CreateDate- tcViewModel.CreateDate).Value.Seconds)<1);
            Assert.True(Math.Abs((NewOneWeekData.UpdateDate - tcViewModel.UpdateDate).Value.Seconds) < 1); 
            Assert.Equal(40, tcViewModel.TotalHors);
        }
        [Fact]
        public void AddOneWeekDataSuccess()
        {
            int respected = 200;
            //Claim[] claims = new[]
            // {
            //   new Claim(ClaimTypes.Name, user_name)
            // };
            //controller.HttpContext.User.   AddIdentity(new ClaimsIdentity(claims));
                //= new System.Security.Claims.ClaimsPrincipal();
            TimeCardViewModel OneWeekData = new TimeCardViewModel()
            {
                UserName = user_name,
                WeekStart = new DateTime(2021,07,26),
                Day1 = 8,
                Day2 = 8,
                Day3 = 8,
                Day4 = 8,
                Day5 = 5.5m,
                Day6 = null,
                Day7 = null,
                Notes = "Remarks",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateUser = user_name,
                UpdateUser = user_name
            };
            ObjectResult result = controller.AddOneWeekData(OneWeekData);
            Assert.Equal(respected, result.StatusCode);
            Assert.Equal(1, (int)result.Value);
           TimeCard deleteItem=  DB.TimeCards.Find(user_name, new DateTime(2021, 07, 26));
            DB.TimeCards.Remove(deleteItem);
            DB.SaveChanges();
        }
        [Fact]
        public void AddOneWeekDataFaildWhenExists()
        {
            int respected = 404;
            TimeCardViewModel OneWeekData = new TimeCardViewModel()
            {
                UserName = user_name,
                WeekStart = current_week,
                Day1 = 8,
                Day2 = 8,
                Day3 = 8,
                Day4 = 8,
                Day5 = 5.5m,
                Day6 = null,
                Day7 = null,
                Notes = "Remarks",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateUser = user_name,
                UpdateUser = user_name
            };
            ObjectResult result = controller.AddOneWeekData(OneWeekData);
            Assert.Equal(respected, result.StatusCode);
            Assert.Contains("has existed", ((List<MessageViewModel>)result.Value).FirstOrDefault().Message);

        }
        [Fact]
        public void DeleteOneWeekDataSuccess()
        {
            int respected = 200;
            ObjectResult result = controller.DeleteByUserName(current_week, user_name);
            Assert.Equal(respected, result.StatusCode);
            Assert.Equal(1, (int)result.Value);
            DB.TimeCards.Add(NewOneWeekData);
            DB.SaveChanges();
        }
        [Fact]
        public void DeleteOneWeekDataExists()
        {
            int respected = 404;
      
            ObjectResult result = controller.DeleteByUserName(current_week.AddDays(-1),user_name);
            Assert.Equal(respected, result.StatusCode);
            Assert.Contains("not exists", ((List<MessageViewModel>)result.Value).FirstOrDefault().Message);
        }

    }
}
