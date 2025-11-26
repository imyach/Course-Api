using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Courses
{
    public class CourseListVm
    {
        public IList<CourseLookupDto> Courses { get; set; } = [];
    }
}
