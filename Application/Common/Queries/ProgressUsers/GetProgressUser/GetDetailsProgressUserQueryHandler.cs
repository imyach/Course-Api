using Application.Common.Dtos.ProgressUsers;
using Application.Common.Dtos.Reviews;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressUsers.GetProgressUser
{
    public class GetDetailsProgressUserQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsProgressUserQuery, ProgressUserLookupDto>
    {
        public async Task<ProgressUserLookupDto> Handle(GetDetailsProgressUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entities = await context.ProgressUsers
            .Include(r => r.Course)
                .ThenInclude(c => c.User)
                .ThenInclude(u => u.Role)
            .Include(r => r.User)
                .ThenInclude(u => u.Role)
            .ToListAsync(cancellationToken);

            if (roleUser.Name == "Admin" || roleUser.Name == "Couch")
            {

                var entityA = entities.FirstOrDefault(r => r.Id == request.Id)
                    ?? throw new NotFoundException(nameof(ProgressUser), request.Id);

                return mapper.Map<ProgressUserLookupDto>(entityA);
            }

            var entity = entities.Where(e=>e.UserId == currentUser.Id)
                .FirstOrDefault(r => r.Id == request.Id)
                    ?? throw new NotFoundException(nameof(ProgressUser), request.Id);

            return mapper.Map<ProgressUserLookupDto>(entity);
        }
    }
}
