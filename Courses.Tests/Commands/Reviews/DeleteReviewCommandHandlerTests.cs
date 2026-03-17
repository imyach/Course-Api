using Application.Common.Commands.Rewies.DeleteReview;
using Application.Common.Commands.Users.DeteleUser;
using Application.Common.Exceptions;
using Courses.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Commands.Reviews
{
    public class DeleteReviewCommandHandlerTests : TestCommandBase
    {

        [Fact]
        public async Task DeleteReviewCommandHandler_Success()
        {
            var handler = new DeleteReviewCommandHandler(Context);

            await handler.Handle(new DeleteReviewCommand
            {
                Id = CoursesContextFactory.ReviewForDelete,
                CurrentUserId = CoursesContextFactory.UserStudent
            }, CancellationToken.None);

            Assert.Null(Context.Reviews.SingleOrDefault(note =>
                note.Id == CoursesContextFactory.ReviewForDelete));
        }

        [Fact]
        public async Task DeleteReviewCommandHandler_AccessException()
        {
            var handler = new DeleteReviewCommandHandler(Context);

            await Assert.ThrowsAsync<AccessException>(async () =>
                await handler.Handle(
                    new DeleteReviewCommand
                    {
                        Id = CoursesContextFactory.ReviewForDelete,
                        CurrentUserId = CoursesContextFactory.UserCouch
                    }, CancellationToken.None));
        }
    }
}
