using Application.Common.Commands.ProgressMaterials.DeleteProgressMaterials;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.ProgressMaterials
{
    public class DeleteProgressMaterialCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteProgressMaterialCommandHandler_Success()
        {
            var handler = new DeleteProgressMaterialCommandHandler(Context);

            await handler.Handle(new DeleteProgressMaterialCommand
            {
                Id = CoursesContextFactory.ProgressMaterialForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.Null(Context.ProgressMaterials.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.ProgressMaterialForDelete));
        }

        [Fact]
        public async Task DeleteProgressMaterialCommandHandler_AccessException()
        {
            var handler = new DeleteProgressMaterialCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteProgressMaterialCommand
                    {
                        Id = CoursesContextFactory.ProgressMaterialForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}
