using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.DeteleUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator() 
        {
            RuleFor(deleteUserCommand => deleteUserCommand.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
