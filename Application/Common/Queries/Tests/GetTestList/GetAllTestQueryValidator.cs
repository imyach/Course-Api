using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Tests.GetTestList
{
    public class GetAllTestQueryValidator : AbstractValidator<GetAllTestQuery>
    {
        public GetAllTestQueryValidator()
        {
            RuleFor(getAllQuestionQuery => getAllQuestionQuery.MaterialId)
                .NotNull().NotEqual(Guid.Empty); ;
        }
    }
}
