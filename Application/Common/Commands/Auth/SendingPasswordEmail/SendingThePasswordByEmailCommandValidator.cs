using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.SendingPasswordEmail
{
    public class SendingThePasswordByEmailCommandValidator : AbstractValidator<SendingThePasswordByEmailCommand>
    {
        public SendingThePasswordByEmailCommandValidator()
        {
            RuleFor(sendingThePasswordByEmailCommand => sendingThePasswordByEmailCommand.Code)
                .NotEmpty().NotNull();
            RuleFor(sendingThePasswordByEmailCommand => sendingThePasswordByEmailCommand.UserId)
               .NotEqual(Guid.Empty);
        }
    }
}
