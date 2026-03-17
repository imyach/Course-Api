using Application.Common.Commands.Courses.DeleteCourse;
using Application.Common.Commands.Courses.UpdateCourse;
using Application.Common.Commands.Users.UpdateUser;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Courses
{
    public class UpdateCourseCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateCourseCommndHandler_Success()
        {
            var handler = new UpdateCourseCommandHandler(Context);
            var updatedTitle = "Updated  title of course";
            var updatedDescription = "Updated desc of course";

            await handler.Handle(new UpdateCourseCommand
            {
                Id = CoursesContextFactory.CourseForUpdate,
                Title = updatedTitle,
                Description = updatedDescription,

                CurrentUserId = CoursesContextFactory.UserCouch,
            }, CancellationToken.None);

            Assert.NotNull(
                await Context.Courses.SingleOrDefaultAsync(user =>
                user.Id == CoursesContextFactory.CourseForUpdate &&
                user.Title == updatedTitle &&
                user.Description == updatedDescription));
        }

        [Fact]
        public async Task UpdateCourseCommandHandler_FailOrWrongId()
        {
            var handler = new UpdateCourseCommandHandler(Context);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateCourseCommand
                    {
                        Id = Guid.NewGuid(),
                        Title = "Updated title",
                        CurrentUserId = CoursesContextFactory.UserCouch,
                    }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task UpdateCourseCommandHandler_AccessException()
        {
            var handler = new UpdateCourseCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new UpdateCourseCommand
                    {
                        Id = CoursesContextFactory.CourseForUpdate,
                        Title = "Updated title",
                        CurrentUserId = CoursesContextFactory.UserStudent,
                    }, CancellationToken.None));
        }
    }
}
