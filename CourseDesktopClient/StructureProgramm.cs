using CourseDesktopClient.Api;
using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Api.Handlers;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Services;
using CourseDesktopClient.UI.Elements;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.View;
using CourseDesktopClient.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Net.Http;
using System.Text;

namespace CourseDesktopClient
{
    public class StructureProgramm
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //SERVICES
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<INavigationService>( provider => new NavigationService(provider));
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IPagerService, PagerService>();

            // HTTP CLIENTS
            services.AddSingleton(provider =>
            {
                var tokenService = provider.GetRequiredService<ITokenService>();
                var navigationService = provider.GetRequiredService<INavigationService>();
                var handler = new TokenHandler(tokenService, navigationService)
                {
                    InnerHandler = new HttpClientHandler()
                };

                return new HttpClient(handler)
                {
                    BaseAddress = new Uri(ApiPaths.API_PATH),
                    Timeout = TimeSpan.FromSeconds(30)
                };
            });


            //API CLIENTS
            services.AddSingleton<ICourseApiClient, CourseApiClient>();

            // VIEW MODELS
            services.AddSingleton<MainWindowVm>();

            services.AddTransient<LoginPageVm>();
            services.AddTransient<CreateTestPageVm>();
            services.AddTransient<CompletingCoursePageVm>();
            services.AddTransient<CompletingMaterialPageVm>();
            services.AddTransient<CompletingTestResultPageVm>();
            services.AddTransient<CompletingModulePageVm>();
            services.AddTransient<CreateQuestionPageVm>();
            services.AddTransient<CreateMaterialPageVm>();
            services.AddTransient<CreateCoursePageVm>();
            services.AddTransient<CreateModulePageVm>();
            services.AddTransient<WorkshopPageVm>();
            services.AddTransient<MistakePageVm>();
            services.AddTransient<CourseInformationPageVm>();
            services.AddTransient<ProfilePageVm>();
            services.AddTransient<AllCoursePageVm>();
            services.AddTransient<AllUsersPageVm>();
            services.AddTransient<MyCoursePageVm>();
            services.AddTransient<UpdateUserPasswordPageVm>();
            services.AddTransient<RegisterPageVm>();


            //VIEWS

            services.AddTransient(provider =>
            {
                var loginVm = provider.GetRequiredService<LoginPageVm>();
                return new LoginPage { DataContext = loginVm };
            });

            services.AddTransient(provider =>
            {
                var mistakeVm = provider.GetRequiredService<MistakePageVm>();
                return new MistakePage { DataContext = mistakeVm };
            });

            services.AddTransient(provider =>
            {
                var updPassVm = provider.GetRequiredService<UpdateUserPasswordPageVm>();
                return new UpdateUserPasswordPage { DataContext = updPassVm };
            });

            services.AddTransient(provider =>
            {
                var profileVm = provider.GetRequiredService<ProfilePageVm>();
                return new ProfilePage { DataContext = profileVm };
            });

            services.AddTransient(provider =>
            {
                var coursesVm = provider.GetRequiredService<AllCoursePageVm>();
                return new AllCoursePage { DataContext = coursesVm };
            });

            services.AddTransient(provider =>
            {
                var registerVm = provider.GetRequiredService<RegisterPageVm>();
                return new RegisterPage { DataContext = registerVm };
            });

            services.AddTransient(provider =>
            {
                var courseDesktopPageVm = provider.GetRequiredService<CourseInformationPageVm>();
                return new CourseInformationPage { DataContext = courseDesktopPageVm };
            });
            services.AddTransient(provider =>
            {
                var myCoursePageVm = provider.GetRequiredService<MyCoursePageVm>();
                return new MyCoursePage { DataContext = myCoursePageVm };
            });
            services.AddTransient(provider =>
            {
                var allUsersPageVm = provider.GetRequiredService<AllUsersPageVm>();
                return new AllUsersPage { DataContext = allUsersPageVm };
            });
            services.AddTransient(provider =>
            {
                var workshopPageVm = provider.GetRequiredService<WorkshopPageVm>();
                return new WorkshopPage { DataContext = workshopPageVm };
            });
            services.AddTransient(provider =>
            {
                var createCoursePageVm = provider.GetRequiredService<CreateCoursePageVm>();
                return new CreateCoursePage { DataContext = createCoursePageVm };
            });
            services.AddTransient(provider =>
            {
                var createModulePageVm = provider.GetRequiredService<CreateModulePageVm>();
                return new CreateModulePage { DataContext = createModulePageVm };
            });
            services.AddTransient(provider =>
            {
                var createMaterialPageVm = provider.GetRequiredService<CreateMaterialPageVm>();
                return new CreateMaterialPage { DataContext = createMaterialPageVm };
            });
            services.AddTransient(provider =>
            {
                var createTestPageVm = provider.GetRequiredService<CreateTestPageVm>();
                return new CreateTestPage { DataContext = createTestPageVm };
            });
            services.AddTransient(provider =>
            {
                var createQuestionPageVm = provider.GetRequiredService<CreateQuestionPageVm>();
                return new CreateQuestionPage { DataContext = createQuestionPageVm };
            });
            services.AddTransient(provider =>
            {
                var completingCoursePageVm = provider.GetRequiredService<CompletingCoursePageVm>();
                return new CompletingCoursePage { DataContext = completingCoursePageVm };
            });
            services.AddTransient(provider =>
            {
                var completingModulePageVm = provider.GetRequiredService<CompletingModulePageVm>();
                return new CompletingModulePage { DataContext = completingModulePageVm };
            });
            services.AddTransient(provider =>
            {
                var completingMaterialPageVm = provider.GetRequiredService<CompletingMaterialPageVm>();
                return new CompletingMaterialPage { DataContext = completingMaterialPageVm };
            });
            services.AddTransient(provider =>
            {
                var completingTestResultPageVm = provider.GetRequiredService<CompletingTestResultPageVm>();
                return new CompletingTestResultPage { DataContext = completingTestResultPageVm };
            });


            services.AddSingleton(provider =>
            {
                var mainVm = provider.GetRequiredService<MainWindowVm>();
                return new MainWindow { DataContext = mainVm };
            });
        }
    }
}
