using Application.Common.Dtos;
using Application.Common.Dtos.Reviews;
using Application.Common.Dtos.Roles;
using Application.Common.Dtos.Users;
using Application.Common.Queries.Reviews.GetReviewList;
using Application.Common.Queries.Roles.GetRoleList;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.Reviews
{

    [Collection("QueryCollection")]
    public class GetAllReviewsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllReviewsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);
        
        [Fact]
        public async Task GetAllReviewsQueryHandler_Success()
        {
            var handler = new GetAllReviewQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllReviewQuery
            {
                SortAscending = true,
                SortBy = "ByDate",
                PageSize = 5,
                PageNumber = 1,
                IdCourse = Guid.Parse("C832A1DB-AC83-4250-B915-FF64AD8383B5"),
            }, CancellationToken.None);

            result[0].ShouldBeOfType<ReviewListVm>();
            var reviewsVm = result[0] as ReviewListVm;
            reviewsVm.Reviews.ShouldNotBeNull();
            reviewsVm.Reviews.Count.ShouldBe(4);

            result[1].ShouldBeOfType<PagerInfoDto>();
            var pagerVm = result[1] as PagerInfoDto;
            pagerVm.ShouldNotBeNull();
            pagerVm.PageNumber.ShouldBe(1);
            pagerVm.PageSize.ShouldBe(5);
        }
    }
}
