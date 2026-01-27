using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Matherials.UpdateMatherial
{
    public class UpdateMatherialCommandValidator : AbstractValidator<UpdateMatherialCommand>
    {
        public UpdateMatherialCommandValidator()
        {
            RuleFor(updateMatherialCommand => updateMatherialCommand.Id)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateMatherialCommand => updateMatherialCommand.CurrentUserId)
              .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateMatherialCommand => updateMatherialCommand.Title)
                .NotEmpty().NotNull().MaximumLength(100);
            RuleFor(updateMatherialCommand => updateMatherialCommand.Description)
                .MaximumLength(1000);
        }
    }
}
