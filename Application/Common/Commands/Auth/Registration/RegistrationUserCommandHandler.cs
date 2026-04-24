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
    public class RegistrationUserCommandHandler(ICoursesDbContext context, IJwtTokenServise tokenServise, IHasherServise passwordHasher) : IRequestHandler<RegistrationUserCommand, TokensDto?>
    {
        public async Task<TokensDto?> Handle(RegistrationUserCommand request, CancellationToken cancellationToken)
        {
            var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == nameof(EnumRoles.Student), cancellationToken);

            var user = new User
            {
                Id = Guid.NewGuid(),
                RoleId = role.Id,
                NameUser = request.NameUser,
                Login = request.Login,
                Email = request.Email,
                HashPassword = passwordHasher.Hash(request.Password),
                CreatedAt = DateTime.UtcNow,
                PhoneNumber = request.PhoneNumber,
                IsActive = true
            };

            var dublicate = await context.Users.AnyAsync(x => x.Email == user.Email && x.Login == user.Login, cancellationToken);

            if (dublicate)
                return null;

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return await tokenServise.GenerateTokens(user);
        }
    }
}
