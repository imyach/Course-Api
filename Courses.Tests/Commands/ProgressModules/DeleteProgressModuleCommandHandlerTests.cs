using Application.Common.Commands.ProgressModules.DeleteProgressModules;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.ProgressModules
{
    public class DeleteProgressModuleCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteProgressModuleCommandHandler_Success()
        {
            var handler = new DeleteProgressModuleCommandHandler(Context);

            await handler.Handle(new DeleteProgressModuleCommand
            {
                Id = CoursesContextFactory.ProgressModuleForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.Null(Context.ProgressModules.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.ProgressModuleForDelete));
        }

        [Fact]
        public async Task DeleteProgressModuleCommandHandler_AccessException()
        {
            var handler = new DeleteProgressModuleCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteProgressModuleCommand
                    {
                        Id = CoursesContextFactory.ProgressModuleForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}
