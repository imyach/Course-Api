using Application.Common.Dtos.Answers;
using Application.Common.Dtos.AnswersUsers;
using Application.Common.Exceptions;
using Application.Common.Queries.Answers.GetAnswerList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Model;
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

            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);


            if (roleUser.RoleName == "Admin" || roleUser.RoleName == "Couch")
            {
                var answersUserQueryA = await context.AnswersUsers
                    .Include(c => c.Answer)
                    .Include(c => c.User)
                    .ProjectTo<AnswersUserLookupDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return new AnswersUserListVm { AnswersUsers = answersUserQueryA };
            }

            var answersUserQuery = await context.AnswersUsers
                    .Include(c => c.Answer)
                    .Include(c => c.User)
                    .Where(c => c.UserId == currentUser.Id)
                    .ProjectTo<AnswersUserLookupDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            return new AnswersUserListVm { AnswersUsers = answersUserQuery };

        }
    }
}
