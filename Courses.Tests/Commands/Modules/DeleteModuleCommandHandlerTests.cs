using Application.Common.Commands.Modules.DeleteModule;
using Application.Common.Commands.Users.DeteleUser;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Modules
{
    public class DeleteModuleCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteModuleCommandHandler_Success()
        {
            var handler = new DeleteModuleCommandHandler(Context);

            await handler.Handle(new DeleteModuleCommand
            {
                Id = CoursesContextFactory.ModuleForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.Null(Context.Modules.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.ModuleForDelete));
        }

        [Fact]
        public async Task DeleteModuleCommandHandler_AccessException()
        {
            var handler = new DeleteModuleCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteModuleCommand
                    {
                        Id = CoursesContextFactory.ModuleForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}
