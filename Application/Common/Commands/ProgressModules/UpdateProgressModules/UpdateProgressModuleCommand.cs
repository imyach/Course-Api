using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressModules.UpdateProgressModules
{
    public class UpdateProgressModuleCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }

        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
    }
}
