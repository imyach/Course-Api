using Application.Common.Dtos.ProgressModules;
using Application.Common.Dtos.ProgressUsers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressModules.GetProgressModules
{
    public class GetDetailsProgressModuleQuery : IRequest<ProgressModuleLookupDto>
    {
        public Guid CurrentUserId { get; set; }
        public Guid Id { get; set; }
    }
}
