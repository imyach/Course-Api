using Application.Common.Commands.Materials.DeleteMaterial;
using Application.Common.Commands.Modules.DeleteModule;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Materials
{
    public class DeleteMaterialCommandHandlerTest : TestCommandBase
    {
        [Fact]
        public async Task DeleteMaterialCommandHandler_Success()
        {
            var handler = new DeleteMaterialCommandHandler(Context);

            await handler.Handle(new DeleteMaterialCommand
            {
                Id = CoursesContextFactory.MaterialForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.Null(Context.Materials.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.MaterialForDelete));
        }

        [Fact]
        public async Task DeleteMaterialCommandHandler_AccessException()
        {
            var handler = new DeleteMaterialCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteMaterialCommand
                    {
                        Id = CoursesContextFactory.MaterialForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}

