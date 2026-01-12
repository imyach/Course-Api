using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CourseDesktopClient.Api.Client
{
    public interface ICourseApiClient
    {
        //AUTH
        Task<TokensDto> LoginAsync(LoginDto loginDto, CancellationToken ct = default);
        Task<TokensDto> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default);
        Task<HttpStatusCode> LogoutAsync(CancellationToken ct = default);

        //USER
        Task<UserDto> GetUserProfileAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode> DeleteProfileAsync(CancellationToken ct = default);
        Task<TokensDto> UpdateUserAsync(UpdateUserRequestDto userDto, CancellationToken ct = default);

        //COURSE
        Task<(CoursesDto?, PagerInfoDto?)> GetCoursesAsync(int pageNumber = 1, int pageSize = 10, string searchText= null, CancellationToken ct = default);
        Task<CourseDto> GetCourseByIdAsync(Guid id, CancellationToken ct = default);

        //REVIEW
        Task<(ReviewsDto, PagerInfoDto)> GetReviewsAsync(Guid idCourse, SortEnum sortBy = SortEnum.ByDate, bool sortAscending = false, int pageNumber = 1, int pageSize = 20, CancellationToken ct = default);
        Task<ReviewDto> GetReviewByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateReviewAsync(ReviewDto reviewDto, CancellationToken ct = default);
        Task<HttpStatusCode> DeleteReviewAsync(Guid id, CancellationToken ct = default);

        //PROGRESS USER
        Task<(ProgressUsersDto, PagerInfoDto)> GetProgressUsersAsync(int pageNumber = 1, int pageSize = 10, string searchText = null, CancellationToken ct = default);
        Task<Guid> CreateProgressUserAsync(ProgressUserDto progressUserDto, CancellationToken ct = default);
        Task<ProgressUserDto> GetProgressUserByIdAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode> DeleteProgressUserAsync(Guid id, CancellationToken ct = default);
    }
}
