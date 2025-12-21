using Application.Common.Dtos.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.Refresh
{
    public class RefreshTokenCommand : IRequest<TokensDto?>
    {
        public Guid RefreshToken { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
