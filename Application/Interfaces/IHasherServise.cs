using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IHasherServise
    {
        string Hash(string text);
        bool VerifyBcrypt(string text, string hash);
    }
}
