using Application.Common.Dtos.Roles;
using Application.Common.Exceptions;
using Application.Common.Queries.Roles.GetRole;
using Application.Common.Queries.Roles.GetRoleList;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.Roles
{
    [Collection("QueryCollection")]
    public class GetAllRolesQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllRolesQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetAllRolesQueryHandler_Success()
        {
            var handler = new GetAllRolesQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllRolesQuery { CurrentUserId = CoursesContextFactory.UserAdmin}, CancellationToken.None);

            result.ShouldBeOfType<RoleListVm>();
            result.Roles.Count.ShouldBe(3);
        }

        [Fact]
        public async Task GetAllRolesQueryHandler_AccessException()
        {
            var handler = new GetAllRolesQueryHandler(context, mapper);

            await Assert.ThrowsAsync<AccessException>(() =>
                handler.Handle(new GetAllRolesQuery
                {
                    CurrentUserId = CoursesContextFactory.UserStudent,
                }, CancellationToken.None));
        }
    }
}
