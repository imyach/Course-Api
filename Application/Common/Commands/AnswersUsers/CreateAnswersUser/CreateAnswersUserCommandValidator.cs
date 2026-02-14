using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.AnswersUsers.CreateAnswersUser
{
    public class CreateAnswersUserCommandValidator : AbstractValidator<CreateAnswersUserCommand>
    {
        public CreateAnswersUserCommandValidator()
        {
            RuleFor(createAnswersUserCommand => createAnswersUserCommand.CurrentUserId)
                .NotEqual(Guid.Empty).NotNull();
            //RuleFor(createAnswersUserCommand => createAnswersUserCommand.AnswerId)
            //    .NotEqual(Guid.Empty).NotNull();
        }

    }
}
