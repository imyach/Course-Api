using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.ProgressModules;
using Application.Common.Queries.ProgressMaterials.GetProgressMaterialsList;
using Application.Common.Queries.ProgressModules.GetProgressModulesList;
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
    public class GetAllProgressMaterialsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllProgressMaterialsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);
        [Fact]
        public async Task GetProgressMaterialsOfProgressModuleQueryHandler_Success()
        {
            var handler = new GetAllProgressMaterialQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllProgressMaterialQuery
            {
                ProgressModuleId = Guid.Parse("0944F140-6A50-432D-9064-BCDF552D92D6"),
                CurrentUserId = CoursesContextFactory.UserStudent,
            }, CancellationToken.None);

            result.ShouldBeOfType<ProgressMaterialListVm>();
            result.ProgressMaterials.ShouldNotBeNull();
            result.ProgressMaterials.Count.ShouldBe(1);

        }
    }
}
