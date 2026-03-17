using Application.Common.Commands.Auth.Registration;
using Application.Common.Commands.Users.CreateUser;
using Application.Common.Dtos.Auth;
using Courses.Tests.Common;
using Domain.Model;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Auth
{
    public class RegistrationUserCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task RegistrationUserCommandHandler_Success()
        {
            var handler = new RegistrationUserCommandHandler(Context,TokenServise, Hasher);
            string name = "Тест пользователь";
            string login = "Тест";
            string email = string.Empty;
            string password = "Тест";
            string phoneNumber = "89304066793";

            var result = await handler.Handle(new RegistrationUserCommand
            {
                NameUser = name,
                Login = login,
                Email = email,
                Password = password,
                PhoneNumber = phoneNumber
            }, CancellationToken.None);

            result.ShouldBeOfType<TokensDto?>();
            result.RefreshToken.ShouldNotBe(Guid.Empty);
            result.AccessToken.ShouldNotBeEmpty();
        }
    }
}
