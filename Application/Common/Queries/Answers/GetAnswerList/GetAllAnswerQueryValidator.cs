using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Answers.GetAnswerList
{
    public class GetAllAnswerQueryValidator: AbstractValidator<GetAllAnswerQuery>
    {
        public GetAllAnswerQueryValidator()
        {
            RuleFor(getAllAnswerQuery=> getAllAnswerQuery.QuestionId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
