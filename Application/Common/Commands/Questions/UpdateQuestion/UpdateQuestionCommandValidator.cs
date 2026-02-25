using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Questions.UpdateQuestion
{
    public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidator() 
        {
            RuleFor(updateQuestionCommand => updateQuestionCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateQuestionCommand => updateQuestionCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
