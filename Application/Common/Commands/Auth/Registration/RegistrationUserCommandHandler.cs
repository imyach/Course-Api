using Application.Common.Commands.Users.CreateUser;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.Registration
{
    public class RegistrationUserCommandHandler(ICoursesDbContext context, IJwtTokenServise JwtTokenServise, IPasswordHasherServise passwordHasher) : IRequestHandler<RegistrationUserCommand, string>
    {
        public async Task<string> Handle(RegistrationUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                RoleId = request.RoleId,
                NameUser = request.NameUser,
                Login = request.Login,
                Email = request.Email,
                HashPassword = passwordHasher.HashPasword(request.Password),
                CreatedAt = DateTime.Now,
                PhoneNumber = request.PhoneNumber
            };

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);



            return await JwtTokenServise.GenerateJwtToken(user);
        }
    }
}
