using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWebApi.Servises
{
    public class HasherServise : IHasherServise
    {
        public  string Hash(string text) => BCrypt.Net.BCrypt.HashPassword(text, 8);
        public bool VerifyBcrypt(string text, string hash)=> BCrypt.Net.BCrypt.Verify(text, hash);
        
    }
}
