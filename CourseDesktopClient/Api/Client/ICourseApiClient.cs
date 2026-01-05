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
        Task<UserDto> GetUserProfileAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode> DeleteProfileAsync(CancellationToken ct = default);


        Task<(CoursesDto?, PagerInfoDto?)> GetCoursesAsync(int pageNumber = 1, int pageSize = 10, string searchText= null, CancellationToken ct = default);
        Task<CourseDto> GetCourseByIdAsync(Guid id, CancellationToken ct = default);


        Task<(ReviewsDto, PagerInfoDto)> GetReviewsAsync(Guid idCourse, int pageNumber = 1,  int pageSize = 20, CancellationToken ct = default);
        Task<ReviewDto> GetReviewByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateReviewAsync(ReviewDto reviewDto, CancellationToken ct = default);
        Task<HttpStatusCode> DeleteReviewAsync(Guid id, CancellationToken ct = default);
    }
}
