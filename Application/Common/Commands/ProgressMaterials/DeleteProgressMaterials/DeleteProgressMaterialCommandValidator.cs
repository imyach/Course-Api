using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressMaterials.DeleteProgressMaterials
{
    public class DeleteProgressMaterialCommandValidator : AbstractValidator<DeleteProgressMaterialCommand>
    {
        public DeleteProgressMaterialCommandValidator()
        {
            RuleFor(deleteProgressUserCommand => deleteProgressUserCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(deleteProgressUserCommand => deleteProgressUserCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
