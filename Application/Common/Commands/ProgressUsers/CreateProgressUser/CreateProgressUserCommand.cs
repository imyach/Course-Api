using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressUsers.CreateProgressUser
{
    public class CreateProgressUserCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }
        public Guid CourseId { get; set; }
    }
}
