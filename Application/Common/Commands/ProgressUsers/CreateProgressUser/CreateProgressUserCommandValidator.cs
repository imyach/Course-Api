using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressUsers.CreateProgressUser
{
    public class CreateProgressUserCommandValidator : AbstractValidator<CreateProgressUserCommand>
    {
        public CreateProgressUserCommandValidator()
        {
            RuleFor(createProgressUserCommand => createProgressUserCommand.CourseId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createProgressUserCommand => createProgressUserCommand.UserId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
