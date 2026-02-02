using Application.Common.Dtos.ProgressModules;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressModules.GetProgressModulesList
{
    public class GetAllProgressModuleQuery : IRequest<ProgressModuleListVm>
    {
        public Guid CurrentUserId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProgressUserId { get; set; }
    }
}
