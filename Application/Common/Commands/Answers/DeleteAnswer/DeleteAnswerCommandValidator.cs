using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Answers.DeleteAnswer
{
    public class DeleteAnswerCommandValidator : AbstractValidator<DeleteAnswerCommand>
    {
        public DeleteAnswerCommandValidator()
        {
            RuleFor(deleteAnswerCommand => deleteAnswerCommand.Id)
                .NotEqual(Guid.Empty).NotNull();
        }
    }
}
