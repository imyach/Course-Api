using CourseDesktopClient.Api;
using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Api.Handlers;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;

namespace CourseDesktopClient.Services
{
    public class AuthService(HttpClient httpClient, ITokenService tokenService, ICourseApiClient apiClient, INavigationService navigationService) : IAuthService
    {
        public bool IsRememberProfile {  get; set; }
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

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            TokenHandler.isRemember = IsRememberProfile;
            var tokens = await apiClient.LoginAsync(loginDto);
            if (tokens is null)
                return "Данные введены неверно";
            await tokenService.SaveTokensAsync(tokens.AccessToken, tokens.RefreshToken, IsRememberProfile);
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

        public async Task<string> RegisterAsync(RegisterDto registerData)
        {
            TokenHandler.isRemember = IsRememberProfile;
            var tokens = await apiClient.RegisterAsync(registerData);

            if (tokens is null)
                return "Пользователь уже существует";

            await tokenService.SaveTokensAsync(tokens.AccessToken, tokens.RefreshToken, IsRememberProfile);
            var (accessToken, _) = await tokenService.GetTokensAsync();


            var currentUser = await tokenService.GetCurrentUserInfoAsync(accessToken);
            var userProfile = await apiClient.GetUserProfileAsync(Guid.Parse(currentUser.Id));

            CurrentUser = userProfile;
            isAuthenticated = true;

            navigationService.NavigateToCourses();
            return string.Empty;
        }
        public async Task DeleteProfile()
        {

            if (MessageBox.Show("Вы точно хотите удалить аккаунт?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await apiClient.DeleteProfileAsync();
                await LocalLogoutAsync();

                navigationService.NavigateToLogin();
            }
        }

        private async Task LocalLogoutAsync()
        {
                await httpClient.PostAsync("api/auth/logout", null);
            tokenService.ClearTokensAsync();
            CurrentUser = null;
            IsAuthenticated = false;
        }

        public async Task UpdateUserAsync(UpdateUserRequestDto userDto, CancellationToken ct = default)
        {
            if (MessageBox.Show("Вы действительно хотите изменить профиль?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var tokens = await apiClient.UpdateUserAsync(userDto, ct);

                if (tokens is not null) 
                {
                    await tokenService.SaveTokensAsync(tokens.AccessToken,tokens.RefreshToken,IsRememberProfile);

                    var (accessToken, _) = await tokenService.GetTokensAsync();

                    var currentUser = await tokenService.GetCurrentUserInfoAsync(accessToken);
                    var userProfile = await apiClient.GetUserProfileAsync(Guid.Parse(currentUser.Id));

                    CurrentUser = userProfile;
                }
            }
        }
    }
}
