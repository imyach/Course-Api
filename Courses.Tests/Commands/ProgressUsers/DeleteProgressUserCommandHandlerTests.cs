using Application.Common.Commands.ProgressUsers.DeleteProgressUser;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.ProgressUsers
{
    public class DeleteProgressUserCommandHandlerTests :TestCommandBase
    {
        [Fact]
        public async Task DeleteProgressUserCommandHandler_Success()
        {
            var handler = new DeleteProgressUserCommandHandler(Context);

            await handler.Handle(new DeleteProgressUserCommand
            {
                Id = CoursesContextFactory.ProgressUserForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.Null(Context.ProgressUsers.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.ProgressUserForDelete));
        }

        [Fact]
        public async Task DeleteProgressUserCommandHandler_AccessException()
        {
            var handler = new DeleteProgressUserCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteProgressUserCommand
                    {
                        Id = CoursesContextFactory.ProgressUserForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}
