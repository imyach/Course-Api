using Application.Common.Dtos;
using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Users;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Users.GetUsersList
{
    public class GetAllUsersQueryHandler(IMapper mapper, ICoursesDbContext context) : IRequestHandler<GetAllUsersQuery, object[]>
    {
        public async Task<object[]> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var query = context.Users
               .Include(u => u.Role)
               .Where(x=>x.IsActive == true)
               .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x => x.NameUser.Contains(request.SearchText));
            }


            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var users = await query
                .OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<UserLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return
            [ new UsersListVm { Users = users },
              new PagerInfoDto{ TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber}
            ];
        }
    }
}
