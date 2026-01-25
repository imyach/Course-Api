using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Tests.CreateTest
{
    public class CreateTestCommandValidator : AbstractValidator<CreateTestCommand>
    {
        public CreateTestCommandValidator()
        {
            RuleFor(createTestCommand => createTestCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createTestCommand => createTestCommand.MatherialId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createTestCommand => createTestCommand.Title)
               .NotNull().NotEmpty().MaximumLength(100);
            RuleFor(createTestCommand => createTestCommand.Description)
                .MaximumLength(1000);
        }
    }
}
