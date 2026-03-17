using Application.Common.Commands.Auth.Logout;
using Application.Common.Commands.Auth.Refresh;
using Application.Common.Dtos.Auth;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Auth
{
    public class RefreshTokenCommandHandlerTests : TestCommandBase
    {

        [Fact]
        public async Task RefreshTokenCommandHandler_Success()
        {

            var handler = new RefreshTokenCommandHandler(Context, TokenServise);
            var currentUserId = CoursesContextFactory.UserAdmin;
            var token = Guid.Parse("7A8C5166-D90B-48BF-A0E9-DC5B3DA3FC2D");

            var result = await handler.Handle(new RefreshTokenCommand { CurrentUserId = currentUserId , RefreshToken = token }, CancellationToken.None);

            result.ShouldBeOfType<TokensDto?>();
            result.RefreshToken.ShouldNotBe(Guid.Empty);
            result.AccessToken.ShouldNotBeEmpty();
        }
    }
}
