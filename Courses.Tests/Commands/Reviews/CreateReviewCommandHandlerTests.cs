using Application.Common.Commands.Courses.CreateCourse;
using Application.Common.Commands.Rewies.CreateReview;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Reviews
{
    public class CreateReviewCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateReviewCommandHandler_Success()
        {
            var handler = new CreateReviewCommandHandler(Context);
            var currentUserId = CoursesContextFactory.UserCouch;
            string text = "Тест";
            int rait = 4;

            var reviewId = await handler.Handle(new CreateReviewCommand
            {
                CourseId = Guid.Parse("C6D49ACE-09A4-4ADB-BB4B-A1267F5173A8"),
                Rait = rait,    
                Text = text,
                CurrentUserId = currentUserId
            }, CancellationToken.None);

            Assert.NotNull(await Context.Reviews.SingleOrDefaultAsync(review =>
            review.Id == reviewId &&
            review.UserId == currentUserId &&
            review.Text == text &&
            review.Rait == rait));
        }
    }
}
