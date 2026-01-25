using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Questions.GetQuestionList
{
    public class GetAllQuestionQueryValidator : AbstractValidator<GetAllQuestionQuery>
    {
        public GetAllQuestionQueryValidator()
        {
            RuleFor(getAllQuestionQuery => getAllQuestionQuery.TestId)
                .NotNull().NotEqual(Guid.Empty); ;
        }
    }
}
