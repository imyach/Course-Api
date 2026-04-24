using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.SendingPasswordEmail
{
    public class SendingThePasswordByEmailCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
