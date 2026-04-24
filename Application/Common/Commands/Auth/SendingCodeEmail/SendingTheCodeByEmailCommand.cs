using Application.Common.Dtos.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.SendingCodeEmail
{
    public class SendingTheCodeByEmailCommand : IRequest
    {
        public string UserEmail {  get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
