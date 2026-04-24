using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IGenerateRandomValueService
    {
        public string GenerateRecoveryCode();
        public string GenerateNewPassword();
    }
}
