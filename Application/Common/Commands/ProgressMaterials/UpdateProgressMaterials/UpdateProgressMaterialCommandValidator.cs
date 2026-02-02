using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials
{
    public class UpdateProgressMaterialCommandValidator : AbstractValidator<UpdateProgressMaterialCommand>
    {
        public UpdateProgressMaterialCommandValidator()
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
