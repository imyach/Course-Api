using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Matherials.DeleteMatherial
{
    public class DeleteMatherialCommandValidator : AbstractValidator<DeleteMatherialCommand>
    {
        public DeleteMatherialCommandValidator()
        {
            RuleFor(deleteMatherialCommand => deleteMatherialCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
