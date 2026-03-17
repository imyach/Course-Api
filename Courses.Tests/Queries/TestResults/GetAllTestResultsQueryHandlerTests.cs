using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.TestResults;
using Application.Common.Queries.ProgressMaterials.GetProgressMaterialsList;
using Application.Common.Queries.TestResults.GetTestResultsList;
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
    public class GetAllTestResultsQueryHandlerTests
    {

        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllTestResultsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetTestResultsOfProgressMaterialQueryHandler_Success()
        {
            var handler = new GetAllTestResultQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllTestResultQuery
            {
                ProgressMaterialId = Guid.Parse("4C27AAF3-CB96-46B2-86D6-65A23D733CB1"),
                CurrentUserId = CoursesContextFactory.UserStudent,
            }, CancellationToken.None);

            result.ShouldBeOfType<TestResultListVm>();
            result.TestResults.ShouldNotBeNull();
            result.TestResults.Count.ShouldBe(1);

        }
    }
}
