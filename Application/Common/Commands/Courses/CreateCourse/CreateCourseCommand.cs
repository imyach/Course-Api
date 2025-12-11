using Application.Common.Mappings;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Courses.CreateCourse
{
    public class CreateCourseCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Rait { get; set; } = 0;
    }
}
