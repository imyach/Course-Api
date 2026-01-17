using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Auth.RequestDto;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.Entities.RequestDto;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Services;
using CourseDesktopClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static CourseDesktopClient.ViewModel.CourseInformationPageVm;

namespace CourseDesktopClient.Api.Client
{
    public class CourseApiClient : ICourseApiClient
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonOptions;
        private readonly ITokenService tokenService;

        public CourseApiClient(HttpClient httpClient, ITokenService tokenService)
        {
            this.tokenService = tokenService;
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }


        //COURSES
        public async Task<CourseDto> GetCourseByIdAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_COURSE_BY_ID + $"{id}", ct);
            if (!CheckOnAvalibleSever(response))
                
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CourseDto>(jsonOptions, ct);
        }

        public async Task<(CoursesDto?, PagerInfoDto?)> GetCoursesAsync(int pageNumber=1, int pageSize = 10, string searchText = null, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_COURSE + $"?pageNumber={pageNumber}&pageSize={pageSize}&searchText={searchText}", ct);
            if (!CheckOnAvalibleSever(response))
                return (null, null);

            response.EnsureSuccessStatusCode();
            var dataArray =  await response.Content.ReadFromJsonAsync<JsonElement[]>(jsonOptions, ct);
            var courses =  JsonSerializer.Deserialize<CoursesDto>(dataArray[0]);
            var pagerInfo = JsonSerializer.Deserialize<PagerInfoDto>(dataArray[1]);

            return  (courses, pagerInfo);
        }


        //PROFILES
        public async Task<UserDto> GetUserProfileAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_USER_BY_ID + $"{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>(jsonOptions, ct);
        }

        public async Task<TokensDto> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_LOGIN_USER, loginDto, ct);
            if (response.StatusCode is HttpStatusCode.Unauthorized)
                return null;
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<TokensDto>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> LogoutAsync(CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_LOGOUT_USER,(object?)null, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<TokensDto> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_REGISTER_USER, registerDto, ct);
            if (response.StatusCode is HttpStatusCode.Unauthorized)
                return null;
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<TokensDto>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteProfileAsync(CancellationToken ct = default)
        {
            var (access, _) = await tokenService.GetTokensAsync();

            var request = new HttpRequestMessage(HttpMethod.Delete, ApiPaths.API_DELETE_UPDATE_CREATE_USER);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access);

            var response = await httpClient.SendAsync(request,ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }


        //REVIEWS
        public async Task<ReviewDto> GetReviewByIdAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_REVIEW_BY_ID + $"{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReviewDto>(jsonOptions, ct);
        }

        public async Task<(ReviewsDto, PagerInfoDto)> GetReviewsAsync(Guid idCourse, SortEnum sortBy = SortEnum.ByDate, bool sortAscending = false, int pageNumber=1, int pageSize = 20,CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_REVIEWS + $"?pageNumber={pageNumber}&pageSize={pageSize}&idCourse={idCourse}&sortBy={sortBy}&sortAscending={sortAscending}", ct);
            if (!CheckOnAvalibleSever(response))
                return (null, null);
            response.EnsureSuccessStatusCode();

            var dataArray = await response.Content.ReadFromJsonAsync<JsonElement[]>(jsonOptions, ct);
            var reviews = JsonSerializer.Deserialize<ReviewsDto>(dataArray[0]);
            var pagerInfo = JsonSerializer.Deserialize<PagerInfoDto>(dataArray[1]);

            return (reviews, pagerInfo);
        }
        public async Task<Guid?> CreateReviewAsync(ReviewDto reviewDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CREATE_REVIEW, reviewDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteReviewAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CREATE_REVIEW + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }


        //PROGRESSUSER
        public async Task<(ProgressUsersDto,PagerInfoDto)> GetProgressUsersAsync(int pageNumber = 1, int pageSize = 20, string searchText = null, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_PROGRESSUSER + $"?searchText={searchText}&pageSize={pageSize}&pageNumber={pageNumber}", ct);

            if (!CheckOnAvalibleSever(response))
                return (null,null);
            response.EnsureSuccessStatusCode();

            var dataArray = await response.Content.ReadFromJsonAsync<JsonElement[]>(jsonOptions, ct);
            var progressUsers = JsonSerializer.Deserialize<ProgressUsersDto>(dataArray[0]);
            var pagerInfo = JsonSerializer.Deserialize<PagerInfoDto>(dataArray[1]);

            return (progressUsers, pagerInfo);
        }
        public async Task<Guid?> CreateProgressUserAsync(ProgressUserRequestDto progressUserDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_PROGRESSUSER, progressUserDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }
        public async Task<ProgressUserDto> GetProgressUserByIdAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_PROGRESSUSER_BY_ID + $"{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProgressUserDto>(jsonOptions, ct);
        }
        public async Task<HttpStatusCode?> DeleteProgressUserAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_PROGRESSUSER + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }
        public async Task<HttpStatusCode?> UpdateProgressUserAsync(ProgressUserRequestDto progressUserDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_PROGRESSUSER, progressUserDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }


        //USER
        public async Task<TokensDto?> UpdateUserAsync(UpdateUserRequestDto userDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CREATE_USER, userDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.NoContent)
                return null;
            return await response.Content.ReadFromJsonAsync<TokensDto?>(jsonOptions, ct);
        }
        public async Task<(UsersDto, PagerInfoDto)> GetUsersAsync(int pageNumber = 1, int pageSize = 20, string searchText = null, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_USERS + $"?searchText={searchText}&pageSize={pageSize}&pageNumber={pageNumber}", ct);
            if (!CheckOnAvalibleSever(response))
                return (null,null);
            response.EnsureSuccessStatusCode();

            var dataArray = await response.Content.ReadFromJsonAsync<JsonElement[]>(jsonOptions, ct);
            var users = JsonSerializer.Deserialize<UsersDto>(dataArray[0]);
            var pagerInfo = JsonSerializer.Deserialize<PagerInfoDto>(dataArray[1]);

            return (users, pagerInfo);
        }


        private bool CheckOnAvalibleSever(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                return false;
            return true;
        }
    }
}
