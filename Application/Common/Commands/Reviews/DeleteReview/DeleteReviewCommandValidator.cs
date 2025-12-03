using Application.Common.Commands.Rewies.DeleteReview;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Reviews.DeleteReview
{
    public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewCommandValidator() 
        {
            RuleFor(deleteReviewCommand => deleteReviewCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
