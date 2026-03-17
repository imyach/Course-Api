using Application.Common.Commands.Questions.DeleteQuestion;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Questions
{
    public class DeleteQuestionCommandHandlerTests: TestCommandBase
    {
        [Fact]
        public async Task DeleteQuestionCommandHandler_Success()
        {
            var handler = new DeleteQuestionCommandHandler(Context);

            await handler.Handle(new DeleteQuestionCommand
            {
                Id = CoursesContextFactory.QuestionForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.Null(Context.Questions.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.QuestionForDelete));
        }

        [Fact]
        public async Task DeleteQuestionCommandHandler_AccessException()
        {
            var handler = new DeleteQuestionCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteQuestionCommand
                    {
                        Id = CoursesContextFactory.QuestionForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}
