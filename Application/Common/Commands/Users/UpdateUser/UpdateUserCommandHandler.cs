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

            var entity = await context.Users.FindAsync([request.Id], cancellationToken) 
                ?? throw new NotFoundException(nameof(User), currentUser.Id);


            if (currentUser.Id == entity.Id)
            {

                var dublicate = await context.Users.AnyAsync(x => (x.Email == entity.Email && x.Login == entity.Login)&&x.Id != entity.Id, cancellationToken);

                if (dublicate)
                    return null;

                entity.RoleId = request.Role.Id;
                entity.NameUser = request.NameUser;
                entity.Login = request.Login;
                entity.Email = request.Email;
                if(!string.IsNullOrEmpty(request.Password))
                    entity.HashPassword = passwordHasher.HashPasword(request.Password);
                entity.PhoneNumber = request.PhoneNumber;
                await context.SaveChangesAsync(cancellationToken);

                return await tokenServise.GenerateTokens(entity);
            }
            throw new AccessException();
        }
    }
}
