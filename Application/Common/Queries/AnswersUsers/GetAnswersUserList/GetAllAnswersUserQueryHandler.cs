using Application.Common.Dtos.Answers;
using Application.Common.Dtos.AnswersUsers;
using Application.Common.Queries.Answers.GetAnswerList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.AnswersUsers.GetAnswersUserList
{
    public class GetAllAnswersUserQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllAnswersUserQuery, AnswersUserListVm>
    {
        public async Task<AnswersUserListVm> Handle(GetAllAnswersUserQuery request, CancellationToken cancellationToken)
        {
            var answersUserQuery = await context.AnswersUsers
                .Include(c => c.Answer)
                .Include(c => c.User)
                .Where(c => c.CurrentUserId == request.CurrentUserId)
                .ProjectTo<AnswersUserLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new AnswersUserListVm { AnswersUsers = answersUserQuery };
        }
    }
}
