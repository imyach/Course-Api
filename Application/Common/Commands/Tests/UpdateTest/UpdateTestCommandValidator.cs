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
            RuleFor(updateTestCommand => updateTestCommand.Title)
               .NotNull().NotEmpty().MaximumLength(100);
            RuleFor(updateTestCommand => updateTestCommand.Description)
                .MaximumLength(1000);

        }
    }
}
