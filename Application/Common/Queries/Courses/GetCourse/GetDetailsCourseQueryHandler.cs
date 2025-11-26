using Application.Common.Dtos.Courses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCourse
{
    public class GetDetailsCourseQueryHandler : IRequestHandler<GetDetailsCourseQuery, CourseLookupDto>
    {
        public Task<CourseLookupDto> Handle(GetDetailsCourseQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
