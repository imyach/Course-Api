using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.UpdateUser
{
    public class UpdateUserForAdminCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserForAdminCommandValidator()
        {
            RuleFor(updateUserCommand => updateUserCommand.Id)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateUserCommand => updateUserCommand.CurrentUserId)
              .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateUserCommand => updateUserCommand.Role)
               .NotNull();
            RuleFor(updateUserCommand => updateUserCommand.NameUser)
               .NotNull().NotEmpty().MaximumLength(30);
            RuleFor(updateUserCommand => updateUserCommand.OldPassword)
               .MaximumLength(30);
            RuleFor(updateUserCommand => updateUserCommand.NewPassword)
               .MaximumLength(30);
            RuleFor(updateUserCommand => updateUserCommand.Login)
              .NotNull().NotEmpty().MaximumLength(30);
            RuleFor(updateUserCommand => updateUserCommand.Email)
              .MaximumLength(50);
            RuleFor(updateUserCommand => updateUserCommand.PhoneNumber)
              .MaximumLength(12);
        }
    }
}
