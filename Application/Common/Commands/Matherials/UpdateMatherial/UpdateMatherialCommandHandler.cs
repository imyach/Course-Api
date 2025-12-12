using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Matherials.UpdateMatherial
{
    public class UpdateMatherialCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateMatherialCommand>
    {
        public async Task<Unit> Handle(UpdateMatherialCommand request, CancellationToken cancellationToken)
        {

            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Matherials.FindAsync([request.Id], cancellationToken)
                ?? throw new NotFoundException(nameof(Matherial), request.Id);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && currentUser.Id == entity.Module.Course.UserId))
            {
                entity.Description = request.Description;
                entity.Order = request.Order;
                entity.Title = request.Title;
                entity.ModuleId = request.ModuleId;
                entity.Module.Course.UpdateAt = DateTime.Now;

                await context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
