using Application.Common.Commands.Auth.Login;
using Application.Common.Commands.Auth.Logout;
using Courses.Tests.Common;
using MediatR;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Auth
{
    public class LogoutUserCommandHandlerTests : TestCommandBase
    {

        [Fact]
        public async Task LogoutUserCommandHandler_Success()
        {

            var handler = new LogoutUserCommandHandler(Context, TokenServise);
            var currentUserId = CoursesContextFactory.UserAdmin;

            var result = await handler.Handle(new LogoutUserCommand { CurrentUserId = currentUserId }, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }
    }
}
