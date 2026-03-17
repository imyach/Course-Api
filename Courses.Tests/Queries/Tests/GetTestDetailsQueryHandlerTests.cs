using Application.Common.Dtos.Materials;
using Application.Common.Dtos.Tests;
using Application.Common.Queries.Materials.GetMaterial;
using Application.Common.Queries.Tests.GetTest;
using AutoMapper;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.Tests
{

    [Collection("QueryCollection")]
    public class GetTestDetailsQueryHandlerTests
    {

        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetTestDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetTestDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsTestQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsTestQuery
                {
                    Id = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7"),
                }, CancellationToken.None);

            result.ShouldBeOfType<TestLookupDto>();
            result.Title.ShouldBe("First title test of first course");
            result.Description.ShouldBe("First description test of first course");
        }
    }
}
