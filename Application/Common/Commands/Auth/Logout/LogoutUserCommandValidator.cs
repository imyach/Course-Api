using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.Logout
{
    public class LogoutUserCommandValidator : AbstractValidator<LogoutUserCommand>
    {
        public LogoutUserCommandValidator()
        {
            RuleFor(refreshTokenCommand => refreshTokenCommand.CurrentUserId)
                .NotEmpty().NotNull();
        }
    }
}
