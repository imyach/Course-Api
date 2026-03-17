using Application.Common.Commands.TestResults.UpdateTestResults;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.TestResults
{
    public class UpdateTestResultCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateTestResultCommndHandler_Success()
        {
            var handler = new UpdateTestResultCommandHandler(Context);
            var score = 93;
            var isPassed = true;

            await handler.Handle(new UpdateTestResultCommand
            {
                Id = CoursesContextFactory.TestResultForUpdate,
                Score = score,
                IsPassed = isPassed,
                CurrentUserId = CoursesContextFactory.UserCouch,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.TestResults.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.TestResultForUpdate &&
                user.Score == score &&
                user.IsPassed == isPassed));
        }

        [Fact]
        public async Task UpdateTestResultCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateTestResultCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateTestResultCommand
                    {
                        Id = Guid.NewGuid(),
                        Score = 93,
                        IsPassed = true,
                        CurrentUserId = CoursesContextFactory.UserCouch,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateTestResultCommandHandler_AccessException()
        {
            var handler = new UpdateTestResultCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new UpdateTestResultCommand
                    {
                        Id = CoursesContextFactory.TestResultForUpdate,
                        Score = 93,
                        IsPassed = true,
                        CurrentUserId = CoursesContextFactory.UserStudent,
                    }, CancellationToken.None));
        }
    }
}
