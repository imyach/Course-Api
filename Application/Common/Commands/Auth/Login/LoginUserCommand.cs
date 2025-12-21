using Application.Common.Dtos.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Common.Commands.Auth.Login
{
    public class LoginUserCommand : IRequest<TokensDto?>
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
