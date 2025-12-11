using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.DeleteModule
{
    public class DeleteModuleCommandValidator : AbstractValidator<DeleteModuleCommand>
    {
        public DeleteModuleCommandValidator()
        {
            RuleFor(deleteModuleCommand => deleteModuleCommand.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(deleteModuleCommand => deleteModuleCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
