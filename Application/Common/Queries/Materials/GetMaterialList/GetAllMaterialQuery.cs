using Application.Common.Dtos.Materials;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Materials.GetMaterialList
{
    public class GetAllMaterialQuery : IRequest<MaterialListVm>
    {
        public Guid ModuleId { get; set; }
    }
}
