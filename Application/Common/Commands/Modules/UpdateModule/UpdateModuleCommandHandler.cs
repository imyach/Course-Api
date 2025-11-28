using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.UpdateModule
{
    public class UpdateModuleCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateModuleCommand>
    {
        public async Task<Unit> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Modules.FindAsync([request.Id], cancellationToken);
            if (entity == null || entity.CurrentUserId != request.CurrentUserId) 
            {
                throw new NotFoundException(nameof(Module), request.Id);
            }
            entity.Title = request.Title;   
            entity.Description = request.Description;
            entity.Order = request.Order;
            entity.CourseId = request.CourseId;

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
