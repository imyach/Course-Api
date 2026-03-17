using Application.Common.Commands.Questions.CreateQuestion;
using Application.Common.Commands.TestResults.CreateTestResults;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.TestResults
{
    public class CreateTestResultCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateTestResultCommandHandler_Success()
        {
            var handler = new CreateTestResultCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserStudent;
            Guid progressMaterialId = Guid.Parse("4C27AAF3-CB96-46B2-86D6-65A23D733CB1");
            Guid testId = Guid.Parse("F019CC79-026F-4572-8E27-79A73495A8E7");

            var testResultId = await handler.Handle(new CreateTestResultCommand
            {
                CurrentUserId = currentUserId,
                TestId = testId,
                ProgressMaterialId = progressMaterialId
            }, CancellationToken.None);

            Assert.NotNull(await Context.TestResults.SingleOrDefaultAsync(testResult =>
            testResult.Id == testResultId));
        }
    }
}
