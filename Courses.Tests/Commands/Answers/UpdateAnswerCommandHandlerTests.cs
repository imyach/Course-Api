using Application.Common.Commands.Answers.UpdateAnswer;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Answers
{
    public class UpdateAnswerCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateAnswerCommndHandler_Success()
        {
            var handler = new UpdateAnswerCommandHandler(Context);
            var updatedText = "Updated  Text of Answer";

            await handler.Handle(new UpdateAnswerCommand
            {
                Id = CoursesContextFactory.AnswerForUpdate,
                Text = updatedText,
                CurrentUserId = CoursesContextFactory.UserCouch,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.Answers.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.AnswerForUpdate &&
                user.Text == updatedText));
        }

        [Fact]
        public async Task UpdateAnswerCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateAnswerCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateAnswerCommand
                    {
                        Id = Guid.NewGuid(),
                        Text = "Updated Text",
                        CurrentUserId = CoursesContextFactory.UserCouch,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateAnswerCommandHandler_AccessException()
        {
            var handler = new UpdateAnswerCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new UpdateAnswerCommand
                    {
                        Id = CoursesContextFactory.AnswerForUpdate,
                        Text = "Updated Text",
                        CurrentUserId = CoursesContextFactory.UserStudent,
                    }, CancellationToken.None));
        }
    }
}

