using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.SendingPasswordEmail
{
    public class SendingThePasswordByEmailCommandHandler(ICoursesDbContext context, IEmailServise emailServise, IHasherServise hasherServise, IGenerateRandomValueService randomValueService) : IRequestHandler<SendingThePasswordByEmailCommand, bool>
    {
        public async Task<bool> Handle(SendingThePasswordByEmailCommand request, CancellationToken cancellationToken)
        {
            var recoveryCodeEntry = await context.RecoveryCode.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            if (!(recoveryCodeEntry != null
                && hasherServise.VerifyBcrypt(request.Code, recoveryCodeEntry.CodeHash)
                && recoveryCodeEntry.ExpirationTime > DateTime.UtcNow
                && recoveryCodeEntry.IsUsedEarlier == false)) 
            {
                return false;
            }

            recoveryCodeEntry.IsUsedEarlier = true;
            

            var newPassword = randomValueService.GenerateNewPassword();
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken) ?? throw new NotFoundException(nameof(User), request.UserId);

            await emailServise.SendMessage($@"
                <h2>Восстановление пароля SkillForge</h2>
                <p>Здравствуйте, {user.NameUser}!</p>
                <p>Ваш новый пароль для входа в систему:</p>
                <h3 style='font-family: monospace;'>{newPassword}</h3>
                <p><strong>Рекомендуем сменить этот пароль после входа в личный кабинет.</strong></p>
                <hr/>
                <p>Если вы не запрашивали сброс пароля, немедленно свяжитесь с поддержкой.</p>
                ",
                "Новый пароль для SkillForge",
                user.Email);

            user.HashPassword = hasherServise.Hash(newPassword);
                
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
