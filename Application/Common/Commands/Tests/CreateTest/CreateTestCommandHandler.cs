using Application.Common.Dtos.Tests;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Tests.CreateTest
{
    public class CreateTestCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateTestCommand, Guid>
    {
        public async Task<Guid> Handle(CreateTestCommand request, CancellationToken cancellationToken)
        {
            var test = new Test
            {
                Id = Guid.NewGuid(),
                CousreId = request.CourseId,
                MatherialId = request.MatherialId,
                Title = request.Title,
                Description = request.Description,
            };

            await context.Tests.AddAsync(test, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return test.Id;
        }
    }
}
