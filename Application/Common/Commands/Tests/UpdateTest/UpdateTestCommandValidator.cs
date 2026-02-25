using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Tests.UpdateTest
{
    public class UpdateTestCommandValidator : AbstractValidator<UpdateTestCommand>
    {
        public UpdateTestCommandValidator() 
        {
            RuleFor(updateTestCommand => updateTestCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateTestCommand => updateTestCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateTestCommand => updateTestCommand.Title)
               .NotNull().NotEmpty().MaximumLength(100);

        }
    }
}
