using Application.Common.Dtos.Auth;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Application.Common.Commands.Users.UpdateUser
{
    public class UpdateUserCommandHandler(ICoursesDbContext context, IJwtTokenServise tokenServise, IPasswordHasherServise passwordHasher, IEmailServise emailServise) : IRequestHandler<UpdateUserCommand, TokensDto?>
    {
        public async Task<TokensDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var entity = await context.Users.FindAsync([request.Id], cancellationToken) 
                ?? throw new NotFoundException(nameof(User), request.Id);

            string emailMessage = $"Здравствуйте, {entity.NameUser}. Сообщаем что данные от вашего аккаунта изменены.\n";

            if (currentUser.Id == entity.Id)
            {

                var dublicate = await context.Users.AnyAsync(x => (x.Email == entity.Email && x.Login == entity.Login)&&x.Id != entity.Id, cancellationToken);

                if (dublicate)
                    return null;

                if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.OldPassword))
                {
                    if (passwordHasher.VerifyBcryptPassword(request.OldPassword, entity.HashPassword))
                    {
                        entity.HashPassword = passwordHasher.HashPasword(request.NewPassword);
                        emailMessage = emailMessage + $"Был изменен пароль от аккаунта\n";
                        await emailServise.SendMessage(emailMessage, "Обновлены данные аккаунта", entity.Email);
                        await context.SaveChangesAsync(cancellationToken);
                        return await tokenServise.GenerateTokens(entity);
                    }
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
                    entity.PhoneNumber = request.PhoneNumber;

                await context.SaveChangesAsync(cancellationToken);

                emailMessage = emailMessage + "\nОбновленные данные: " +
                $"\nРоль: {request.Role.Name}" +
                $"\nИмя пользователя: {entity.NameUser}" +
                $"\nЛогин: {entity.Login}" +
                $"\nПочта: {entity.Email}" +
                $"\nТелефон: {entity.PhoneNumber}";

                await emailServise.SendMessage(emailMessage, "Обновлены данные аккаунта", entity.Email);

                return await tokenServise.GenerateTokens(entity);

            }
            throw new AccessException();
        }
    }
}
