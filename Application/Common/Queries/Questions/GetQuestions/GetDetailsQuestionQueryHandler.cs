using Application.Common.Dtos.Questions;
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

namespace Application.Common.Queries.Questions.GetQuestions
{
    public class GetDetailsQuestionQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsQuestionQuery, QuestionLookupDto>
    {
        public async Task<QuestionLookupDto> Handle(GetDetailsQuestionQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.Questions
                .Include(q => q.Test)
                    .ThenInclude(t => t.Matherial)
                    .ThenInclude(m => m.Module)
                    .ThenInclude(m => m.Course)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity == null || request.CurrentUserId != entity.CurrentUserId)
            {
                throw new NotFoundException(nameof(Question), request.Id);
            }

            return mapper.Map<QuestionLookupDto>(entity);
        }
    }
}
