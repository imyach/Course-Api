using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCreatedCourse
{
    public class GetAllCreatedCoursesQuery : IRequest<object[]>
    {
        public Guid CurrentUserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        
    }
}
