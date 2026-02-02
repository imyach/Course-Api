using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressMaterials.CreateProgressMaterials
{
    public class CreateProgressMaterialCommandValidator : AbstractValidator<CreateProgressMaterialCommand>
    {
        public CreateProgressMaterialCommandValidator()
        {
            RuleFor(createProgressUserCommand => createProgressUserCommand.ProgressModuleId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createProgressUserCommand => createProgressUserCommand.MaterialId)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(createProgressUserCommand => createProgressUserCommand.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
