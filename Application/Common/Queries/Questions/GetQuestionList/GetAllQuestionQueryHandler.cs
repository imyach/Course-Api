using Application.Common.Dtos.Questions;
using Application.Common.Dtos.Reviews;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Questions.GetQuestionList
{
    public class GetAllQuestionQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllQuestionQuery, QuestionListVm>
    {
        public async Task<QuestionListVm> Handle(GetAllQuestionQuery request, CancellationToken cancellationToken)
        {
            var questionQuery = await context.Questions
                .Include(m => m.Test)
                .ProjectTo<QuestionLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new QuestionListVm { Questions = questionQuery };
        }
    }
}
