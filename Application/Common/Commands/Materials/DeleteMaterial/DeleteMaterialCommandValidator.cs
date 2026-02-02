using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Materials.DeleteMaterial
{
    public class DeleteMaterialCommandValidator : AbstractValidator<DeleteMaterialCommand>
    {
        public DeleteMaterialCommandValidator()
        {
            RuleFor(deleteMatherialCommand => deleteMatherialCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(deleteMatherialCommand => deleteMatherialCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
