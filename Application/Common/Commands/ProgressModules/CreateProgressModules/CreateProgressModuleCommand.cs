using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressModules.CreateProgressModules
{
    public class CreateProgressModuleCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }
        public Guid ProgressUserId { get; set; }
        public Guid ModuleId { get; set; }
    }
}
