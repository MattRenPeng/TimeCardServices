using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeCardServices.Controllers;
using TimeCardServices.Domain;
using TimeCardServices.Model;
using TimeCardServices.Repository;
using TimeCardServices.Services;
using Xunit;

namespace TimeCardServices.Test
{
   public class UserControllerTest : IDisposable
    {
        TimeCardDBContext DB;
        public void Dispose()
        {
            DB?.Dispose();
        }
        private UserController controller;
        private static String user_name = "zhangsan";
        private static String user_name2 = "lisi";
        public UserControllerTest()
        {
            //var builder = new DbContextOptionsBuilder<TimeCardDBContext>()
            //   .UseSqlServer("");

            var builder = new DbContextOptionsBuilder<TimeCardDBContext>()
              .UseInMemoryDatabase("TimeCardDB2");

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

            TimeCardServices.Repository.Repository<User> repository = new TimeCardServices.Repository.Repository<User>(DB);
            UserService userService = new UserService(repository);
            controller = new UserController(userService);
        }
        [Fact]
        public void GetAllUserSuccess()
        {
            int respected_UserCount = 2;
            var result = controller.GetAll();
            Assert.Equal(respected_UserCount, ((List<UserInfoViewModel>)(result.Value)).Count);
        }
        [Fact]
        public void DeleteUserExists()
        {
            int count = 1;
            int result = (int)(controller.Delete("zhangsan").Value);
            Assert.Equal(count, result);
        }
        [Fact]
        public void DeleteUserNotExists()
        {
            int expected = 404;
            int result = (int)(controller.Delete("liming").StatusCode);
            Assert.Equal(expected, result);
        }
    }
}
