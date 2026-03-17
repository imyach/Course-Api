using Application.Common.Dtos;
using Application.Common.Dtos.ProgressModules;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Queries.ProgressModules.GetProgressModulesList;
using Application.Common.Queries.ProgressUsers.GetProgressUserList;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.ProgressModules
{
    [Collection("QueryCollection")]
    public class GetAllProgressModulesQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllProgressModulesQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetProgressModulesOfProgressUsersQueryHandler_Success()
        {
            var handler = new GetAllProgressModuleQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllProgressModuleQuery
            {
                ProgressUserId = Guid.Parse("812268C6-5E26-4003-A11B-61C13D858735"),
                CurrentUserId = CoursesContextFactory.UserStudent,
            }, CancellationToken.None);

            result.ShouldBeOfType<ProgressModuleListVm>();
            result.ProgressModules.ShouldNotBeNull();
            result.ProgressModules.Count.ShouldBe(1);

        }
    }
}
