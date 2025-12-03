using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Answers.UpdateAnswer
{
    public class UpdateAnswerCommandValidator : AbstractValidator<UpdateAnswerCommand>
    {
        public UpdateAnswerCommandValidator()
        {
            RuleFor(updateAnswerCommand => updateAnswerCommand.Text)
                .NotEmpty().MaximumLength(1000).NotNull();
            RuleFor(updateAnswerCommand => updateAnswerCommand.IsCorrect)
                .NotEmpty().NotNull();
            RuleFor(updateAnswerCommand => updateAnswerCommand.Id)
                .NotEqual(Guid.Empty).NotNull();
        }
    }
}
