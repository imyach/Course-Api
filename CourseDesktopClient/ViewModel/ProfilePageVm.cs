using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    class ProfilePageVm : NavigationVm
    {
        private readonly IAuthService authService;

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                SetProperty(ref _userName, value); Update();
            }
        }
            
        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value;
                SetProperty(ref _userEmail, value); Update();
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                SetProperty(ref _phoneNumber, value); Update();
            }
        }
        private string _userLogin;
        public string UserLogin
        {
            get { return _userLogin; }
            set
            {
                _userLogin = value;
                SetProperty(ref _userLogin, value); Update();
            }
        }
        private string _userRole;
        public string UserRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                OnPropertyChanged();
            }
        }

        private bool _isEnable = false;
        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                _isEnable = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogOutCommand {  get; set; }
        public ICommand DeleteUserProfile {  get; set; }
        public ICommand UpdateUserProfile {  get; set; }
        public ProfilePageVm(INavigationService navigationService, IAuthService authService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            this.authService = authService;

            LogOutCommand = new RelayCommand(async _ =>
            {
                await authService.LogoutAsync();
            });


            DeleteUserProfile = new RelayCommand(async _ =>
            {
                await authService.DeleteProfile();
            });

            UpdateUserProfile = new RelayCommand(async _ =>
            {
                var userDto = new UpdateUserRequestDto
                {
                    Id = authService.CurrentUser.Id,
                    Login = UserLogin,
                    Email = UserEmail,
                    PhoneNumber = PhoneNumber,
                    NameUser = UserName,
                    Role = authService.CurrentUser.Role,
                };
                    await authService.UpdateUserAsync(userDto);
                    navigationService.NavigateToProfile();
            });
        }

        public void LoadingProfilePage()
        {
            UserName = authService.CurrentUser.NameUser;
            UserEmail = authService.CurrentUser.Email;
            PhoneNumber = authService.CurrentUser.PhoneNumber;
            UserLogin = authService.CurrentUser.Login;
            UserRole = authService.CurrentUser.Role.Name;
        }

        private void Update()
        {
            if(UserName == authService.CurrentUser.NameUser
                && UserEmail == authService.CurrentUser.Email
                && PhoneNumber == authService.CurrentUser.PhoneNumber
                && UserLogin == authService.CurrentUser.Login)
            {
                IsEnable = false;
            }
            else
                IsEnable = true;
        }
    }
}
