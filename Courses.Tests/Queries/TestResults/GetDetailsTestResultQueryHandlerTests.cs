
using Application.Common.Dtos.TestResults;
using Application.Common.Queries.ProgressMaterials.GetProgressMaterials;
using Application.Common.Queries.TestResults.GetTestResults;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.TestResults
{
    [Collection("QueryCollection")]
    public class GetDetailsTestResultQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetDetailsTestResultQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);
        
        [Fact]
        public async Task GetTestResultDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsTestResultQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsTestResultQuery
                {
                    Id = Guid.Parse("DF31BBD3-2818-49F6-A716-5AC8E033B55C"),
                    CurrentUserId = CoursesContextFactory.UserStudent
                }, CancellationToken.None);

            result.ShouldBeOfType<TestResultLookupDto>();
            result.Score.ShouldBe(0);
            result.IsPassed.ShouldBe(false);
            result.CompletedAt.ShouldBe(DateTime.Today);
        }

    }
}
