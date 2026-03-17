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
    public class UpdateUserCommandHandler(ICoursesDbContext context, IJwtTokenServise tokenServise, IPasswordHasherServise passwordHasher) : IRequestHandler<UpdateUserCommand, TokensDto?>
    {
        public async Task<TokensDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var entity = await context.Users.FindAsync([request.Id], cancellationToken) 
                ?? throw new NotFoundException(nameof(User), request.Id);


            if (currentUser.Id == entity.Id)
            {

                var dublicate = await context.Users.AnyAsync(x => (x.Email == entity.Email && x.Login == entity.Login)&&x.Id != entity.Id, cancellationToken);

                if (dublicate)
                    return null;

                if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.OldPassword))
                {
                    if (passwordHasher.VerifyBcryptPassword(request.OldPassword, entity.HashPassword))
                        entity.HashPassword = passwordHasher.HashPasword(request.NewPassword);
                    else return null;
                }
                if(request.Role != null)
                    entity.RoleId = request.Role.Id;
                if(!string.IsNullOrEmpty(request.NameUser))
                    entity.NameUser = request.NameUser;
                if (!string.IsNullOrEmpty(request.Login))
                    entity.Login = request.Login;
                if (!string.IsNullOrEmpty(request.Email))
                    entity.Email = request.Email;
                if (!string.IsNullOrEmpty(request.PhoneNumber))
                    entity.PhoneNumber = request.PhoneNumber;

                await context.SaveChangesAsync(cancellationToken);


                return await tokenServise.GenerateTokens(entity);
            }
            throw new AccessException();
        }
    }
}
