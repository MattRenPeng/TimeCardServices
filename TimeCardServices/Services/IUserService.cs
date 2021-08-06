using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCardServices.Domain;
using TimeCardServices.Model;

namespace TimeCardServices.Services
{
   public interface IUserService
    {
        public User CheckLogin(UserViewModel userViewModel);

        public int AddUser(UserForSignUpViewModel UserForSignUpViewModel);

        public User GetUserByUsername(string user_name);
    }
}
