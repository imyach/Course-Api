using Application.Common.Dtos.Materials;
using Application.Common.Dtos.Reviews;
using Application.Common.Queries.Materials.GetMaterial;
using Application.Common.Queries.Reviews.GetReview;
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
    public class GetMaterialDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetMaterialDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetMaterialDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsMaterialQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsMaterialQuery
                {
                    Id = Guid.Parse("C411E9B9-B6B5-4AC5-8CE3-34BE991BC910"),
                }, CancellationToken.None);

            result.ShouldBeOfType<MaterialLookupDto>();
            result.Title.ShouldBe("First title material of first course");
            result.Order.ShouldBe(1);
            result.Description.ShouldBe("First description material of first course");
        }
    }
}
