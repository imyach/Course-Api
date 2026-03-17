using Application.Common.Commands.AnswersUsers.DeleteAnswersUser;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.AnswersUsers
{
    public class DeleteAnswersUserCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteAnswersUserCommandHandler_Success()
        {
            var handler = new DeleteAnswersUserCommandHandler(Context);

            await handler.Handle(new DeleteAnswersUserCommand
            {
                Id = CoursesContextFactory.AnswersUserForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.Null(Context.AnswersUsers.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.AnswersUserForDelete));
        }

        [Fact]
        public async Task DeleteAnswersUserCommandHandler_AccessException()
        {
            var handler = new DeleteAnswersUserCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteAnswersUserCommand
                    {
                        Id = CoursesContextFactory.AnswersUserForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}
