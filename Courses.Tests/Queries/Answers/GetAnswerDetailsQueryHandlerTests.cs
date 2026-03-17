using Application.Common.Dtos.Answers;
using Application.Common.Dtos.Tests;
using Application.Common.Queries.Answers.GetAnswer;
using Application.Common.Queries.Tests.GetTest;
using AutoMapper;
using Courses.Tests.Common;
using Persistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Queries.Answers
{
    [Collection("QueryCollection")]
    public class GetAnswerDetailsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAnswerDetailsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);
        
        [Fact]
        public async Task GetAnswerDetailsQueryHandler_Success()
        {
            var handler = new GetDetailsAnswerQueryHandler(context, mapper);

            var result = await handler.Handle(
                new GetDetailsAnswerQuery
                {
                    Id = Guid.Parse("8F6DBD71-5590-4796-BB25-64070C1A6B62"),
                }, CancellationToken.None);

            result.ShouldBeOfType<AnswerLookupDto>();
            result.Text.ShouldBe("Second answer for first course");
            result.IsCorrect.ShouldBe(false);
        }
    }
}
