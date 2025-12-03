using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Answers.GetAnswer
{
    public class GetDetailsAnswerQueryValidator : AbstractValidator<GetDetailsAnswerQuery>
    {
        public GetDetailsAnswerQueryValidator() 
        {
            RuleFor(getDetailsAnswerQuery => getDetailsAnswerQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
