using Application.Common.Dtos.ProgressMaterials;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressMaterials.GetProgressMaterials
{
    public class GetDetailsProgressMaterialQuery : IRequest<ProgressMaterialLookupDto>
    {
        public Guid CurrentUserId { get; set; }
        public Guid Id { get; set; }
    }
}
