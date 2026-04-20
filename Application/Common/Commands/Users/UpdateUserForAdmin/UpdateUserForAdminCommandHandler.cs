using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.UpdateUserForAdmin
{
    public class UpdateUserForAdminCommandHandler(ICoursesDbContext context, IPasswordHasherServise passwordHasher, IEmailServise emailServise) : IRequestHandler<UpdateUserForAdminCommand, bool>
    {
        public async Task<bool> Handle(UpdateUserForAdminCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(User), request.Id);

            var dublicate = await context.Users.AnyAsync(x => (x.Email == request.Email && x.Login == request.Login) && x.Id != request.Id, cancellationToken);

            if (dublicate)
                return false;

            string emailMessage = $"Здравствуйте, {entity.NameUser}. Сообщаем что данные от вашего аккаунта изменены.";


            if (request.Role != null)
             entity.RoleId = request.Role.Id;
            if (!string.IsNullOrEmpty(request.NameUser))
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

            await emailServise.SendMessage(emailMessage, "Администратор изменил данные вашего аккаунта", entity.Email);

            return true;
        }
    }
}
