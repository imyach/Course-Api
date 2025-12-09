using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IPasswordHasherServise
    {
        string HashPasword(string password);
        bool VerifyBcryptPassword(string password, string hash);
    }
}
