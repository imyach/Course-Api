using Application.Common.Dtos.ProgressUsers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressUsers.GetProgressUserList
{
    public class GetAllProgressUserQuery : IRequest<object[]>
    {
        public Guid CurrentUserId { get; set; }
        public Guid UserId { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
    }
}
