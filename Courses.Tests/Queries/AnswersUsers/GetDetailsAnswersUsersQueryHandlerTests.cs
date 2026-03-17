using Application.Common.Dtos.AnswersUsers;
using Application.Common.Dtos.TestResults;
using Application.Common.Queries.AnswersUsers.GetAnswersUser;
using Application.Common.Queries.TestResults.GetTestResults;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.AnswersUsers
{
    [Collection("QueryCollection")]
    public class GetDetailsAnswersUsersQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetDetailsAnswersUsersQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetAnswersUserDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsAnswersUserQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsAnswersUserQuery
                {
                    Id = Guid.Parse("519F30AD-F025-44B6-8CFF-066679D81004"),
                    CurrentUserId = CoursesContextFactory.UserStudent
                }, CancellationToken.None);

            result.ShouldBeOfType<AnswersUserLookupDto>();
            result.ShouldNotBeNull();
        }
    }
}
