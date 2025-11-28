using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.CreateModule
{
    public class CreateModuleCommandhandler(ICoursesDbContext context) : IRequestHandler<CreateModuleCommand, Guid>
    {
        public async Task<Guid> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
        {
            var module = new Module
            {
                Id = Guid.NewGuid(),
                CourseId = request.CourseId,
                Title = request.Title,
                Description = request.Description,
                Order = request.Order,
            };

            await context.Modules.AddAsync(module, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return module.Id;
        }
    }
}
