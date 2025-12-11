using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Courses.DeleteCourse
{
    public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
    {
        public DeleteCourseCommandValidator()
        {
            RuleFor(deleteCourseCommand => deleteCourseCommand.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(deleteCourseCommand => deleteCourseCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
