using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Reviews.GetReview
{
    public class GetDetailsReviewQueryValidator : AbstractValidator<GetDetailsReviewQuery>
    {
        public GetDetailsReviewQueryValidator()
        {
            RuleFor(getDetailsReviewQuery => getDetailsReviewQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
