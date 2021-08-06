using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TimeCardServices.Model;

namespace TimeCardServices.Utility
{
    public class SecurityUtity
    {

       public const string _issuer= "Matt";
        public const string _audience = "Matt";
        public const string _securityKey = "12344557891234567";
        //PBKDF2
        public static string HashPassword(string value, string salt)
        {
            var result = KeyDerivation.Pbkdf2(
                password: value,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 9812,
                numBytesRequested: 32);

            return Convert.ToBase64String(result);
        }

        public static string GetToken(User user,string role)
        {
            Claim[] claims = new[]
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim("Email",user.Email??""),
               new Claim("Role",role??""),//传递其他信息  
               new Claim("Address",user.Address??"")
              
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),//5分钟有效期
                signingCredentials: creds);
            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }


        //public static string HashPassword(string password)
        //{
        //    var salt = GenerateSalt();
        //    var hash = HashPassword(password, salt);
        //    var result = $"{salt}.{hash}";
        //    return result;
        //}

        //private static bool Validate(string password, string salt, string hash)
        //    => HashPassword(password, salt) == hash;

        //public static bool VerifyHashedPassword(string password, string storePassword)
        //{
        //    if (string.IsNullOrEmpty(password))
        //    {
        //        throw new ArgumentNullException(nameof(password));
        //    }

        //    if (string.IsNullOrEmpty(storePassword))
        //    {
        //        throw new ArgumentNullException(nameof(storePassword));
        //    }

        //    var parts = storePassword.Split('.');
        //    var salt = parts[0];
        //    var hash = parts[1];

        //    return Validate(password, salt, hash); ;
        //}

        //private static string GenerateSalt()
        //{
        //    byte[] randomBytes = new byte[128 / 8];
        //    using (var generator = RandomNumberGenerator.Create())
        //    {
        //        generator.GetBytes(randomBytes);
        //        return Convert.ToBase64String(randomBytes);
        //    }
        //}

    }
}
