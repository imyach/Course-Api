using Application.Common.Dtos.Tests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Tests.CreateTest
{
    public class CreateTestCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }

        public Guid MatherialId { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
