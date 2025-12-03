using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Questions.DeleteQuestion
{
    public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
    {
        public DeleteQuestionCommandValidator() 
        {
            RuleFor(deleteQuestionCommand => deleteQuestionCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
