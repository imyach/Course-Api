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
    public class LoginUserCommandHandler(ICoursesDbContext context, IJwtTokenServise JwtTokenServise, IPasswordHasherServise passwordHasher) : IRequestHandler<LoginUserCommand,string>
    {
        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {

            var users = await context.Users.Where(user=> 
                user.Login == request.Login || user.Email == request.Login).ToListAsync(cancellationToken);
            if (users == null)
            {
                return string.Empty;
            }
            var user = users.FirstOrDefault(user => passwordHasher.VerifyBcryptPassword(request.Password, user.HashPassword));
            if (user == null)
            {
                return string.Empty;
            }
            var token = JwtTokenServise.GenerateJwtToken(user);

            return await token;
        }
    }
}
