using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.DeteleUser
{
    public class DeleteUserCommand: IRequest
    {
        public Guid CurrentUserId { get; set; }
        public Guid Id { get; set; }
    }
}
