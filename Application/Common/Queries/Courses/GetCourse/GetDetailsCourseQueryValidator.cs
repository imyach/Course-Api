using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCourse
{
    public class GetDetailsCourseQueryValidator : AbstractValidator<GetDetailsCourseQuery>
    {
        public GetDetailsCourseQueryValidator() 
        {
            RuleFor(getDetailsCourseQuery => getDetailsCourseQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
