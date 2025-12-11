using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWebApi.Servises
{
    public class PasswordHasherServise : IPasswordHasherServise
    {
        public  string HashPasword(string password) => BCrypt.Net.BCrypt.HashPassword(password, 8);
        public bool VerifyBcryptPassword(string password, string hash)=> BCrypt.Net.BCrypt.Verify(password, hash);
        
    }
}
