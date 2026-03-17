using Application.Common.Dtos;
using Application.Common.Dtos.Roles;
using Application.Common.Dtos.Users;
using Application.Common.Queries.Roles.GetRoleList;
using Application.Common.Queries.Users.GetUsersList;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.Users
{
    [Collection("QueryCollection")]
    public class GetAllUsersQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllUsersQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetAllUsersQueryHandler_Success()
        {
            var handler = new GetAllUsersQueryHandler(mapper, context);

            var result = await handler.Handle(new GetAllUsersQuery 
            { 
                PageNumber = 1,
                PageSize = 3,
                SearchText = string.Empty 
            },CancellationToken.None);

            result.ShouldBeOfType<object[]>();

            result[0].ShouldBeOfType<UsersListVm>();
            var usersVm = result[0] as UsersListVm;
            usersVm.Users.ShouldNotBeNull();
            usersVm.Users.Count.ShouldBe(3);

            result[1].ShouldBeOfType<PagerInfoDto>();
            var pagerVm = result[1] as PagerInfoDto;
            pagerVm.ShouldNotBeNull();
            pagerVm.PageNumber.ShouldBe(1);
            pagerVm.PageSize.ShouldBe(3);

        }
    }
}
