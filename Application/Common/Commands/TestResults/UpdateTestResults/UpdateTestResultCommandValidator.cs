using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.TestResults.UpdateTestResults
{
    public class UpdateTestResultCommandValidator : AbstractValidator<UpdateTestResultCommand>
    {
        public UpdateTestResultCommandValidator() 
        {
            RuleFor(updateProgressUserCommand => updateProgressUserCommand.Id)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateProgressUserCommand => updateProgressUserCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
