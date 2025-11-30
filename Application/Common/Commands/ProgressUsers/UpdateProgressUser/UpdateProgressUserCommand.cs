using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressUsers.UpdateProgressUser
{
    public class UpdateProgressUserCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }

        public string Status {  get; set; } = string.Empty;
        public DateTime FinishedAt {  get; set; }

    }
}
