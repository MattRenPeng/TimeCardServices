using System;
using TimeCardServices.Controllers;
using TimeCardServices.Repository;
using Xunit;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TimeCardServices.Model;
using TimeCardServices.Services;
using System.Collections.Generic;

namespace TimeCardServices.Test
{
    public class AccountControllerTest: IDisposable
    {
        TimeCardDBContext DB;
        public void Dispose()
        {
            DB?.Dispose();
        }
        private AccountController controller;
        private static String user_name = "zhangsan";
        
        public AccountControllerTest()
        {
            //var builder = new DbContextOptionsBuilder<TimeCardDBContext>()
            //   .UseSqlServer("");

            var builder = new DbContextOptionsBuilder<TimeCardDBContext>()
              .UseInMemoryDatabase("TimeCardDB");

            DB = new TimeCardDBContext(builder.Options);
            //init test data, if not exists then add zhangsan count
            if (DB.Users.Count(f => f.UserName == user_name) == 0)
            {
                User NewUser = new Model.User() { UserName = user_name, Password = "12345678" };
                NewUser.Password = Utility.SecurityUtity.HashPassword(NewUser.Password, NewUser.UserName);
                DB.Users.Add(NewUser);
             
                DB.SaveChanges();
            }
            TimeCardServices.Repository.Repository<User> repository = new TimeCardServices.Repository.Repository<User>(DB);
            UserService userService = new UserService(repository);
            controller = new AccountController(userService);
        }
        [Fact]
        public void LoginSuccessWithRightUserNameAndPassword()
        {
            int respected = 200;
            Domain.UserViewModel TestModel = new Domain.UserViewModel() { UserName = user_name, Password = "12345678"};
             TestModel.UserType = UserType.Admin;
            var result = controller.Login(TestModel);
            Assert.Equal(respected, result.StatusCode);
        }
        [Fact]
        public void LoginFaildWithWrongUserNameOrPassword()
        {
            int respected = 404;
            Domain.UserViewModel TestModel = new Domain.UserViewModel() { UserName = user_name, Password = "123" };
            TestModel.UserType = UserType.Admin;

            var result = controller.Login(TestModel);
            Assert.Equal(respected, result.StatusCode);
        
        }
        [Fact]
        public void SignUpSuccess()
        {
            int respected = 200;
            Domain.UserForSignUpViewModel TestModel = new Domain.UserForSignUpViewModel() { UserName = "TEST_ONE", Password = "123", Address="", Email="123@163.com" };
            var result = controller.signUp(TestModel);
            Assert.Equal(respected, result.StatusCode);
            User existsUser = DB.Users.Find(TestModel.UserName);
            DB.Users.Remove(existsUser);
            DB.SaveChanges();
        }
        [Fact]
        public void SignUpFaildWhenUserExists()
        {
          
            int respected = 404;
            Domain.UserForSignUpViewModel TestModel = new Domain.UserForSignUpViewModel() { UserName = user_name, Password = "123", Address = "", Email = "123@163.com" };
            var result = controller.signUp(TestModel);
            Assert.Equal(respected, result.StatusCode);
            User existsUser = DB.Users.Find(TestModel.UserName);
            if (existsUser != null)
            {
                DB.Users.Remove(existsUser);
                DB.SaveChanges();
            }
        }

    }
}
