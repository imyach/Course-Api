using Application.Common.Dtos.ProgressUsers;
using Application.Common.Dtos.Users;
using Application.Common.Queries.ProgressUsers.GetProgressUser;
using Application.Common.Queries.Users.GetUser;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.ProgressUsers
{

    [Collection("QueryCollection")]
    public class GetProgressUserDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetProgressUserDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetProgressUserDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsProgressUserQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsProgressUserQuery
                {
                    Id = Guid.Parse("812268C6-5E26-4003-A11B-61C13D858735"),
                    CurrentUserId = CoursesContextFactory.UserStudent
                }, CancellationToken.None);

            result.ShouldBeOfType<ProgressUserLookupDto>();
            result.Status.ShouldBe("В прохождении");
            result.StartedAt.ShouldBe(DateTime.Today);
            result.FineshedAt.ShouldBeNull();

        }
    }
}
