using Application.Common.Dtos.Roles;
using Application.Common.Exceptions;
using Application.Common.Queries.Roles.GetRole;
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
    public class GetRoleDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper  mapper;

        public GetRoleDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);
        
        [Fact]
        public async Task GetRoleDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsRoleQueryHandler(context ,mapper);

            var result = await handler.Handle(
                new GetDetailsRoleQuery
                {
                    CurrentUserId = CoursesContextFactory.UserAdmin,
                    Id = CoursesContextFactory.RoleStudent
                }, CancellationToken.None);

            result.ShouldBeOfType<RoleLookupDto>();
            result.CreatedAt.ShouldBe(DateTime.Today);
            result.Name.ShouldBe("Student");
        }

        [Fact]
        public async Task GetRoleDetailsQueryHandler_AccessException()
        {
            var handler = new GetDetailsRoleQueryHandler(context, mapper);

            await Assert.ThrowsAsync<AccessException>(() => 
                handler.Handle(new GetDetailsRoleQuery
                {
                    CurrentUserId = CoursesContextFactory.UserStudent,
                    Id = CoursesContextFactory.RoleStudent
                }, CancellationToken.None));

        }
    }
}
