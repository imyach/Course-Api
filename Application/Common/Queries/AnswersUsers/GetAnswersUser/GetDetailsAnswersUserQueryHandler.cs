using Application.Common.Dtos.Answers;
using Application.Common.Dtos.AnswersUsers;
using Application.Common.Exceptions;
using Application.Common.Queries.Answers.GetAnswer;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.AnswersUsers.GetAnswersUser
{
    public class GetDetailsAnswersUserQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsAnswersUserQuery, AnswersUserLookupDto>
    {
        public async Task<AnswersUserLookupDto> Handle(GetDetailsAnswersUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entities = await context.AnswersUsers
                .Include(a => a.Answer)
                    .ThenInclude(a => a.Question)
                    .ThenInclude(q => q.Test)
                    .ThenInclude(t => t.Material)
                    .ThenInclude(m => m.Module)
                    .ThenInclude(m => m.Course)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(a => a.User)
                    .ThenInclude(u => u.Role)
                .ToListAsync(cancellationToken);


            if (roleUser.Name == "Admin" || roleUser.Name == "Couch")
            {
                var entityA = entities.FirstOrDefault(c => c.Id == request.Id)
                    ?? throw new NotFoundException(nameof(AnswersUser), request.Id);

                return mapper.Map<AnswersUserLookupDto>(entityA);
            }


            var entity = entities.Where(x=> x.UserId == currentUser.Id)
                .FirstOrDefault(c => c.Id == request.Id)
                    ?? throw new NotFoundException(nameof(AnswersUser), request.Id);

            return mapper.Map<AnswersUserLookupDto>(entity);

        }
    }
}