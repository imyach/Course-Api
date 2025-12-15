using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.Registration
{
    public class RegistrationUserValidator : AbstractValidator<RegistrationUserCommand>
    {
        public RegistrationUserValidator() 
        {
            RuleFor(registrationUserCommand => registrationUserCommand.RoleId)
    .NotNull().NotEqual(Guid.Empty);
            RuleFor(registrationUserCommand => registrationUserCommand.NameUser)
               .NotNull().NotEmpty().MaximumLength(30);
            RuleFor(registrationUserCommand => registrationUserCommand.Login)
              .NotNull().NotEmpty().MaximumLength(30);
            RuleFor(registrationUserCommand => registrationUserCommand.Email)
              .MaximumLength(50);
            RuleFor(registrationUserCommand => registrationUserCommand.PhoneNumber)
              .MaximumLength(12);
            RuleFor(registrationUserCommand => registrationUserCommand.Password)
              .NotNull().NotEmpty().MaximumLength(60);
        }
    }
}
