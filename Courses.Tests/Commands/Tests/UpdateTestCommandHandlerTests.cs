using Application.Common.Commands.Tests.UpdateTest;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Tests
{
    public class UpdateTestCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateTestCommndHandler_Success()
        {
            var handler = new UpdateTestCommandHandler(Context);
            var updatedTitle = "Updated  title of Test";

            await handler.Handle(new UpdateTestCommand
            {
                Id = CoursesContextFactory.TestForUpdate,
                Title = updatedTitle,
                CurrentUserId = CoursesContextFactory.UserCouch,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.Tests.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.TestForUpdate &&
                user.Title == updatedTitle));
        }

        [Fact]
        public async Task UpdateTestCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateTestCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateTestCommand
                    {
                        Id = Guid.NewGuid(),
                        Title = "Updated title",
                        CurrentUserId = CoursesContextFactory.UserCouch,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateTestCommandHandler_AccessException()
        {
            var handler = new UpdateTestCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new UpdateTestCommand
                    {
                        Id = CoursesContextFactory.TestForUpdate,
                        Title = "Updated title",
                        CurrentUserId = CoursesContextFactory.UserStudent,
                    }, CancellationToken.None));
        }
    }
}
