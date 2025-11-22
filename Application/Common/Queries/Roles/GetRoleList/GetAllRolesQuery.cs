using Application.Common.Dtos.Roles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Roles.GetRoleList
{
    public class GetAllRolesQuery : IRequest<RoleListVm>
    {
        public Guid UserId { get; set; }
    }
}
