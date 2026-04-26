using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Auth.RequestDto;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.Entities.RequestDto;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Models.DtosModel.Reports;
using CourseDesktopClient.Services;
using CourseDesktopClient.ViewModel;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Printing;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Windows.Media.Media3D;
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

        //ROLES
        public async Task<RolesDto> GetRolesAsync(CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_ROLES, ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RolesDto>(jsonOptions, ct);
        }


        //COURSES
        public async Task<Guid?> CreateCourseAsync(CourseDto courseDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_COURSE, courseDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

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

        public async Task<(CoursesDto?, PagerInfoDto?)> GetCreatedCoursesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_CREATED_COURSE + $"?pageNumber={pageNumber}&pageSize={pageSize}", ct);
            if (!CheckOnAvalibleSever(response))
                return (null, null);

            response.EnsureSuccessStatusCode();
            var dataArray = await response.Content.ReadFromJsonAsync<JsonElement[]>(jsonOptions, ct);
            var courses = JsonSerializer.Deserialize<CoursesDto>(dataArray[0]);
            var pagerInfo = JsonSerializer.Deserialize<PagerInfoDto>(dataArray[1]);

            return (courses, pagerInfo);
        }

        public async Task<HttpStatusCode?> UpdateCourseAsync(CourseDto courseDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_COURSE, courseDto , ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<HttpStatusCode?> DeleteCourseAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_COURSE + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
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

        public async Task<Guid?> CreateUserAsync(UserRequestDto userDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CREATE_USER, userDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteProfileAsync(Guid idUser, CancellationToken ct = default)
        {
            var (access, _) = await tokenService.GetTokensAsync();

            var request = new HttpRequestMessage(HttpMethod.Delete, ApiPaths.API_DELETE_UPDATE_CREATE_USER + $"/{idUser}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access);

            var response = await httpClient.SendAsync(request,ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<HttpStatusCode?> SendCodeEmailAsync(string userEmail, Guid userId, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsync(ApiPaths.API_SEND_RECOVERY_CODE + $"?userEmail={userEmail}&userId={userId}", null);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();

            return response.StatusCode;

        }
        public async Task<bool> SendNewPasswordOnEmailAsync(string code, Guid userId, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsync(ApiPaths.API_SEND_NEW_PASSWORD + $"?code={code}&userId={userId}", null);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<bool>(jsonOptions, ct);
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

        public async Task<HttpStatusCode?> UpdateUserForAdminAsync(UpdateUserRequestDto userDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_UPDATE_USER_ADMIN, userDto, ct);

            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
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

        public async Task<UsersDto?> GetUsersByEmailAsync(string email, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_USER_BY_EMAIL + $"?email={email}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UsersDto?>(jsonOptions, ct);
        }


        //MODULE
        public async Task<ModulesDto?> GetModulesAsync(Guid courseId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_MODULE + $"/{courseId}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<ModulesDto?>(jsonOptions, ct);
        }

        public async Task<ModuleDto?> GetModuleAsync(Guid moduleId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_MODULE_BY_ID + moduleId, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<ModuleDto?>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteModuleAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_MODULE + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
        }

        public async Task<Guid?> CreateModuleAsync(ModuleDto moduleDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_MODULE, moduleDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> UpdateModuleAsync(ModuleDto moduleDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_MODULE, moduleDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
        }


        //MATERIAL
        public async Task<MaterialsDto?> GetMaterialsAsync(Guid moduleId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_MATERIAL + $"/{moduleId}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<MaterialsDto?>(jsonOptions, ct);
        }

        public async Task<MaterialDto?> GetMaterialAsync(Guid materialId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_MATERIAL_BY_ID + materialId, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<MaterialDto?>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteMaterialAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_MATERIAL + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
        }

        public async Task<Guid?> CreateMaterialAsync(MaterialDto materialDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_MATERIAL, materialDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> UpdateMaterialAsync(MaterialDto materialDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_MATERIAL, materialDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
        }


        //TEST
        public async Task<TestsDto?> GetTestsAsync(Guid materialId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_TEST + $"/{materialId}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<TestsDto?>(jsonOptions, ct);
        }

        public async Task<TestDto?> GetTestAsync(Guid testId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_TEST_BY_ID + testId, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<TestDto?>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteTestAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_TEST + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
        }

        public async Task<Guid?> CreateTestAsync(TestDto testDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_TEST, testDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> UpdateTestAsync(TestDto testDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_TEST, testDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
        }


        //QUESTION
        public async Task<QuestionsDto?> GetQuestionsAsync(Guid testId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_QUESTION + $"/{testId}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<QuestionsDto?>(jsonOptions, ct);
        }

        public async Task<QuestionDto?> GetQuestionAsync(Guid questionId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_QUESTION_BY_ID + questionId, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<QuestionDto?>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteQuestionAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_QUESTION + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
        }

        public async Task<Guid?> CreateQuestionAsync(QuestionDto questionDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_QUESTION, questionDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> UpdateQuestionAsync(QuestionDto questionDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_QUESTION, questionDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
        }


        //ANSWER
        public async Task<AnswersDto?> GetAnswersAsync(Guid questionId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_ANSWER + $"/{questionId}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<AnswersDto?>(jsonOptions, ct);
        }

        public async Task<AnswerDto?> GetAnswerAsync(Guid answerId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ANSWER_BY_ID + answerId, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<AnswerDto?>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteAnswerAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_ANSWER + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
        }

        public async Task<Guid?> CreateAnswerAsync(AnswerDto answerDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_ANSWER, answerDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> UpdateAnswerAsync(AnswerDto answerDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_ANSWER, answerDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            return response.StatusCode;
        }


        private bool CheckOnAvalibleSever(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                return false;
            return true;
        }

        //PROGRESSUSER
        public async Task<(ProgressUsersDto, ProgerssInfoDto, PagerInfoDto)> GetProgressUsersAsync(Guid userId, int pageNumber = 1, int pageSize = 20, string searchText = null, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_PROGRESSUSER + $"{userId}?searchText={searchText}&pageSize={pageSize}&pageNumber={pageNumber}", ct);

            if (!CheckOnAvalibleSever(response))
                return (null, null, null);
            response.EnsureSuccessStatusCode();

            var dataArray = await response.Content.ReadFromJsonAsync<JsonElement[]>(jsonOptions, ct);
            var progressUsers = JsonSerializer.Deserialize<ProgressUsersDto>(dataArray[0]);
            var progressInfo = JsonSerializer.Deserialize<ProgerssInfoDto>(dataArray[1]);
            var pagerInfo = JsonSerializer.Deserialize<PagerInfoDto>(dataArray[2]);

            return (progressUsers, progressInfo, pagerInfo);
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


        //PROGRESSMODULE
        public async Task<ProgressModulesDto?> GetProgressModulesAsync(Guid progressUserId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_PROGRESSMODULE + progressUserId, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProgressModulesDto>(jsonOptions, ct);
        }

        public async Task<Guid?> CreateProgressModuleAsync(ProgressModuleDto progressModuleDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_PROGRESSMODULE, progressModuleDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

        public async Task<ProgressModuleDto?> GetProgressModuleByIdAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_PROGRESSMODULE_BY_ID + $"{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProgressModuleDto>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteProgressModuleAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_PROGRESSMODULE + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<HttpStatusCode?> UpdateProgressModuleAsync(ProgressModuleDto progressModuleDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_PROGRESSMODULE, progressModuleDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        //PROGRESSMATERIAL
        public async Task<ProgressMaterialsDto?> GetProgressMaterialsAsync(Guid progressModuleId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_PROGRESSMATERIAL + progressModuleId, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProgressMaterialsDto>(jsonOptions, ct);
        }

        public async Task<Guid?> CreateProgressMaterialAsync(ProgressMaterialDto progressMaterialDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_PROGRESSMATERIAL, progressMaterialDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

        public async Task<ProgressMaterialDto?> GetProgressMaterialByIdAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_PROGRESSMATERIAL_BY_ID + $"{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProgressMaterialDto>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteProgressMaterialAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_PROGRESSMATERIAL + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<HttpStatusCode?> UpdateProgressMaterialAsync(ProgressMaterialDto progressMaterialDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_PROGRESSMATERIAL, progressMaterialDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        //TESTRESULT
        public async Task<TestResultsDto?> GetTestResultsAsync(Guid progressMaterialId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_TESTRESULT + progressMaterialId, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TestResultsDto>(jsonOptions, ct);
        }

        public async Task<Guid?> CreateTestResultAsync(TestResultDto testResultDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_TESTRESULT, testResultDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>(jsonOptions, ct);
        }

        public async Task<TestResultDto?> GetTestResultByIdAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_TESTRESULT_BY_ID + $"{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TestResultDto>(jsonOptions, ct);
        }

        public async Task<HttpStatusCode?> DeleteTestResultAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_TESTRESULT + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<HttpStatusCode?> UpdateTestResultAsync(TestResultDto testResultDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_TESTRESULT, testResultDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        //ANSWERSUSER
        public async Task<TestHistoryVm?> GetAnswersUsersAsync(Guid testResultsId, CancellationToken ct = default)
        {
            var response = await httpClient.GetAsync(ApiPaths.API_GET_ALL_ANSWERSUSER + testResultsId, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TestHistoryVm>(jsonOptions, ct);
        }

        public async Task<CompleteTestResponseDto?> CompleteTestAsync(CompleteTestRequestDto request, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_ANSWERSUSER, request, ct);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                Console.WriteLine($"Ошибка при завершении теста: {response.StatusCode} - {error}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<CompleteTestResponseDto?>(cancellationToken: ct);
            return result;
        }

        public async Task<HttpStatusCode?> DeleteAnswersUserAsync(Guid id, CancellationToken ct = default)
        {
            var response = await httpClient.DeleteAsync(ApiPaths.API_DELETE_UPDATE_CERATE_ANSWERSUSER + $"/{id}", ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<HttpStatusCode?> UpdateAnswersUserAsync(AnswersUserDto answersUserDto, CancellationToken ct = default)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPaths.API_DELETE_UPDATE_CERATE_ANSWERSUSER, answersUserDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        //REPORTS
        public async Task<UserReportReusltDto?> GenerateUsersReport(UserReportDto userReportDto, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPaths.API_GENERATE_USER_REPORT, userReportDto, ct);
            if (!CheckOnAvalibleSever(response))
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserReportReusltDto?>(ct);
        }
    }
}
