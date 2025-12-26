using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CourseDesktopClient.Api.Client
{
    public class CourseApiClient : ICourseApiClient
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonOptions;
        private readonly ITokenService tokenService;

        public CourseApiClient(HttpClient httpClient, ITokenService tokenService )
        {
            this.tokenService = tokenService;
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<CourseDto> GetCourseByIdAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_COURSE_BY_ID + $"{id}", ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CourseDto>(jsonOptions, ct);
        }

        public async Task<(CoursesDto?, PagerInfoDto?)> GetCoursesAsync(int pageNumber, int pageSize = 10, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_COURSE + $"?pageNumber={pageNumber}&pageSize={pageSize}", ct);
            response.EnsureSuccessStatusCode();
            var dataArray =  await response.Content.ReadFromJsonAsync<JsonElement[]>(jsonOptions, ct);
            var courses =  JsonSerializer.Deserialize<CoursesDto>(dataArray[0]);
            var pagerInfo = JsonSerializer.Deserialize<PagerInfoDto>(dataArray[1]);

            return  (courses, pagerInfo);
        }

        public async Task<UserDto> GetUserProfileAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_USER + $"{id}", ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>(jsonOptions, ct);
        }

        public async Task<TokensDto> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_LOGIN_USER, loginDto, ct);
            if (response.StatusCode is HttpStatusCode.Unauthorized)
                return null;
            return await response.Content.ReadFromJsonAsync<TokensDto>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode> LogoutAsync(CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_LOGOUT_USER,(object?)null, ct);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<TokensDto> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_REGISTER_USER, registerDto, ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TokensDto>(jsonOptions, ct);
        }
    }
}
