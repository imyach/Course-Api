using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Answers.CreateAnswer
{
    public class CreateAnswerCommandValidator : AbstractValidator<CreateAnswerCommand>
    {
        public CreateAnswerCommandValidator()
        {
            RuleFor(createAnswerCommand => createAnswerCommand.Text)
                .NotEmpty().MaximumLength(250).NotNull();
            RuleFor(createAnswerCommand => createAnswerCommand.QuestionId)
                .NotEqual(Guid.Empty).NotNull();
            RuleFor(createAnswerCommand => createAnswerCommand.CurrentUserId)
                .NotEqual(Guid.Empty).NotNull();
        }
    }
}
