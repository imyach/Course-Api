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
            var entity = await context.AnswersUsers
                .Include(a => a.Answer)
                    .ThenInclude(a => a.Question)
                    .ThenInclude(q => q.Test)
                    .ThenInclude(t => t.Matherial)
                    .ThenInclude(m => m.Module)
                    .ThenInclude(m => m.Course)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(a=> a.User)
                    .ThenInclude(u=> u.Role)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(AnswersUser), request.Id);
            }

            return mapper.Map<AnswersUserLookupDto>(entity);
        }
    }
}