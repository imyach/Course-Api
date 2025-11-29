using Application.Common.Dtos.Matherials;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Matherials.GetMatherialList
{
    public class GetAllMatherialQuery : IRequest<MatherialListVm>
    {
        public Guid CurrentUserId { get; set; }
    }
}
