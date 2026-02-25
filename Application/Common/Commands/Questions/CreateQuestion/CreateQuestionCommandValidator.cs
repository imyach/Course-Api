using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Questions.CreateQuestion
{
    public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator() 
        {
            RuleFor(createQuestionCommand => createQuestionCommand.TestId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createQuestionCommand => createQuestionCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
