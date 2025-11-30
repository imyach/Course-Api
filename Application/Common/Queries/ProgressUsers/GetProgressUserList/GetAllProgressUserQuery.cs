using Application.Common.Dtos.ProgressUsers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressUsers.GetProgressUserList
{
    public class GetAllProgressUserQuery : IRequest<ProgressUserListVm>
    {
        public Guid CurrentUserId { get; set; }
    }
}
