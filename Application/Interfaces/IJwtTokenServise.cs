using Application.Common.Dtos.Auth;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IJwtTokenServise
    {
        public Task<TokensDto> GenerateTokens(User user);
        public  Task DeleteResreshToken(User user, CancellationToken cancellationToken);
    }
}
