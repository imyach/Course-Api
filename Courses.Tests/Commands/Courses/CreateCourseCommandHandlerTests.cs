using Application.Common.Commands.Courses.CreateCourse;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Courses
{
    public class CreateCourseCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateCourseCommandHandler_Success()
        {
            var handler = new CreateCourseCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserCouch;
            string title = "Тест заголовок";
            string description = "Тест описание";

            var courseId = await handler.Handle(new CreateCourseCommand
            {
                Title = title,
                Description = description,
                CurrentUserId = currentUserId
            }, CancellationToken.None);

            Assert.NotNull(await Context.Courses.SingleOrDefaultAsync(course => 
            course.Id == courseId &&
            course.UserId == currentUserId &&
            course.Title == title&&
            course.Description == description));
        }

        [Fact]
        public async Task CreateCourseCommandHandler_AccessEcxeption()
        {
            var handler = new CreateCourseCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserStudent;
            string title = "Тест заголовок";
            string description = "Тест описание";

            await Assert.ThrowsAsync<AccessException>(async () =>
            {
               await handler.Handle(new CreateCourseCommand 
                {
                    Title = title,
                    Description = description,
                    CurrentUserId = currentUserId
                }, CancellationToken.None);
            });
        }
    }
}
