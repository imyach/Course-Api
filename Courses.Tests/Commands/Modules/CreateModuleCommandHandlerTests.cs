
using Application.Common.Commands.Courses.CreateCourse;
using Application.Common.Commands.Modules.CreateModule;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Modules
{
    public class CreateModuleCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateModuleCommandHandler_Success()
        {
            var handler = new CreateModuleCommandhandler(Context);
            var currentUserId = CoursesContextFactory.UserCouch;
            string title = "Тест заголовок";
            string description = "Тест описание";
            Guid courseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8");

            var moduleId = await handler.Handle(new CreateModuleCommand
            {
                Title = title,
                Description = description,
                CurrentUserId = currentUserId,
                CourseId = courseId
            }, CancellationToken.None);

            Assert.NotNull(await Context.Modules.SingleOrDefaultAsync(module =>
            module.Id == moduleId &&
            module.Course.UserId == currentUserId &&
            module.Title == title &&
            module.CourseId == courseId &&
            module.Description == description));
        }

        [Fact]
        public async Task CreateModuleCommandHandler_AccessEcxeption()
        {
            var handler = new CreateModuleCommandhandler(Context);
            var currentUserId = CoursesContextFactory.UserStudent;
            string title = "Тест заголовок";
            string description = "Тест описание";
            Guid courseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8");


            await Assert.ThrowsAsync<AccessException>(async () =>
            { 
                await handler.Handle(new CreateModuleCommand
                {
                    Title = title,
                    Description = description,
                    CurrentUserId = currentUserId,
                    CourseId = courseId
                }, CancellationToken.None);
            });
        }
    }
}
