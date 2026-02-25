using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Materials.CreateMaterial
{
    public class CreateMaterialCommandValidator : AbstractValidator<CreateMaterialCommand>
    {
        public CreateMaterialCommandValidator() 
        {
            RuleFor(createMatherialCommand => createMatherialCommand.ModuleId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createMatherialCommand => createMatherialCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createMatherialCommand => createMatherialCommand.Title)
                .NotEmpty().NotNull().MaximumLength(100);
        }
    }
}
