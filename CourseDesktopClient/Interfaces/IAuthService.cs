using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CourseDesktopClient.Interfaces
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }
        bool IsRememberProfile { get; set; }
        UserDto CurrentUser { get; }

        event EventHandler AuthenticationChanged;
        event EventHandler<UserDto> UserChanged;

        Task<bool> CheckAuthOnStartupAsync();
        Task<string> LoginAsync(LoginDto loginDto);
        Task<string> RegisterAsync(RegisterDto registerData);
        Task UpdateUserAsync(UpdateUserRequestDto userDto, CancellationToken ct = default);
        Task LogoutAsync();
        Task DeleteProfile();

    }
}
