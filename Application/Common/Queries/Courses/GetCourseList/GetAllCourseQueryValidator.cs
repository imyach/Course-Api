using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCourseList
{
    public class GetAllCourseQueryValidator : AbstractValidator<GetAllCourseQuery>
    {
        public GetAllCourseQueryValidator() 
        {
            RuleFor(getAllCourseQuery => getAllCourseQuery.PageNumber)
                .NotNull().NotEmpty();
            RuleFor(getAllCourseQuery => getAllCourseQuery.PageSize)
                .NotNull().NotEmpty();
        }
    }
}
