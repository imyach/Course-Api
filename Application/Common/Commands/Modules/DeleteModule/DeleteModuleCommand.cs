using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.DeleteModule
{
    public class DeleteModuleCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
