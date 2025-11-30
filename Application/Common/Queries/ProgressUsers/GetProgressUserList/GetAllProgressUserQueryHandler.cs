using Application.Common.Dtos.Modules;
using Application.Common.Dtos.ProgressUsers;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressUsers.GetProgressUserList
{
    public class GetAllProgressUserQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllProgressUserQuery, ProgressUserListVm>
    {
        public async Task<ProgressUserListVm> Handle(GetAllProgressUserQuery request, CancellationToken cancellationToken)
        {
            var progressQuery = await context.ProgressUsers
                .Include(m => m.Course)
                .Include(m => m.User)
                .Where(m => m.CurrentUserId == request.CurrentUserId)
                .ProjectTo<ProgressUserLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ProgressUserListVm {  ProgressUsers = progressQuery };
        }
    }
}
