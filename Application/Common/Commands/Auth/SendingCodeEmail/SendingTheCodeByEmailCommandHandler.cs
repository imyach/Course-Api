using Application.Common.Dtos.Auth;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.SendingCodeEmail
{
    public class SendingTheCodeByEmailCommandHandler(ICoursesDbContext context, IEmailServise emailServise, IHasherServise hasherServise, IGenerateRandomValueService randomValueService) : IRequestHandler<SendingTheCodeByEmailCommand>
    {
        public async Task<Unit> Handle(SendingTheCodeByEmailCommand request, CancellationToken cancellationToken)
        {
            var oldCode = await context.RecoveryCode.Where(x => x.UserId == request.UserId).ToListAsync(cancellationToken);
            context.RecoveryCode.RemoveRange(oldCode);
            await context.SaveChangesAsync(cancellationToken);

            var newCode = randomValueService.GenerateRecoveryCode();

            await emailServise.SendMessage($@"<h2>Подтверждение восстановления пароля</h2>
                <p>Ваш код подтверждения: <strong style='font-size: 24px;'>{newCode}</strong></p>
                <p>Код действителен в течение <strong>5 минут</strong>.</p>
                <p><strong>Важно:</strong> код можно использовать только один раз.</p>
                <hr/>
                <p>Если вы не запрашивали восстановление пароля, проигнорируйте это письмо.</p>",
                "Код подтверждения — SkillForge",
                request.UserEmail);

            var recPas = new RecoveryCode
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ExpirationTime = DateTime.UtcNow + TimeSpan.FromMinutes(5),
                IsUsedEarlier = false,
                CodeHash = hasherServise.Hash(newCode)
            };

            await context.RecoveryCode.AddAsync(recPas, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
