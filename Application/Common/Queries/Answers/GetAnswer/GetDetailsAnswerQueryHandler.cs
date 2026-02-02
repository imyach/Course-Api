using Application.Common.Dtos.Answers;
using Application.Common.Dtos.Courses;
using Application.Common.Exceptions;
using Application.Common.Queries.Courses.GetCourse;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Answers.GetAnswer
{
    public class GetDetailsAnswerQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsAnswerQuery, AnswerLookupDto>
    {
        public async Task<AnswerLookupDto> Handle(GetDetailsAnswerQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.Answers
                .Include(a => a.Question)
                    .ThenInclude(q => q.Test)
                    .ThenInclude(t => t.Material)
                    .ThenInclude(m => m.Module)
                    .ThenInclude(m => m.Course)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(Answer), request.Id);

            return mapper.Map<AnswerLookupDto>(entity);
        }
    }
}
