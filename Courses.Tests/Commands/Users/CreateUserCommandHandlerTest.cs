using Application.Common.Commands.Users.CreateUser;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Users
{
    public class CreateUserCommandHandlerTest : TestCommandBase
    {
        [Fact]
        public async Task CreateUserCommandHandler_Success()
        {
            var handler = new CreateUserCommandHandler(Context, Hasher);
            var currentUserId = CoursesContextFactory.UserAdmin;
            string name = "Добавленный пользователь";
            string login = "Логин";
            string email = string.Empty;
            string password = "12345";
            Role role = new Role 
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                RoleName = "Тест"
            };
            string phoneNumber = "89304066793";

            var userId = await handler.Handle(new CreateUserCommand
            {
                CurrentUserId = currentUserId,
                NameUser = name,
                Login = login,
                Email = email,
                Password = password,
                Role = role,   
                PhoneNumber = phoneNumber
            }, CancellationToken.None);

            Assert.NotNull(await Context.Users.SingleOrDefaultAsync(user=> 
            user.Id == userId &&
            user.NameUser == name &&
            user.Login == login && 
            user.Email == email &&
            user.RoleId == role.Id &&
            user.PhoneNumber == phoneNumber));
        }



        [Fact]
        public async Task CreateUserCommandHandler_AccessEcxeption()
        {
            var handler = new CreateUserCommandHandler(Context, Hasher);
            var currentUserId = CoursesContextFactory.UserStudent;
            string name = "Добавленный пользователь";
            string login = "Логин";
            string email = string.Empty;
            string password = "12345";
            Role role = new Role
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                RoleName = "Тест"
            };
            string phoneNumber = "89304066793";


            await Assert.ThrowsAsync<AccessException>(()=>
                handler.Handle(new CreateUserCommand
                {
                    CurrentUserId = currentUserId,
                    NameUser = name,
                    Login = login,
                    Email = email,
                    Password = password,
                    Role = role,
                    PhoneNumber = phoneNumber
                }, CancellationToken.None));
        }
    }
}
