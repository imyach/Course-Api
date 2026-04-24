using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Application.Common.Commands.Users.DeteleUser
{
    public class DeleteUserCommandHandler(ICoursesDbContext context, IEmailServise emailServise) : IRequestHandler<DeleteUserCommand>
    {
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);
            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Users
                .Include(u => u.Role) 
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.Id);

            if (roleUser.Name == "Admin" || currentUser.Id == entity.Id)
            {
                if (entity.Role.Name == "Student") 
                    context.Users.Remove(entity);
                else
                    entity.IsActive = false;
                   
                if(roleUser.Name == "Admin" && currentUser.Id != entity.Id)
                    await emailServise.SendMessage($"Здравствуйте, {entity.NameUser}.\n\n" +
                    $"Сообщаем, что ваш аккаунт был удален в связи с нарушением правил сообщества.\n\n" +
                    $"Если вы считаете это ошибкой, свяжитесь с поддержкой: support@skillforge.com",
                    "Ваш аккаунт удален — SkillForge",
                    entity.Email);
                else
                   await emailServise.SendMessage($"Здравствуйте, {entity.NameUser}.\n\n" +
                    $"Мы сожалеем, что вы решили покинуть SkillForge.\n\n" +
                    $"Если вы передумаете, мы всегда будем рады видеть вас снова!\n\n" +
                    $"Ваши данные будут удалены из системы в течение 30 дней.",
                    "Ваш аккаунт удален — SkillForge",
                    entity.Email);

                await context.SaveChangesAsync(cancellationToken); 
                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}

