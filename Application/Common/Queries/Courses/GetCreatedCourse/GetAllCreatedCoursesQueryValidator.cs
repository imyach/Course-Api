using Application.Common.Queries.Courses.GetCourseList;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCreatedCourse
{
    public class GetAllCreatedCoursesQueryValidator : AbstractValidator<GetAllCreatedCoursesQuery>
    {
        public GetAllCreatedCoursesQueryValidator()
        {
            RuleFor(getAllCreatedCourseQuery => getAllCreatedCourseQuery.PageNumber)
                .NotNull().NotEmpty();
            RuleFor(getAllCreatedCourseQuery => getAllCreatedCourseQuery.PageSize)
                .NotNull().NotEmpty();
            RuleFor(getAllCreatedCourseQuery => getAllCreatedCourseQuery.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
