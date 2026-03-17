using Application.Common.Commands.TestResults.DeleteTestResults;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.TestResults
{
    public class DeleteTestResultCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteTestResultCommandHandler_Success()
        {
            var handler = new DeleteTestResultCommandHandler(Context);

            await handler.Handle(new DeleteTestResultCommand
            {
                Id = CoursesContextFactory.TestResultForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.Null(Context.TestResults.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.TestResultForDelete));
        }

        [Fact]
        public async Task DeleteTestResultCommandHandler_AccessException()
        {
            var handler = new DeleteTestResultCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteTestResultCommand
                    {
                        Id = CoursesContextFactory.TestResultForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}
