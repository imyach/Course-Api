using Application.Common.Dtos.Modules;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Modules.GetModule
{
    public class GetDetailsModuleQuery : IRequest<ModuleLookupDto>
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
