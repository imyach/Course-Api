using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Matherials.CreateMatherial
{
    public class CreateMatherialCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateMatherialCommand, Guid>
    {
        public async Task<Guid> Handle(CreateMatherialCommand request, CancellationToken cancellationToken)
        {
            var matherial = new Matherial
            {
                Id = Guid.NewGuid(),
                IdModule = request.IdModule,
                Title = request.Title,
                Description = request.Description,
                Order = request.Order,
            };

            await context.Matherials.AddAsync(matherial,cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return matherial.Id;
        }
    }
}
