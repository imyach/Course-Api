using CourseDesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Interfaces
{
    public interface ITokenService
    {
        public  Task<CurrentUserInfo> GetCurrentUserInfoAsync(string token);
        public Task SaveTokensAsync(string access, string refresh, bool rememberMe);
        public  Task<(string? AccessToken, string? RefreshToken)> GetTokensAsync();
        public Task ClearTokensAsync();
        public  Task<bool> HasTokensAsync();
    }
}
