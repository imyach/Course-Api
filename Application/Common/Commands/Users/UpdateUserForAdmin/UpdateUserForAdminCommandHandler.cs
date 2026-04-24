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
    public class UpdateUserForAdminCommandHandler(ICoursesDbContext context, IHasherServise passwordHasher, IEmailServise emailServise) : IRequestHandler<UpdateUserForAdminCommand, bool>
    {
        public async Task<bool> Handle(UpdateUserForAdminCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(User), request.Id);

            var dublicate = await context.Users.AnyAsync(x => (x.Email == request.Email && x.Login == request.Login) && x.Id != request.Id, cancellationToken);

            if (dublicate)
                return false;

            var emailMessage = new StringBuilder();
            emailMessage.AppendLine($"Здравствуйте, {entity.NameUser}.");
            emailMessage.AppendLine();
            emailMessage.AppendLine("Администратор изменил данные вашего аккаунта.");
            emailMessage.AppendLine();
            emailMessage.AppendLine("Новые данные:");

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


            emailMessage.AppendLine($"• Роль: {request.Role?.Name ?? "не изменена"}");
            emailMessage.AppendLine($"• Имя пользователя: {entity.NameUser}");
            emailMessage.AppendLine($"• Логин: {entity.Login}");
            emailMessage.AppendLine($"• Email: {entity.Email}");
            emailMessage.AppendLine($"• Телефон: {entity.PhoneNumber ?? "не указан"}");
            emailMessage.AppendLine();
            emailMessage.AppendLine("---");
            emailMessage.AppendLine("Если у вас возникли вопросы, свяжитесь с поддержкой: support@skillforge.com");

            await emailServise.SendMessage(emailMessage.ToString(), "Данные аккаунта изменены администратором — SkillForge", entity.Email);

            return true;
        }
    }
}
