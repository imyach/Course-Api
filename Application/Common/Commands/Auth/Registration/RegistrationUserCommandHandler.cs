using Application.Common.Commands.Users.CreateUser;
using Application.Common.Dtos.Auth;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.Registration
{
    public class RegistrationUserCommandHandler(ICoursesDbContext context, IJwtTokenServise tokenServise, IPasswordHasherServise passwordHasher) : IRequestHandler<RegistrationUserCommand, TokensDto?>
    {
        public async Task<TokensDto?> Handle(RegistrationUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                RoleId = request.RoleId,
                NameUser = request.NameUser,
                Login = request.Login,
                Email = request.Email,
                HashPassword = passwordHasher.HashPasword(request.Password),
                CreatedAt = DateTime.UtcNow,
                PhoneNumber = request.PhoneNumber
            };

            var dulicate = await context.Users.FirstOrDefaultAsync(x=> (x.Login == user.Login && x.HashPassword == user.HashPassword)
                || (x.Email == user.Email && passwordHasher.VerifyBcryptPassword(request.Password, x.HashPassword)), cancellationToken);

            if (dulicate is null)
            {
                await context.Users.AddAsync(user, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return await tokenServise.GenerateTokens(user); 
            }
            return null;
        }
    }
}
