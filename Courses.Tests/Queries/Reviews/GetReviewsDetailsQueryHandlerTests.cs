using Application.Common.Dtos.Reviews;
using Application.Common.Dtos.Users;
using Application.Common.Queries.Reviews.GetReview;
using Application.Common.Queries.Users.GetUser;
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
    public class GetReviewsDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetReviewsDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetReviewDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsReviewQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsReviewQuery
                {
                    Id = Guid.Parse("CE90037A-8F66-4CDC-97A7-C16B84F860D5")
                }, CancellationToken.None);

            result.ShouldBeOfType<ReviewLookupDto>();
            result.CreatedAt.ShouldBe(DateTime.Today);
            result.Rait.ShouldBe(5);
            result.Text.ShouldBe("Неплохой курс");
        }
    }
}
