using Application.Common.Commands.Users.DeteleUser;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Courses.Tests.Commands.Users
{
    public class DeleteUserCommandHandlerTest : TestCommandBase
    {

        [Fact]
        public async Task DeleteUserCommandHandler_Success()
        {
            var handler = new DeleteUserCommandHandler(Context);

            await handler.Handle(new DeleteUserCommand
            {
                Id = CoursesContextFactory.UserForDelete,
                CurrentUserId = CoursesContextFactory.UserForDelete
            }, CancellationToken.None);

            Assert.Null(Context.Users.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.UserForDelete));
        }

        [Fact]
        public async Task DeleteUserCommandHandler_AccessException()
        {
            var handler = new DeleteUserCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteUserCommand
                    {
                        Id = CoursesContextFactory.UserForDelete,
                        CurrentUserId = CoursesContextFactory.UserForUpdate
                    }, CancellationToken.None));
        }
    }
}
