using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Api.Handlers;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Services
{
    public class AuthService(ITokenService tokenService, ICourseApiClient apiClient, INavigationService navigationService) : IAuthService
    {
        private bool isAuthenticated;
        private UserDto currentUser;
        public bool IsAuthenticated
        {
            get => isAuthenticated;
            private set
            {
                if (isAuthenticated != value)
                {
                    isAuthenticated = value;
                    AuthenticationChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public UserDto CurrentUser
        {
            get => currentUser;
            private set
            {
                currentUser = value;
                UserChanged?.Invoke(this, value);
            }
        }

        public event EventHandler AuthenticationChanged;
        public event EventHandler<UserDto> UserChanged;

        public async Task<bool> CheckAuthOnStartupAsync()
        {
            
            if (!await tokenService.HasTokensAsync())
                return false;
                
            var(_, refresh) = await tokenService.GetTokensAsync();

            if (string.IsNullOrEmpty(refresh)) return false;


            var (accessToken, _) = await tokenService.GetTokensAsync();
            var currentUser = await tokenService.GetCurrentUserInfoAsync(accessToken);
            CurrentUser = await apiClient.GetUserProfileAsync(Guid.Parse(currentUser.Id));
            IsAuthenticated = true;

            return true;
        }

        public async Task<string> LoginAsync(LoginDto loginDto, bool isRememberMe)
        {
            TokenHandler.isRemember = isRememberMe;
            var tokens = await apiClient.LoginAsync(loginDto);
            if (tokens is null)
                return "Данные введены неверно";
            await tokenService.SaveTokensAsync(tokens.AccessToken, tokens.RefreshToken, isRememberMe);
            var (accessToken, refreshToken) = await tokenService.GetTokensAsync();

            var currentUser = await tokenService.GetCurrentUserInfoAsync(accessToken);
            var userProfile = await apiClient.GetUserProfileAsync(Guid.Parse(currentUser.Id));

            CurrentUser = userProfile;
            isAuthenticated = true;    

            navigationService.NavigateToCourses();
            return string.Empty;
        }

        public async Task LogoutAsync()
        {
            await apiClient.LogoutAsync();
            await tokenService.ClearTokensAsync();

            isAuthenticated = false;
            CurrentUser = null;

            navigationService.NavigateToLogin();
        }

        public async Task<string> RegisterAsync(RegisterDto registerData, bool isRememberMe)
        {
            TokenHandler.isRemember = isRememberMe;
            var tokens = await apiClient.RegisterAsync(registerData);
            await tokenService.SaveTokensAsync(tokens.AccessToken, tokens.RefreshToken, isRememberMe);

            var (accessToken, refreshToken) = await tokenService.GetTokensAsync();
            if (accessToken == null || refreshToken == null)
                return "Пользователь уже существует";

            var currentUser = await tokenService.GetCurrentUserInfoAsync(accessToken);
            var userProfile = await apiClient.GetUserProfileAsync(Guid.Parse(currentUser.Id));

            CurrentUser = userProfile;
            isAuthenticated = true;

            navigationService.NavigateToCourses();
            return string.Empty;
        }
    }
}
