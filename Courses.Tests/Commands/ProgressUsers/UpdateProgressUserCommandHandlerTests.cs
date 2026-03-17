using Application.Common.Commands.ProgressUsers.UpdateProgressUser;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.ProgressUsers
{
    public class UpdateProgressUserCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateProgressUserCommndHandler_Success()
        {
            var handler = new UpdateProgressUserCommandHandler(Context);
            var updatedStatus= "Test status";

            await handler.Handle(new UpdateProgressUserCommand
            {
                Id = CoursesContextFactory.ProgressUserForUpdate,
                Status = updatedStatus,
                CurrentUserId = CoursesContextFactory.UserCouch,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.ProgressUsers.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.ProgressUserForUpdate &&
                user.Status == updatedStatus));
        }

        [Fact]
        public async Task UpdateProgressUserCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateProgressUserCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateProgressUserCommand
                    {
                        Id = Guid.NewGuid(),
                        Status = "Updated status",
                        CurrentUserId = CoursesContextFactory.UserCouch,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateProgressUserCommandHandler_AccessException()
        {
            var handler = new UpdateProgressUserCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new UpdateProgressUserCommand
                    {
                        Id = CoursesContextFactory.ProgressUserForUpdate,
                        Status = "Updated status",
                        CurrentUserId = CoursesContextFactory.UserStudent,
                    }, CancellationToken.None));
        }
    }
}

