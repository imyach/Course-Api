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
                .NotEmpty().MaximumLength(250).NotNull();
            RuleFor(updateAnswerCommand => updateAnswerCommand.Id)
                .NotEqual(Guid.Empty).NotNull();
            RuleFor(updateAnswerCommand => updateAnswerCommand.CurrentUserId)
               .NotEqual(Guid.Empty).NotNull();
        }
    }
}
