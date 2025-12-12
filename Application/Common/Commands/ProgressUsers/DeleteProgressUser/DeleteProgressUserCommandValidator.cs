using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressUsers.DeleteProgressUser
{
    public class DeleteProgressUserCommandValidator : AbstractValidator<DeleteProgressUserCommand>
    {
        public DeleteProgressUserCommandValidator() 
        {
            RuleFor(deleteProgressUserCommand => deleteProgressUserCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(deleteProgressUserCommand => deleteProgressUserCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
