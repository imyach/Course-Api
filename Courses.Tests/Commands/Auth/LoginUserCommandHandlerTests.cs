using Application.Common.Commands.Auth.Login;
using Application.Common.Commands.Courses.CreateCourse;
using Application.Common.Dtos.Auth;
using Courses.Tests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Auth
{
    public class LoginUserCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task LoginUserCommandHandler_Success()
        {
            var handler = new LoginUserCommandHandler(Context,TokenServise,Hasher);
            var login = "Student";
            var password = "Student";

            var token = await handler.Handle(new LoginUserCommand
            {
                Login = login,
                Password = password,
            }, CancellationToken.None);

            token.ShouldBeOfType<TokensDto?>();
            token.RefreshToken.ShouldNotBe(Guid.Empty);
            token.AccessToken.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task LoginUserCommandHandler_WrognPassword()
        {
            var handler = new LoginUserCommandHandler(Context, TokenServise, Hasher);
            var login = "Student";
            var password = "123";

            var token = await handler.Handle(new LoginUserCommand
            {
                Login = login,
                Password = password,
            }, CancellationToken.None);

            token.ShouldBeNull();
        }
    }
}
