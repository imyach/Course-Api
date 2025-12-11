using Application.Common.Dtos.Answers;
using Application.Common.Dtos.Courses;
using Application.Common.Queries.Courses.GetCourseList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Answers.GetAnswerList
{
    public class GetAllAnswerQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllAnswerQuery, AnswerListVm>
    {
        public async Task<AnswerListVm> Handle(GetAllAnswerQuery request, CancellationToken cancellationToken)
        {
            var answerQuery = await context.Answers
                .Include(c => c.Question)
                .ProjectTo<AnswerLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new AnswerListVm { Answers = answerQuery };
        }
    }
}
