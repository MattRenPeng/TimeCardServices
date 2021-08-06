using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TimeCardServices.Model;

namespace TimeCardServices.Domain
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "User name is required")]
        [StringLength(60, ErrorMessage = "User name is too long!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(200, ErrorMessage = "Password is too long!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "User type is only for admin or staff!")]
        [EnumDataType(typeof(UserType), ErrorMessage = "User type is only for admin or staff!")]
        public UserType UserType { get; set; }
    }
}
