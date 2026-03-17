using Application.Common.Dtos;
using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Modules;
using Application.Common.Queries.Courses.GetCourseList;
using Application.Common.Queries.Modules.GetModuleList;
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
    public class GetAllModulesQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllModulesQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetModulesOfCourseQueryHandler_Success()
        {
            var handler = new GetAllModuleQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllModuleQuery
            {
                CourseId = Guid.Parse("C832A1DB-AC83-4250-B915-FF64AD8383B5"),
            }, CancellationToken.None);

            result.ShouldBeOfType<ModuleListVm>();
            result.Modules.Count.ShouldBe(1);
        }
    }
}
