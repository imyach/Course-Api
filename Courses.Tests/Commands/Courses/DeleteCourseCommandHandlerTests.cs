using Application.Common.Commands.Courses.DeleteCourse;
using Application.Common.Commands.Users.DeteleUser;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Courses
{
    public class DeleteCourseCommandHandlerTests : TestCommandBase
    {

        [Fact]
        public async Task DeleteCourseCommandHandler_Success()
        {
            var handler = new DeleteCourseCommandHandler(Context);

            await handler.Handle(new DeleteCourseCommand
            {
                Id = CoursesContextFactory.CourseForDelete,
                CurrentUserId = CoursesContextFactory.UserCouch
            }, CancellationToken.None);

            Assert.NotNull(Context.Courses.SingleOrDefault(note =>
                note.Status == "Archived" &&
                note.Id == CoursesContextFactory.CourseForDelete));
        }

        [Fact]
        public async Task DeleteCourseCommandHandler_AccessException()
        {
            var handler = new DeleteCourseCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteCourseCommand
                    {
                        Id = CoursesContextFactory.CourseForDelete,
                        CurrentUserId = CoursesContextFactory.UserStudent
                    }, CancellationToken.None));
        }
    }
}