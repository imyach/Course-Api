using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.Login
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator() 
        {
            RuleFor(loginUserCommand => loginUserCommand.Login)
                .NotNull().NotEqual(string.Empty);
            RuleFor(loginUserCommand => loginUserCommand.Password)
                .NotNull().NotEqual(string.Empty);
        }
    }
}
