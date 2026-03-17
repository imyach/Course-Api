using Application.Common.Dtos;
using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Materials;
using Application.Common.Dtos.Modules;
using Application.Common.Queries.Courses.GetCourseList;
using Application.Common.Queries.Materials.GetMaterialList;
using Application.Common.Queries.Modules.GetModuleList;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.Materials
{

    [Collection("QueryCollection")]
    public class GetAllMaterialsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllMaterialsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetMaterialsOfModuleQueryHandler_Success()
        {
            var handler = new GetAllMaterialQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllMaterialQuery
            {
                ModuleId = Guid.Parse("B5293697-9BC5-4137-835A-39E3E0873FE5"),
            }, CancellationToken.None);

            result.ShouldBeOfType<MaterialListVm>();
            result.Materials.Count.ShouldBe(1);
        }
    }
}
