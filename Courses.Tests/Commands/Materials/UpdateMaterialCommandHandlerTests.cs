using Application.Common.Commands.Materials.UpdateMaterial;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Materials
{
    public class UpdateMaterialCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateMaterialCommndHandler_Success()
        {
            var handler = new UpdateMaterialCommandHandler(Context);
            var updatedTitle = "Updated  title of Material";

            await handler.Handle(new UpdateMaterialCommand
            {
                Id = CoursesContextFactory.MaterialForUpdate,
                Title = updatedTitle,
                CurrentUserId = CoursesContextFactory.UserCouch,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.Materials.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.MaterialForUpdate &&
                user.Title == updatedTitle));
        }

        [Fact]
        public async Task UpdateMaterialCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateMaterialCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateMaterialCommand
                    {
                        Id = Guid.NewGuid(),
                        Title = "Updated title",
                        CurrentUserId = CoursesContextFactory.UserCouch,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateMaterialCommandHandler_AccessException()
        {
            var handler = new UpdateMaterialCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new UpdateMaterialCommand
                    {
                        Id = CoursesContextFactory.MaterialForUpdate,
                        Title = "Updated title",
                        CurrentUserId = CoursesContextFactory.UserStudent,
                    }, CancellationToken.None));
        }
    }
}
