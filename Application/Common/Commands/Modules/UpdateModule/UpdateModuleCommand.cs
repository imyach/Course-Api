using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.UpdateModule
{
    public class UpdateModuleCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }
        public Guid CourseId { get; set; }


    }
}
