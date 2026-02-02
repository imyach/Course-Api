using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.TestResults.DeleteTestResults
{
    public class DeleteTestResultCommandValidator : AbstractValidator<DeleteTestResultCommand>
    {
        public DeleteTestResultCommandValidator()
        {
            RuleFor(deleteProgressUserCommand => deleteProgressUserCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(deleteProgressUserCommand => deleteProgressUserCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
