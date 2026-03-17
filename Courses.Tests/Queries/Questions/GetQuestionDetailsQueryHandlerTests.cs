using Application.Common.Dtos.Questions;
using Application.Common.Dtos.Tests;
using Application.Common.Queries.Questions.GetQuestions;
using Application.Common.Queries.Tests.GetTest;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.Questions
{
    [Collection("QueryCollection")]
    public class GetQuestionDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetQuestionDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);
        [Fact]
        public async Task GetQuestionDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsQuestionQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsQuestionQuery
                {
                    Id = Guid.Parse("2BD4F87B-F177-4C29-8A72-81AC7C3633DC"),
                }, CancellationToken.None);

            result.ShouldBeOfType<QuestionLookupDto>();
            result.Text.ShouldBe("Question first course");
        }
    }
}
