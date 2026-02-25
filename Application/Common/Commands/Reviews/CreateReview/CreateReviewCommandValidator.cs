using Application.Common.Commands.Answers.CreateAnswer;
using Application.Common.Commands.Questions.CreateQuestion;
using Application.Common.Commands.Rewies.CreateReview;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Reviews.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator() 
        {
            RuleFor(сreateReviewCommand => сreateReviewCommand.CourseId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(сreateReviewCommand => сreateReviewCommand.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(сreateReviewCommand => сreateReviewCommand.Text)
               .NotNull().NotEmpty().MaximumLength(3000);
            RuleFor(сreateReviewCommand => сreateReviewCommand.Rait)
                .NotNull().NotEmpty().InclusiveBetween(1, 5);
        }
    }
}
