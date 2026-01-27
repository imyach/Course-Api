using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Courses.CreateCourse
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
            RuleFor(createCourseCommand => createCourseCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createCourseCommand => createCourseCommand.Title)
                .NotNull().NotEmpty().MaximumLength(100);
        }

            
    }
}
