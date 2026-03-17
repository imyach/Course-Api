using Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.ProgressMaterials
{
    public class UpdateProgressMaterialCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateProgressMaterialCommndHandler_Success()
        {
            var handler = new UpdateProgressMaterialCommandHandler(Context);
            var updatedStatus = "Test status";

            await handler.Handle(new UpdateProgressMaterialCommand
            {
                Id = CoursesContextFactory.ProgressMaterialForUpdate,
                Status = updatedStatus,
                CurrentUserId = CoursesContextFactory.UserCouch,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.ProgressMaterials.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.ProgressMaterialForUpdate &&
                user.Status == updatedStatus));
        }

        [Fact]
        public async Task UpdateProgressMaterialCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateProgressMaterialCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateProgressMaterialCommand
                    {
                        Id = Guid.NewGuid(),
                        Status = "Updated status",
                        CurrentUserId = CoursesContextFactory.UserCouch,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateProgressMaterialCommandHandler_AccessException()
        {
            var handler = new UpdateProgressMaterialCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new UpdateProgressMaterialCommand
                    {
                        Id = CoursesContextFactory.ProgressMaterialForUpdate,
                        Status = "Updated status",
                        CurrentUserId = CoursesContextFactory.UserStudent,
                    }, CancellationToken.None));
        }
    }
}
