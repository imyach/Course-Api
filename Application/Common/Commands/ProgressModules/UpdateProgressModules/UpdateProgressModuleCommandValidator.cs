using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressModules.UpdateProgressModules
{
    public class UpdateProgressModuleCommandValidator : AbstractValidator<UpdateProgressModuleCommand>
    {
        public UpdateProgressModuleCommandValidator()
        {
            RuleFor(updateProgressUserCommand => updateProgressUserCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateProgressUserCommand => updateProgressUserCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateProgressUserCommand => updateProgressUserCommand.Status)
                .MaximumLength(30);
        }
    }
}
