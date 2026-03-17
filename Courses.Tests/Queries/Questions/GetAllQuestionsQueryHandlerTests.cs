using Application.Common.Dtos.Modules;
using Application.Common.Dtos.Questions;
using Application.Common.Queries.Modules.GetModuleList;
using Application.Common.Queries.Questions.GetQuestionList;
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
    public class GetAllQuestionsQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllQuestionsQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper); 
        
        [Fact]
        public async Task GetQuestionsOfTestQueryHandler_Success()
        {
            var handler = new GetAllQuestionQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllQuestionQuery
            {
                TestId = Guid.Parse("D5440AF1-AC4D-4DBA-A072-2A6B1A6F1FB7"),
            }, CancellationToken.None);

            result.ShouldBeOfType<QuestionListVm>();
            result.Questions.Count.ShouldBe(1);
        }
    }
}
