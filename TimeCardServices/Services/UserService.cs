using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCardServices.Domain;
using TimeCardServices.Model;
using TimeCardServices.Repository;

namespace TimeCardServices.Services
{
    public class UserService: IUserService
    {
        //IRepository<User> Repository = new TimeCardServices.Repository.Repository<User>();
        IRepository<User> Repository;
        public UserService(IRepository<User> repository)
        {
            Repository = repository;
        }

        public User CheckLogin(UserViewModel userViewModel)
        {
            if (userViewModel == null || userViewModel.UserName == null || userViewModel.Password == null)
            {
                return null;
            }
            else 
            {
               User result = Repository.SearchFor(f => f.UserName == userViewModel.UserName.Trim() && f.Password == Utility.SecurityUtity.HashPassword(userViewModel.Password, userViewModel.UserName)).FirstOrDefault();
                return result;
            }
        }

        public int AddUser(UserForSignUpViewModel UserForSignUpViewModel,string create_user)
        {
            User user = new User();
            // AutoMapper.Mapper.Map<UserForSignUpViewModel, User>(UserForSignUpViewModel, user);
            user.Address = UserForSignUpViewModel.Address;
            user.Email = UserForSignUpViewModel.Email;
            user.Password = UserForSignUpViewModel.Password;
            user.UserName = UserForSignUpViewModel.UserName;
            user.CreateDate = DateTime.Now;
            user.UpdateDate = DateTime.Now;
            user.CreateUser= create_user;
            user.UpdateUser = create_user;
            user.Password= Utility.SecurityUtity.HashPassword(user.Password, user.UserName);
            return Repository.Insert(user);
        }
        public List<User> GetAllUserInfo()
        {
           return Repository.GetAll().ToList();
        }
        public int DeleteOneUser(string userName)
        {
            User userDeleting = Repository.GetById(userName);
            if (userDeleting != null)
            {
                return Repository.Delete(userDeleting);
            }
            else
                return 0;
        }
        public User GetUserByUsername(string user_name)
        {
            return Repository.GetById(user_name);
        }
    }
}
