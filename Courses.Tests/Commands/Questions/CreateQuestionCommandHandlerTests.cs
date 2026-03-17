using Application.Common.Commands.Questions.CreateQuestion;
using Application.Common.Commands.Tests.CreateTest;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Questions
{
    public class CreateQuestionCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateQuestionCommandHandler_Success()
        {
            var handler = new CreateQuestionCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserCouch;
            string text = "Тест текст";
            Guid testId  = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7");

            var questionId = await handler.Handle(new CreateQuestionCommand
            {
                Text = text, 
                CurrentUserId = currentUserId,
                TestId = testId
            }, CancellationToken.None);

            Assert.NotNull(await Context.Questions.SingleOrDefaultAsync(question =>
            question.Id == questionId &&
            question.Test.Material.Module.Course.UserId == currentUserId &&
            question.Text == text &&
            question.TestId == testId));
        }

        [Fact]
        public async Task CreateQuestionCommandHandler_AccessEcxeption()
        {
            var handler = new CreateQuestionCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserStudent;
            string text = "Тест текст";
            Guid testId = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7");


            await Assert.ThrowsAsync<AccessException>(async () =>
            {
                await handler.Handle(new CreateQuestionCommand
                {
                    Text = text,
                    CurrentUserId = currentUserId,
                    TestId = testId
                }, CancellationToken.None);
            });
        }
    }
}

