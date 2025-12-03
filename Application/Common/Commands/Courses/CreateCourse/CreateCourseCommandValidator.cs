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
            RuleFor(createCourseCommand => createCourseCommand.Title)
                .NotNull().NotEmpty().MaximumLength(100);
            RuleFor(createCourseCommand => createCourseCommand.Rait)
                .NotNull().NotEmpty().InclusiveBetween(1,5);
            RuleFor(createCourseCommand => createCourseCommand.UserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createCourseCommand => createCourseCommand.Description)
               .MaximumLength(1000);
        }

            
    }
}
