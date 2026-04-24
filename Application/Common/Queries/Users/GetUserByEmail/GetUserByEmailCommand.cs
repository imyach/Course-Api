using Application.Common.Dtos.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Users.GetUserByEmail
{
    public class GetUserByEmailCommand : IRequest<UsersListVm>
    {
        public string Email { get; set; } = string.Empty;
    }
}
