using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressUsers.UpdateProgressUser
{
    public class UpdateProgressUserCommandValidator : AbstractValidator<UpdateProgressUserCommand>
    {
        public UpdateProgressUserCommandValidator() 
        {
            RuleFor(updateProgressUserCommand => updateProgressUserCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateProgressUserCommand => updateProgressUserCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateProgressUserCommand => updateProgressUserCommand.Status)
                .NotNull().MaximumLength(30);
        }
    }
}
