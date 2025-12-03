using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Courses.UpdateCourse
{
    public class UpdateCourseCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Rait { get; set; } = 0;
        public Guid UserId { get; set; }
    }
}
