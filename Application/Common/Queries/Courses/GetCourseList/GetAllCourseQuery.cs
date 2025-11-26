using Application.Common.Dtos.Courses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCourseList
{
    public class GetAllCourseQuery : IRequest<CourseListVm>
    {
    }
}
