using Application.Common.Commands.Matherials.DeleteMatherial;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Rewies.DeleteReview
{
    public class DeleteReviewCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteReviewCommand>
    {
        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {

            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Reviews.FindAsync([request.Id], cancellationToken)
                ?? throw new NotFoundException(nameof(Review), request.Id);

            if (roleUser.RoleName == "Admin" || entity.UserId == currentUser.Id)
            {
                context.Reviews.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}