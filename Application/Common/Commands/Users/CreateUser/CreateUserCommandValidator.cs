using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(createUserCommand => createUserCommand.RoleId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createUserCommand => createUserCommand.NameUser)
               .NotNull().NotEmpty().MaximumLength(30);
            RuleFor(createUserCommand => createUserCommand.Login)
              .NotNull().NotEmpty().MaximumLength(30);
            RuleFor(createUserCommand => createUserCommand.Email)
              .MaximumLength(50);
            RuleFor(createUserCommand => createUserCommand.PhoneNumber)
              .MaximumLength(12);
            RuleFor(createUserCommand => createUserCommand.Password)
              .NotNull().NotEmpty().MaximumLength(60);
        }
    }
}
