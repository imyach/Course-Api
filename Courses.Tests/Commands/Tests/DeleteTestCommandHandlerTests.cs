using Application.Common.Commands.Tests.DeleteTest;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Tests
{
    public class DeleteTestCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteTestCommandHandler_Success()
        {
            var handler = new DeleteTestCommandHandler(Context);

            await handler.Handle(new DeleteTestCommand
            {
                Id = CoursesContextFactory.TestForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.Null(Context.Tests.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.TestForDelete));
        }

        [Fact]
        public async Task DeleteTestCommandHandler_AccessException()
        {
            var handler = new DeleteTestCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteTestCommand
                    {
                        Id = CoursesContextFactory.TestForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}
