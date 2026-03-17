using Application.Common.Commands.Answers.CreateAnswer;
using Application.Common.Commands.Tests.CreateTest;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Answers
{
    public class CreateAnswerCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateAnswerCommandHandler_Success()
        {
            var handler = new CreateAnswerCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserCouch;
            string text = "Тест текст";
            bool isCorrect = false;
            Guid questionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC");

            var answerId = await handler.Handle(new CreateAnswerCommand
            {
                Text = text,
                IsCorrect = isCorrect,
                CurrentUserId = currentUserId,
                QuestionId = questionId
            }, CancellationToken.None);

            Assert.NotNull(await Context.Answers.SingleOrDefaultAsync(answer =>
            answer.Id == answerId &&
            answer.Question.Test.Material.Module.Course.UserId == currentUserId &&
            answer.Text == text &&
            answer.IsCorrect == isCorrect &&
            answer.QuestionId == questionId));
        }

        [Fact]
        public async Task CreateAnswerCommandHandler_AccessEcxeption()
        {
            var handler = new CreateAnswerCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserStudent;
            string text = "Тест текст";
            bool isCorrect = false;
            Guid questionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC");

            await Assert.ThrowsAsync<AccessException>(async () =>
            {
                await handler.Handle(new CreateAnswerCommand
                {
                    Text = text,
                    IsCorrect = isCorrect,
                    CurrentUserId = currentUserId,
                    QuestionId = questionId
                }, CancellationToken.None);
            });
        }
    }
}
