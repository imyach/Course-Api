using Application.Common.Commands.Materials.CreateMaterial;
using Application.Common.Commands.Modules.CreateModule;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Materials
{
    public class CreateMaterialCommandHandlerTest : TestCommandBase
    {
        [Fact]
        public async Task CreateMaterialCommandHandler_Success()
        {
            var handler = new CreateMaterialCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserCouch;
            string title = "Тест заголовок";
            string description = "Тест описание";
            Guid moduleId = Guid.Parse("FED57263-8338-42FA-82F7-D5752D914CE1");

            var materialId = await handler.Handle(new CreateMaterialCommand
            {
                Title = title,
                Description = description,
                CurrentUserId = currentUserId,
                ModuleId = moduleId
            }, CancellationToken.None);

            Assert.NotNull(await Context.Materials.SingleOrDefaultAsync(material =>
            material.Id == materialId &&
            material.Module.Course.UserId == currentUserId &&
            material.Title == title &&
            material.ModuleId == moduleId &&
            material.Description == description));
        }

        [Fact]
        public async Task CreateMaterialCommandHandler_AccessEcxeption()
        {
            var handler = new CreateMaterialCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserStudent;
            string title = "Тест заголовок";
            string description = "Тест описание";
            Guid moduleId = Guid.Parse("FED57263-8338-42FA-82F7-D5752D914CE1");

            await Assert.ThrowsAsync<AccessException>(async () =>
            {
                await handler.Handle(new CreateMaterialCommand
                {
                    Title = title,
                    Description = description,
                    CurrentUserId = currentUserId,
                    ModuleId = moduleId
                }, CancellationToken.None);
            });
        }
    }
}
