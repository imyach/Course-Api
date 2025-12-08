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
    public class LoginUserCommandHandler(ICoursesDbContext context, IJwtTokenServise servise) : IRequestHandler<LoginUserCommand,string>
    {
        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {

            var user = await context.Users.FirstOrDefaultAsync(user=> 
                (user.Login == request.Login || user.Email == request.Login)
                && user.HashPassword == request.Password,cancellationToken);
            if (user == null)
            {
                return string.Empty;
            }
            var token =  servise.GenerateJwtToken(user);

            return await token;
        }
    }
}
