using Application.Common.Dtos.Courses;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCourse
{
    public class GetDetailsCourseQuery : IRequest<CourseLookupDto>
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
