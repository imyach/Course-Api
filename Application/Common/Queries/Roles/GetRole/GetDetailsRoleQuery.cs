using Application.Common.Dtos.Roles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Roles.GetRole
{
    public class GetDetailsRoleQuery : IRequest<RoleLookupDto>
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
