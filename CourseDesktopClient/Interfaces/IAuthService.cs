using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Interfaces
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }
        UserDto CurrentUser { get; }

        event EventHandler AuthenticationChanged;
        event EventHandler<UserDto> UserChanged;

        Task<bool> CheckAuthOnStartupAsync();
        Task<string> LoginAsync(LoginDto loginDto, bool isRememberMe);
        Task<string> RegisterAsync(RegisterDto registerData, bool isRememberMe);
        Task LogoutAsync();
        Task DeleteProfile();
    }
}
