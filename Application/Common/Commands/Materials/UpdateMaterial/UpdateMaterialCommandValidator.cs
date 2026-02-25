using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Materials.UpdateMaterial
{
    public class UpdateMaterialCommandValidator : AbstractValidator<UpdateMaterialCommand>
    {
        public UpdateMaterialCommandValidator()
        {
            RuleFor(updateMatherialCommand => updateMatherialCommand.Id)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateMatherialCommand => updateMatherialCommand.CurrentUserId)
              .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateMatherialCommand => updateMatherialCommand.Title)
                .NotEmpty().NotNull().MaximumLength(100);
        }
    }
}
