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

            query = query.Where(m => m.UserId == request.UserId);

            var complited = await query.CountAsync(q => q.Status == "Завершен", cancellationToken);
            var inPassage = await query.CountAsync(q => q.Status == "В прохождении", cancellationToken);

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x => x.Course.Title.ToLower().Contains(request.SearchText.ToLower()) 
                || x.Course.Description.ToLower().Contains(request.SearchText.ToLower()) 
                || x.Course.User.NameUser.ToLower().Contains(request.SearchText.ToLower()));
            }

            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var progressUsers = await query.OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<ProgressUserLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);


            return [new ProgressUserListVm { ProgressUsers = progressUsers },
                        new ProgressInfo
                        {
                            CompletedCourse = complited,
                            CourseInPassage = inPassage
                        },
                        new PagerInfoDto {
                            TotalItems = totalItems,
                            TotalPages = totalPages,
                            PageSize = request.PageSize,
                            PageNumber = request.PageNumber
                        }];
        }
    }
}
