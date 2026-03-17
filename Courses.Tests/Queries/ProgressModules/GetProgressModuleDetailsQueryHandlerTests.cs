using Application.Common.Dtos.ProgressModules;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Queries.ProgressModules.GetProgressModules;
using Application.Common.Queries.ProgressUsers.GetProgressUser;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.ProgressModules
{
    [Collection("QueryCollection")]
    public class GetProgressModuleDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetProgressModuleDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);
        [Fact]
        public async Task GetProgressModuleDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsProgressModuleQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsProgressModuleQuery
                {
                    Id = Guid.Parse("0944F140-6A50-432D-9064-BCDF552D92D6"),
                    CurrentUserId = CoursesContextFactory.UserStudent
                }, CancellationToken.None);

            result.ShouldBeOfType<ProgressModuleLookupDto>();
            result.Status.ShouldBe("Не начат");
            result.StartedAt.ShouldBeNull();
        }
    }
}
