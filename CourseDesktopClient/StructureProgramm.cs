using CourseDesktopClient.Api;
using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Api.Handlers;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Services;
using CourseDesktopClient.UI.Elements;
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
                var AllUsersPageVm = provider.GetRequiredService<AllUsersPageVm>();
                return new AllUsersPage { DataContext = AllUsersPageVm };
            });


            services.AddSingleton(provider =>
            {
                var mainVm = provider.GetRequiredService<MainWindowVm>();
                return new MainWindow { DataContext = mainVm };
            });
        }
    }
}
