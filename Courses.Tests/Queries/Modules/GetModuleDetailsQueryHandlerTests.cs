using Application.Common.Dtos.Modules;
using Application.Common.Dtos.Reviews;
using Application.Common.Queries.Modules.GetModule;
using Application.Common.Queries.Reviews.GetReview;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.Modules
{

    [Collection("QueryCollection")]
    public class GetModuleDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetModuleDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetModuleDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsModuleQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsModuleQuery
                {
                    Id = Guid.Parse("FED57263-8338-42FA-82F7-D5752D914CE1")
                }, CancellationToken.None);

            result.ShouldBeOfType<ModuleLookupDto>();
            result.Title.ShouldBe("First title module of first course");
            result.Description.ShouldBe("First description module of first course");
            result.Order.ShouldBe(1);
        }
    }
}
