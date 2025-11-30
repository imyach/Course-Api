using Application.Common.Dtos.ProgressUsers;
using Application.Common.Mappings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressUsers.GetProgressUser
{
    public class GetDetailsProgressUserQuery : IRequest<ProgressUserLookupDto>
    {
        public Guid CurrentUserId { get; set; }
        public Guid Id { get; set; }
    }
}
