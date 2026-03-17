using Application.Common.Dtos;
using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Reviews;
using Application.Common.Queries.Courses.GetCourseList;
using Application.Common.Queries.Courses.GetCreatedCourse;
using Application.Common.Queries.Reviews.GetReviewList;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.Courses
{

    [Collection("QueryCollection")]
    public class GetAllCoursesQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllCoursesQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetPublishedCoursesQueryHandler_Success()
        {
            var handler = new GetAllCourseQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllCourseQuery
            {
                PageSize = 5,
                PageNumber = 1,
                SearchText = string.Empty
            }, CancellationToken.None);

            result[0].ShouldBeOfType<CourseListVm>();
            var coursesVm = result[0] as CourseListVm;
            coursesVm.Courses.ShouldNotBeNull();
            coursesVm.Courses.Count.ShouldBe(3);

            result[1].ShouldBeOfType<PagerInfoDto>();
            var pagerVm = result[1] as PagerInfoDto;
            pagerVm.ShouldNotBeNull();
            pagerVm.PageNumber.ShouldBe(1);
            pagerVm.PageSize.ShouldBe(5);
        }

        [Fact]
        public async Task GetPublishedAndDraftedCoursesQueryHandler_Success()
        {
            var handler = new GetAllCreatedCoursesQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllCreatedCoursesQuery
            {
                CurrentUserId = CoursesContextFactory.UserCouch,
                PageSize = 5,
                PageNumber = 1,
            }, CancellationToken.None);

            result[0].ShouldBeOfType<CourseListVm>();
            var coursesVm = result[0] as CourseListVm;
            coursesVm.Courses.ShouldNotBeNull();
            coursesVm.Courses.Count.ShouldBe(3);

            result[1].ShouldBeOfType<PagerInfoDto>();
            var pagerVm = result[1] as PagerInfoDto;
            pagerVm.ShouldNotBeNull();
            pagerVm.PageNumber.ShouldBe(1);
            pagerVm.PageSize.ShouldBe(5);
        }
    }
}
