using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IJwtTokenServise
    {
        Task<string> GenerateJwtToken(User username);
    }
}
