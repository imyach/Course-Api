using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.ProgressModules;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressMaterials.GetProgressMaterialsList
{
    public class GetAllProgressMaterialQuery : IRequest<ProgressMaterialListVm>
    {
        public Guid CurrentUserId { get; set; }
        public Guid ProgressModuleId { get; set; }
    }
}
