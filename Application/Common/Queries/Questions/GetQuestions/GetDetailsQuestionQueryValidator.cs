using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Questions.GetQuestions
{
    public class GetDetailsQuestionQueryValidator : AbstractValidator<GetDetailsQuestionQuery>
    {
        public GetDetailsQuestionQueryValidator() 
        {
            RuleFor(getDetailsQuestionQuery => getDetailsQuestionQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
