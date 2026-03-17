using Application.Common.Dtos.TestResults;
using Application.Common.Queries.AnswersUsers.GetAnswersUserList;
using Application.Common.Queries.TestResults.GetTestResultsList;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.Common.Dtos.AnswersUsers.TestResult.CheckingResponsesDto;

namespace Courses.Tests.Queries.AnswersUsers
{
    [Collection("QueryCollection")]
    public class GetAllAnswersUsersQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllAnswersUsersQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetAnswersUsersOfTestResultQueryHandler_Success()
        {
            var handler = new GetAllAnswersUserQueryHandler(context);

            var result = await handler.Handle(new GetAllAnswersUserQuery
            {
                TestResultId = Guid.Parse("DF31BBD3-2818-49F6-A716-5AC8E033B55C"),
                CurrentUserId = CoursesContextFactory.UserStudent,
            }, CancellationToken.None);

            result.ShouldBeOfType<TestHistoryVm>();
            result.ShouldNotBeNull();

        }
    }
}
