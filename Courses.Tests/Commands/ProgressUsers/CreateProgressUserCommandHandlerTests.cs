using Application.Common.Commands.ProgressUsers.CreateProgressUser;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.ProgressUsers
{
    public class CreateProgressUserCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateProgressUserCommandHandler_Success()
        {
            var handler = new CreateProgressUserCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserStudent;
            Guid courseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8");

            var progressUserId = await handler.Handle(new CreateProgressUserCommand
            {
                CurrentUserId = currentUserId,
                CourseId = courseId
            }, CancellationToken.None);

            progressUserId.ShouldBe(Guid.Empty);
        }
    }
}