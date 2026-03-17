using Application.Common.Dtos.Modules;
using Application.Common.Dtos.Tests;
using Application.Common.Queries.Modules.GetModuleList;
using Application.Common.Queries.Tests.GetTestList;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.Tests
{

    [Collection("QueryCollection")]
    public class GetAllTestsQueryHandlerTests
    {

        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllTestsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);
        
        [Fact]
        public async Task GetTestsOfMaterialQueryHandler_Success()
        {
            var handler = new GetAllTestQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllTestQuery
            {
               MaterialId = Guid.Parse("A329C9A4-50B2-48E8-A213-8F6B64E15358"),
            }, CancellationToken.None);

            result.ShouldBeOfType<TestListVm>();
            result.Tests.Count.ShouldBe(1);
        }
    }
}
