using Application.Common.Dtos.Auth;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.UpdateUser
{
    public class UpdateUserForAdminCommandHandler(ICoursesDbContext context, IJwtTokenServise tokenServise, IPasswordHasherServise passwordHasher) : IRequestHandler<UpdateUserCommand, TokensDto?>
    {
        public async Task<TokensDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);
            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Users.FindAsync([request.Id], cancellationToken) ?? throw new NotFoundException(nameof(User), currentUser.Id);

            if (!passwordHasher.VerifyBcryptPassword(request.Password, entity.HashPassword))
                await tokenServise.DeleteResreshToken(currentUser, cancellationToken);

            if (currentUser.Id == entity.Id)
            {
                var dublicate = await context.Users.AnyAsync(x => x.Email == currentUser.Email || x.Login == currentUser.Login, cancellationToken);

                if (dublicate)
                    return null;

                entity.RoleId = request.RoleId;
                entity.NameUser = request.NameUser;
                entity.Login = request.Login;
                entity.Email = request.Email;
                entity.HashPassword = passwordHasher.HashPasword(request.Password);
                entity.PhoneNumber = request.PhoneNumber;
                await context.SaveChangesAsync(cancellationToken);

                return await tokenServise.GenerateTokens(entity);
            }
            throw new AccessException();
        }
    }
}
