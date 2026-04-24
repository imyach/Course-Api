using Application.Common.Dtos.Auth;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Common.Commands.Auth.Login
{
    public class LoginUserCommandHandler(ICoursesDbContext context, IJwtTokenServise tokenServise, IHasherServise passwordHasher) : IRequestHandler<LoginUserCommand, TokensDto?>
    {
        public async Task<TokensDto?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var users = await context.Users.Where(user=> 
                user.Login == request.Login || user.Email == request.Login ).ToListAsync(cancellationToken);
            if (users == null)
                return null;

            var user = users.FirstOrDefault(user => passwordHasher.VerifyBcrypt(request.Password, user.HashPassword) && user.IsActive == true);
            if (user == null)
                return null;



            return await tokenServise.GenerateTokens(user);
        }
    }
}
