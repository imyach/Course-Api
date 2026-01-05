using Application.Common.Queries.Reviews.GetReview;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Reviews.GetReviewList
{
    public class GetAllReviewQueryValidator : AbstractValidator<GetAllReviewQuery>
    {
        public GetAllReviewQueryValidator()
        {
            RuleFor(getAllReviewQuery => getAllReviewQuery.IdCourse)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(getAllReviewQuery => getAllReviewQuery.PageNumber)
                .NotNull().NotEmpty();
            RuleFor(getAllReviewQuery => getAllReviewQuery.PageSize)
                .NotNull().NotEmpty();
        }
    }
}
