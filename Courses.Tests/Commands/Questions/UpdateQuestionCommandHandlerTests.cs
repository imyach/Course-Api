using Application.Common.Commands.Questions.UpdateQuestion;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Questions
{
    public class UpdateQuestionCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateQuestionCommndHandler_Success()
        {
            var handler = new UpdateQuestionCommandHandler(Context);
            var updatedText = "Updated  Text of Question";

            await handler.Handle(new UpdateQuestionCommand
            {
                Id = CoursesContextFactory.QuestionForUpdate,
                Text = updatedText,
                CurrentUserId = CoursesContextFactory.UserCouch,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.Questions.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.QuestionForUpdate &&
                user.Text == updatedText));
        }

        [Fact]
        public async Task UpdateQuestionCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateQuestionCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateQuestionCommand
                    {
                        Id = Guid.NewGuid(),
                        Text = "Updated Text",
                        CurrentUserId = CoursesContextFactory.UserCouch,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateQuestionCommandHandler_AccessException()
        {
            var handler = new UpdateQuestionCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new UpdateQuestionCommand
                    {
                        Id = CoursesContextFactory.QuestionForUpdate,
                        Text = "Updated Text",
                        CurrentUserId = CoursesContextFactory.UserStudent,
                    }, CancellationToken.None));
        }
    }
}
