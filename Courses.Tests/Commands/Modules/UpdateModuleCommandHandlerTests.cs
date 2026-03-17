using Application.Common.Commands.Modules.UpdateModule;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Modules
{
    public class UpdateModuleCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateModuleCommndHandler_Success()
        {
            var handler = new UpdateModuleCommandHandler(Context);
            var updatedTitle = "Updated  title of Module";

            await handler.Handle(new UpdateModuleCommand
            {
                Id = CoursesContextFactory.ModuleForUpdate,
                Title = updatedTitle,
                CurrentUserId = CoursesContextFactory.UserCouch,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.Modules.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.ModuleForUpdate &&
                user.Title == updatedTitle));
        }

        [Fact]
        public async Task UpdateModuleCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateModuleCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateModuleCommand
                    {
                        Id = Guid.NewGuid(),
                        Title = "Updated title",
                        CurrentUserId = CoursesContextFactory.UserCouch,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateModuleCommandHandler_AccessException()
        {
            var handler = new UpdateModuleCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new UpdateModuleCommand
                    {
                        Id = CoursesContextFactory.ModuleForUpdate,
                        Title = "Updated title",
                        CurrentUserId = CoursesContextFactory.UserStudent,
                    }, CancellationToken.None));
            }
        }
    }
