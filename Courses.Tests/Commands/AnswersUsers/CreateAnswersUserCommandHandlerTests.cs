using Application.Common.Commands.AnswersUsers.CreateAnswersUser;
using Application.Common.Commands.Questions.CreateQuestion;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.Common.Dtos.AnswersUsers.TestResult.CheckingResponsesDto;

namespace Courses.Tests.Commands.AnswersUsers
{
    public class CreateAnswersUserCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateAnswersUserCommandHandler_Success()
        {

            var handler = new CreateAnswersUserCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserStudent;
            DateTime complitedAt = DateTime.Today;
            Guid testId = Guid.Parse("DF31BBD3-2818-49F6-A716-5AC8E033B55C");
            var selectedAnswer = new List<SelectedAnswerDto>
            {
                new SelectedAnswerDto
                {
                    QuestionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                    AnswerId = Guid.Parse("C0233D09-16DA-4D64-9282-617A1D4D14CF"),
                },
                 new SelectedAnswerDto
                {
                    QuestionId = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                    AnswerId = Guid.Parse("8F6DBD71-5590-4796-BB25-64070C1A6B62"),
                }
            };

            var result = await handler.Handle(new CreateAnswersUserCommand
            {
                CurrentUserId = currentUserId,
                TestResultId = testId,
                SelectedAnswers = selectedAnswer,
                CompletedAt = complitedAt
            }, CancellationToken.None);


            result.ShouldBeOfType<CompleteTestResponseDto>();
        }
    }
}
