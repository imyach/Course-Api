using Application.Common.Dtos.Answers;
using Application.Common.Dtos.Modules;
using Application.Common.Queries.Answers.GetAnswerList;
using Application.Common.Queries.Modules.GetModuleList;
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
    public class GetAllAnswersQueryHandlerTests
    {
        private readonly CoursesDbContext context;
        private readonly IMapper mapper;

        public GetAllAnswersQueryHandlerTests(QueryTestFixture fixture) => (context, mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetAnswersOfQuestionQueryHandler_Success()
        {
            var handler = new GetAllAnswerQueryHandler(context, mapper);

            var result = await handler.Handle(new GetAllAnswerQuery
            {
               QuestionId = Guid.Parse("893962C6-CB12-4D6C-A0B5-B3BB4669416F"),
            }, CancellationToken.None);

            result.ShouldBeOfType<AnswerListVm>();
            result.Answers.Count.ShouldBe(2);
        }
    }
}
