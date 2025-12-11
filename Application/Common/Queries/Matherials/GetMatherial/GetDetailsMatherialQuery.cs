using Application.Common.Dtos.Matherials;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Matherials.GetMatherial
{
    public class GetDetailsMatherialQuery : IRequest<MatherialLookupDto>
    {
        public Guid Id { get; set; }
    }
}
