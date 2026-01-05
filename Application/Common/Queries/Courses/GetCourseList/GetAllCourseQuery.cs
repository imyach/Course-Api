using Application.Common.Dtos.Courses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCourseList
{
    public class GetAllCourseQuery : IRequest<object[]>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; } 
    }
}
