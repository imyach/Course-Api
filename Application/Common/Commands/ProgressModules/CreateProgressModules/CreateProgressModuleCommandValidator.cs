using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressModules.CreateProgressModules
{
    public class CreateProgressModuleCommandValidator : AbstractValidator<CreateProgressModuleCommand>
    {
        public CreateProgressModuleCommandValidator()
        {
            RuleFor(createProgressUserCommand => createProgressUserCommand.ProgressUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createProgressUserCommand => createProgressUserCommand.ModuleId)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(createProgressUserCommand => createProgressUserCommand.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
