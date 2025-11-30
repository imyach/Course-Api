using MediatR;
using System;
using System.Text;

namespace Application.Common.Commands.ProgressUsers.DeleteProgressUser
{
    public class DeleteProgressUserCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
