using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.ProgressModules;
using Application.Common.Queries.ProgressMaterials.GetProgressMaterials;
using Application.Common.Queries.ProgressModules.GetProgressModules;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.ProgressMaterials
{
    [Collection("QueryCollection")]
    public class GetProgressMaterialDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetProgressMaterialDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetProgressMaterialDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsProgressMaterialQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsProgressMaterialQuery
                {
                    Id = Guid.Parse("4C27AAF3-CB96-46B2-86D6-65A23D733CB1"),
                    CurrentUserId = CoursesContextFactory.UserStudent
                }, CancellationToken.None);

            result.ShouldBeOfType<ProgressMaterialLookupDto>();
            result.Status.ShouldBe("Не начатa");
            result.StartedAt.ShouldBeNull();
        }
    }
}
