using Application.Common.Dtos;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Dtos.Users;
using Application.Common.Queries.ProgressUsers.GetProgressUserList;
using Application.Common.Queries.Users.GetUsersList;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.ProgressUsers
{

    [Collection("QueryCollection")]
    public class GetAllProgressUsersQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllProgressUsersQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetAllProgressUsersQueryHandler_Success()
        {
            var handler = new GetAllProgressUserQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllProgressUserQuery
            {
                UserId = CoursesContextFactory.UserStudent,

                PageNumber = 1,
                PageSize = 3,
                SearchText = string.Empty
            }, CancellationToken.None);

            result.ShouldBeOfType<object[]>();

            result[0].ShouldBeOfType<ProgressUserListVm>();
            var progressVm = result[0] as ProgressUserListVm;
            progressVm.ProgressUsers.ShouldNotBeNull();
            progressVm.ProgressUsers.Count.ShouldBe(1);

            result[1].ShouldBeOfType<ProgressInfo>();
            var progressInfoVm = result[1] as ProgressInfo;
            progressInfoVm.ShouldNotBeNull();
            progressInfoVm.CourseInPassage.ShouldBe(1);
            progressInfoVm.CompletedCourse.ShouldBe(0);

            result[2].ShouldBeOfType<PagerInfoDto>();
            var pagerVm = result[2] as PagerInfoDto;
            pagerVm.ShouldNotBeNull();
            pagerVm.PageNumber.ShouldBe(1);
            pagerVm.PageSize.ShouldBe(3);

        }
    }
}
