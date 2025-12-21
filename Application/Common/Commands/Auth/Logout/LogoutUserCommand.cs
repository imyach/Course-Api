using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.Logout
{
    public class LogoutUserCommand : IRequest<Unit?>
    {
        public Guid CurrentUserId { get; set; }
    }
}
