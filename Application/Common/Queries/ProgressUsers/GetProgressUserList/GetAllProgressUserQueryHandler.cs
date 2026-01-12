using Application.Common.Dtos;
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
    public class GetAllProgressUserQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllProgressUserQuery, object[]>
    {
        public async Task<object[]> Handle(GetAllProgressUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var query = context.ProgressUsers
                .Include(m => m.Course)
                .Include(m => m.User).AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x => x.Course.Title.Contains(request.SearchText));
            }


            if (roleUser.RoleName == "Admin" || roleUser.RoleName == "Couch")
            {

                var totalItemsA = await query.CountAsync(cancellationToken);
                var totalPagesA = (int)Math.Ceiling(totalItemsA / (double)request.PageSize);

                var progressUsersA = await query.OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<ProgressUserLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

                return [new ProgressUserListVm { ProgressUsers = progressUsersA },
                        new PagerInfoDto { TotalItems = totalItemsA,
                            TotalPages = totalPagesA,
                            PageSize = request.PageSize,
                            PageNumber = request.PageNumber} ];
            }

            query = query.Where(m => m.UserId == currentUser.Id);

            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var progressUsers = await query.OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<ProgressUserLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);


            return [new ProgressUserListVm { ProgressUsers = progressUsers },
                        new PagerInfoDto { TotalItems = totalItems,
                            TotalPages = totalPages,
                            PageSize = request.PageSize,
                            PageNumber = request.PageNumber} ];
        }
    }
}
