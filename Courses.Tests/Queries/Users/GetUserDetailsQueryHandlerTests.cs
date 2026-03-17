using Application.Common.Dtos.Roles;
using Application.Common.Dtos.Users;
using Application.Common.Queries.Roles.GetRole;
using Application.Common.Queries.Users.GetUser;
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
    public class GetUserDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetUserDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetUserDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsUserQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsUserQuery
                {
                    Id = CoursesContextFactory.UserCouch
                }, CancellationToken.None);

            result.ShouldBeOfType<UserLookupDto>();
            result.CreatedAt.ShouldBe(DateTime.Today);
            result.NameUser.ShouldBe("Couch");
            result.Login.ShouldBe("Couch");
            result.Email.ShouldBe("Couch@gmail.com");
            result.HashPassword.ShouldBe("$2a$08$YVGkRoiMjJXAwo5N/vwb.OdHm8RPhr0btvZi9Jos9HOnfAhlA0/lu");
            result.PhoneNumber.ShouldBe("89304066793");

        }
    }
}
