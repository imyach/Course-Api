using Application.Common.Dtos.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Users.GetUsersList
{
    public class GetAllUsersQuery : IRequest<object[]>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
    }
}
