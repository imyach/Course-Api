using Application.Common.Dtos.Materials;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Materials.GetMaterial
{
    public class GetDetailsMaterialQuery : IRequest<MaterialLookupDto>
    {
        public Guid Id { get; set; }
    }
}
