using Application.Common.Commands.Modules.CreateModule;
using Application.Common.Commands.Tests.CreateTest;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Tests
{
    public class CreateTestCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateTestCommandHandler_Success()
        {
            var handler = new CreateTestCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserCouch;
            string title = "Тест заголовок";
            string description = "Тест описание";
            Guid materialId = Guid.Parse("C411E9B9-B6B5-4AC5-8CE3-34BE991BC910");

            var testId = await handler.Handle(new CreateTestCommand
            {
                Title = title,
                Description = description,
                CurrentUserId = currentUserId,
                MaterialId = materialId
            }, CancellationToken.None);

            Assert.NotNull(await Context.Tests.SingleOrDefaultAsync(test =>
            test.Id == testId &&
            test.Material.Module.Course.UserId == currentUserId &&
            test.Title == title &&
            test.MaterialId == materialId &&
            test.Description == description));
        }

        [Fact]
        public async Task CreateTestCommandHandler_AccessEcxeption()
        {
            var handler = new CreateTestCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserStudent;
            string title = "Тест заголовок";
            string description = "Тест описание";
            Guid materialId = Guid.Parse("C411E9B9-B6B5-4AC5-8CE3-34BE991BC910");


            await Assert.ThrowsAsync<AccessException>(async () =>
            {
                await handler.Handle(new CreateTestCommand
                {
                    Title = title,
                    Description = description,
                    CurrentUserId = currentUserId,
                    MaterialId = materialId
                }, CancellationToken.None);
            });
        }
    }
}
