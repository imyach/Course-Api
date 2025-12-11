using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Courses.UpdateCourse
{
    public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseCommandValidator()
        {
            RuleFor(updateCourseCommand => updateCourseCommand.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateCourseCommand => updateCourseCommand.Id)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateCourseCommand => updateCourseCommand.Title)
                .NotNull().NotEmpty().MaximumLength(100);
            RuleFor(updateCourseCommand => updateCourseCommand.Rait)
                .NotNull().NotEmpty().InclusiveBetween(1, 5);
            RuleFor(updateCourseCommand => updateCourseCommand.UserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(updateCourseCommand => updateCourseCommand.Description)
                .MaximumLength(1000);
        }
    }
}
