using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeCardServices.Domain
{
    public class UserForSignUpViewModel
    {
        [Required(ErrorMessage = "User name is required")]
        [StringLength(60, ErrorMessage = "User name is too long!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(200, ErrorMessage = "Password is too long!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(60, ErrorMessage = "Email is too long!")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please Enter Correct Email Address")]
        public string Email { get; set; }

        [StringLength(400, ErrorMessage = "Address is too long!")]
        public string Address { get; set; }
    }
}
