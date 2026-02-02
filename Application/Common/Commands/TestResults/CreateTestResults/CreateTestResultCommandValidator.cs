using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.TestResults.CreateTestResults
{
    public class CreateTestResultCommandValidator : AbstractValidator<CreateTestResultCommand>
    {
        public CreateTestResultCommandValidator()
        {
            RuleFor(createProgressUserCommand => createProgressUserCommand.ProgressMaterialId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createProgressUserCommand => createProgressUserCommand.TestId)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(createProgressUserCommand => createProgressUserCommand.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
