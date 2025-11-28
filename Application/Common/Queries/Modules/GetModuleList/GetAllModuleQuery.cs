using Application.Common.Dtos.Modules;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Modules.GetModuleList
{
    public class GetAllModuleQuery : IRequest<ModuleListVm>
    {
        public Guid CurrentUserId { get; set; }
    }
}
