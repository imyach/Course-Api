using Application.Common.Commands.Answers.DeleteAnswer;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Answers
{
    public class DeleteAnswerCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteAnswerCommandHandler_Success()
        {
            var handler = new DeleteAnswerCommandHandler(Context);

            await handler.Handle(new DeleteAnswerCommand
            {
                Id = CoursesContextFactory.AnswerForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.Null(Context.Answers.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.AnswerForDelete));
        }

        [Fact]
        public async Task DeleteAnswerCommandHandler_AccessException()
        {
            var handler = new DeleteAnswerCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteAnswerCommand
                    {
                        Id = CoursesContextFactory.AnswerForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}
