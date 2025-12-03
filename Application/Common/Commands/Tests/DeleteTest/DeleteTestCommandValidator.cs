using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Tests.DeleteTest
{
    public class DeleteTestCommandValidator : AbstractValidator<DeleteTestCommand>
    {
        public DeleteTestCommandValidator()
        {
            RuleFor(deleteTestCommand => deleteTestCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
