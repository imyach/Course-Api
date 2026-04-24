using Domain.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.SendingCodeEmail
{
    public class SendingTheCodeByEmailCommandValidator : AbstractValidator<SendingTheCodeByEmailCommand>
    {
        public SendingTheCodeByEmailCommandValidator()
        {
            RuleFor(recoveryCode => recoveryCode.UserEmail)
               .NotNull().NotEmpty();
            RuleFor(recoveryCode => recoveryCode.UserId)
               .NotEqual(Guid.Empty);
        }
    }
}
