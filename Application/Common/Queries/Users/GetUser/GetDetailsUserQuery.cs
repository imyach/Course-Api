using Application.Common.Dtos.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Users.GetUser
{
    public class GetDetailsUserQuery : IRequest<UserLooupDto>
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
