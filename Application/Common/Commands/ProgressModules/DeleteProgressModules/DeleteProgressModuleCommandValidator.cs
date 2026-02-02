using Application.Common.Commands.ProgressUsers.DeleteProgressUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressModules.DeleteProgressModules
{
    public class DeleteProgressModuleCommandValidator : AbstractValidator<DeleteProgressModuleCommand>
    {
        public DeleteProgressModuleCommandValidator()
        {
            RuleFor(deleteProgressUserCommand => deleteProgressUserCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(deleteProgressUserCommand => deleteProgressUserCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
