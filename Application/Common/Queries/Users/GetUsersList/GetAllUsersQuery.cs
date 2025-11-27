using Application.Common.Dtos.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Users.GetUsersList
{
    public class GetAllUsersQuery : IRequest<UsersListVm>
    {
        public Guid CurrentUserId { get; set; }
    }
}
