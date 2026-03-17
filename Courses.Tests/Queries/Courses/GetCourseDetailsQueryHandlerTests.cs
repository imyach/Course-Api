using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Users;
using Application.Common.Queries.Courses.GetCourse;
using Application.Common.Queries.Users.GetUser;
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
    public class GetCourseDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetCourseDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetCourseDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsCourseQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsCourseQuery
                {
                    Id = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8")
                }, CancellationToken.None);

            result.ShouldBeOfType<CourseLookupDto>();
            result.CreatedAt.ShouldBe(DateTime.Today);
            result.Title.ShouldBe("First course title");
            result.Description.ShouldBe("First course description");
            result.UpdateAt.ShouldBeNull();
            result.Rait.ShouldBe(2);
            result.Status.ShouldBe("Draft");


        }
    }
}
