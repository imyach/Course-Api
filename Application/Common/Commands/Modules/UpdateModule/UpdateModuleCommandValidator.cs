using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.UpdateModule
{
    public class UpdateModuleCommandValidator : AbstractValidator<UpdateModuleCommand>
    {
        public UpdateModuleCommandValidator()
        {
            RuleFor(updateModuleCommand => updateModuleCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateModuleCommand => updateModuleCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateModuleCommand => updateModuleCommand.Title)
                .NotEmpty().NotNull().MaximumLength(100);
            RuleFor(updateModuleCommand => updateModuleCommand.Description)
                .MaximumLength(1000);
        }
    }
}
