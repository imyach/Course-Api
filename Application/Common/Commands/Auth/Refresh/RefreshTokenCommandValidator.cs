using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.Refresh
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator() 
        {
            RuleFor(refreshTokenCommand => refreshTokenCommand.RefreshToken)
                .NotEmpty().NotNull();
            RuleFor(refreshTokenCommand => refreshTokenCommand.CurrentUserId)
                .NotEmpty().NotNull();
        }
    }
}
