using Application.Common.Dtos.Modules;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Model;
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
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            if (roleUser.RoleName == "Admin" || roleUser.RoleName == "Couch")
            {
                var progressQueryA = await context.ProgressUsers
                .Include(m => m.Course)
                .Include(m => m.User)
                .ProjectTo<ProgressUserLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

                return new ProgressUserListVm { ProgressUsers = progressQueryA };
            }

            var progressQuery = await context.ProgressUsers
                .Include(m => m.Course)
                .Include(m => m.User)
                .Where(m => m.UserId == currentUser.Id)
                .ProjectTo<ProgressUserLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ProgressUserListVm { ProgressUsers = progressQuery };
        }
    }
}
