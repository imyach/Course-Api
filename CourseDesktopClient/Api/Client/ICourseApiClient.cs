using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CourseDesktopClient.Api.Client
{
    public interface ICourseApiClient
    {
        Task<TokensDto> LoginAsync(LoginDto loginDto, CancellationToken ct = default);
        Task<TokensDto> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default);
        Task<HttpStatusCode> LogoutAsync(CancellationToken ct = default);

        Task<(CoursesDto?, PagerInfoDto?)> GetCoursesAsync(int pageNumber, int pageSize = 10, CancellationToken ct = default);
        Task<CourseDto> GetCourseByIdAsync(Guid id, CancellationToken ct = default);

        Task<UserDto> GetUserProfileAsync(Guid id, CancellationToken ct = default);
    }
}
