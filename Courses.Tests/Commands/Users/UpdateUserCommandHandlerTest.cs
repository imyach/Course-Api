using Application.Common.Commands.Users.UpdateUser;
using Application.Common.Commands.Users.UpdateUserForAdmin;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Users
{
    public class UpdateUserCommandHandlerTest :  TestCommandBase
    {
        [Fact]
        public async Task UpdateUserCommndHandler_Success()
        {
            var handler = new UpdateUserCommandHandler(Context, TokenServise, Hasher);
            var updatedEmail = "Updated email";
            var updatedLogin = "Updated user login";

            await handler.Handle(new UpdateUserCommand
            {
                Id = CoursesContextFactory.UserForUpdate,
                Email = updatedEmail,
                Login = updatedLogin,

                CurrentUserId = CoursesContextFactory.UserForUpdate,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.Users.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.UserForUpdate &&
                user.Email == updatedEmail && 
                user.Login == updatedLogin));
        }

        [Fact]
        public async Task UpdateUserCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateUserCommandHandler(Context, TokenServise, Hasher);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateUserCommand
                    {
                        Id = Guid.NewGuid(),
                        Email = "Updated email",
                        CurrentUserId = CoursesContextFactory.UserForUpdate,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateUserForAdminCommndHandler_Success()
        {
            var handler = new UpdateUserForAdminCommandHandler(Context, Hasher);
            var updatedEmail = "Updated email";
            var updatedLogin = "Updated user login";

            await handler.Handle(new UpdateUserForAdminCommand
            {
                Id = CoursesContextFactory.UserForUpdate,
                Email = updatedEmail,
                Login = updatedLogin,
                CurrentUserId = CoursesContextFactory.UserAdmin,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.Users.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.UserForUpdate &&
                user.Email == updatedEmail &&
                user.Login == updatedLogin));
        }

        [Fact]
        public async Task UpdateUserForAdminCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateUserForAdminCommandHandler(Context, Hasher);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateUserForAdminCommand
                    {
                        Id = Guid.NewGuid(),
                        Email = "Updated email",
                        CurrentUserId = CoursesContextFactory.UserAdmin,
                    }, CancellationToken.None);
            });
        }
    }
}
