using Application.Common.Commands.ProgressModules.UpdateProgressModules;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.ProgressModules
{
    public class UpdateProgressModuleCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateProgressModuleCommndHandler_Success()
        {
            var handler = new UpdateProgressModuleCommandHandler(Context);
            var updatedStatus = "Test status";

            await handler.Handle(new UpdateProgressModuleCommand
            {
                Id = CoursesContextFactory.ProgressModuleForUpdate,
                Status = updatedStatus,
                CurrentUserId = CoursesContextFactory.UserCouch,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.ProgressModules.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.ProgressModuleForUpdate &&
                user.Status == updatedStatus));
        }

        [Fact]
        public async Task UpdateProgressModuleCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateProgressModuleCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateProgressModuleCommand
                    {
                        Id = Guid.NewGuid(),
                        Status = "Updated status",
                        CurrentUserId = CoursesContextFactory.UserCouch,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateProgressModuleCommandHandler_AccessException()
        {
            var handler = new UpdateProgressModuleCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new UpdateProgressModuleCommand
                    {
                        Id = CoursesContextFactory.ProgressModuleForUpdate,
                        Status = "Updated status",
                        CurrentUserId = CoursesContextFactory.UserStudent,
                    }, CancellationToken.None));
        }
    }
}
