using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Auth.RequestDto;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.Entities.RequestDto;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Models.DtosModel.Reports;
using CourseDesktopClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CourseDesktopClient.Api.Client
{
    public interface ICourseApiClient
    {
        //ROLE
        Task<RolesDto> GetRolesAsync(CancellationToken ct = default);

        //AUTH
        Task<TokensDto> LoginAsync(LoginDto loginDto, CancellationToken ct = default);
        Task<TokensDto> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default);
        Task<HttpStatusCode?> LogoutAsync(CancellationToken ct = default);
        Task<HttpStatusCode?> SendCodeEmailAsync(string userEmail, Guid userId, CancellationToken ct = default);
        Task<bool> SendNewPasswordOnEmailAsync(string code, Guid userId, CancellationToken ct = default);

        //USER
        Task<UserDto> GetUserProfileAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteProfileAsync(Guid idUser, CancellationToken ct = default);
        Task<Guid?> CreateUserAsync(UserRequestDto userDto, CancellationToken ct = default);
        Task<TokensDto?> UpdateUserAsync(UpdateUserRequestDto userDto, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateUserForAdminAsync(UpdateUserRequestDto userDto, CancellationToken ct = default);
        Task<(UsersDto, PagerInfoDto)> GetUsersAsync(int pageNumber = 1, int pageSize = 20, string searchText = null, CancellationToken ct = default);
        Task<UsersDto?> GetUsersByEmailAsync(string email, CancellationToken ct = default);

        //COURSE
        Task<(CoursesDto?, PagerInfoDto?)> GetCoursesAsync(int pageNumber = 1, int pageSize = 10, string searchText= null, CancellationToken ct = default);
        Task<(CoursesDto?, PagerInfoDto?)> GetCreatedCoursesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
        Task<Guid?> CreateCourseAsync(CourseDto courseDto, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateCourseAsync(CourseDto courseDto, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteCourseAsync(Guid id, CancellationToken ct = default);
        Task<CourseDto> GetCourseByIdAsync(Guid id, CancellationToken ct = default);

        //REVIEW
        Task<(ReviewsDto, PagerInfoDto)> GetReviewsAsync(Guid idCourse, SortEnum sortBy = SortEnum.ByDate, bool sortAscending = false, int pageNumber = 1, int pageSize = 20, CancellationToken ct = default);
        Task<ReviewDto> GetReviewByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid?> CreateReviewAsync(ReviewDto reviewDto, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteReviewAsync(Guid id, CancellationToken ct = default);

        //MODULE
        Task<ModulesDto?> GetModulesAsync(Guid courseId, CancellationToken ct = default);
        Task<ModuleDto?> GetModuleAsync(Guid moduleId, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteModuleAsync(Guid id, CancellationToken ct = default);
        Task<Guid?> CreateModuleAsync(ModuleDto moduleDto, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateModuleAsync(ModuleDto moduleDto, CancellationToken ct = default);

        //MATERIAL
        Task<MaterialsDto?> GetMaterialsAsync(Guid moduleId, CancellationToken ct = default);
        Task<MaterialDto?> GetMaterialAsync(Guid materialId, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteMaterialAsync(Guid id, CancellationToken ct = default);
        Task<Guid?> CreateMaterialAsync(MaterialDto materialDto, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateMaterialAsync(MaterialDto materialDto, CancellationToken ct = default);

        //TEST
        Task<TestsDto?> GetTestsAsync(Guid materialId, CancellationToken ct = default);
        Task<TestDto?> GetTestAsync(Guid testId, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteTestAsync(Guid id, CancellationToken ct = default);
        Task<Guid?> CreateTestAsync(TestDto testDto, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateTestAsync(TestDto testDto, CancellationToken ct = default);

        //QUESTION
        Task<QuestionsDto?> GetQuestionsAsync(Guid testId, CancellationToken ct = default);
        Task<QuestionDto?> GetQuestionAsync(Guid questionId, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteQuestionAsync(Guid id, CancellationToken ct = default);
        Task<Guid?> CreateQuestionAsync(QuestionDto questionDto, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateQuestionAsync(QuestionDto questionDto, CancellationToken ct = default);

        //ANSWER
        Task<AnswersDto?> GetAnswersAsync(Guid questionId, CancellationToken ct = default);
        Task<AnswerDto?> GetAnswerAsync(Guid answerId, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteAnswerAsync(Guid id, CancellationToken ct = default);
        Task<Guid?> CreateAnswerAsync(AnswerDto answerDto, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateAnswerAsync(AnswerDto answerDto, CancellationToken ct = default);

        //PROGRESS USER
        Task<(ProgressUsersDto, ProgerssInfoDto, PagerInfoDto)> GetProgressUsersAsync(Guid userId, int pageNumber = 1, int pageSize = 10, string searchText = null, CancellationToken ct = default);
        Task<Guid?> CreateProgressUserAsync(ProgressUserRequestDto progressUserDto, CancellationToken ct = default);
        Task<ProgressUserDto> GetProgressUserByIdAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteProgressUserAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateProgressUserAsync(ProgressUserRequestDto progressUserDto, CancellationToken ct = default);

        //PROGRESS MODULE
        Task<ProgressModulesDto?> GetProgressModulesAsync(Guid progressUserId, CancellationToken ct = default);
        Task<Guid?> CreateProgressModuleAsync(ProgressModuleDto progressModuleDto, CancellationToken ct = default);
        Task<ProgressModuleDto?> GetProgressModuleByIdAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteProgressModuleAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateProgressModuleAsync(ProgressModuleDto progressModuleDto, CancellationToken ct = default);

        //PROGRESS MATERIAL
        Task<ProgressMaterialsDto?> GetProgressMaterialsAsync(Guid progressModuleId, CancellationToken ct = default);
        Task<Guid?> CreateProgressMaterialAsync(ProgressMaterialDto progressMaterialDto, CancellationToken ct = default);
        Task<ProgressMaterialDto?> GetProgressMaterialByIdAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteProgressMaterialAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateProgressMaterialAsync(ProgressMaterialDto progressMaterialDto, CancellationToken ct = default);

        //TEST RESULT
        Task<TestResultsDto?> GetTestResultsAsync(Guid progressMaterialId, CancellationToken ct = default);
        Task<Guid?> CreateTestResultAsync(TestResultDto testResultDto, CancellationToken ct = default);
        Task<TestResultDto?> GetTestResultByIdAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteTestResultAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateTestResultAsync(TestResultDto testResultDto, CancellationToken ct = default);

        //ANSWER USER
        Task<TestHistoryVm?> GetAnswersUsersAsync(Guid testResultsId, CancellationToken ct = default);
        Task<CompleteTestResponseDto?> CompleteTestAsync(CompleteTestRequestDto request, CancellationToken ct = default);
        Task<HttpStatusCode?> DeleteAnswersUserAsync(Guid id, CancellationToken ct = default);
        Task<HttpStatusCode?> UpdateAnswersUserAsync(AnswersUserDto answersUserDto, CancellationToken ct = default);

        //REPORTS
        Task<UserReportReusltDto?> GenerateUsersReport(UserReportDto userReportDto,CancellationToken ct = default);
    }
}
