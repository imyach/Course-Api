using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    class ProfilePageVm : NavigationVm
    {
        private readonly IAuthService authService;

        private string _userName = string.Empty;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value; 
                OnPropertyChanged();
            }
        }
            
        private string _userEmail = string.Empty;
        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value;
                OnPropertyChanged();}
        }

        private string _phoneNumber = string.Empty;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }
        private string _userLogin = string.Empty;
        public string UserLogin
        {
            get { return _userLogin; }
            set
            {
                _userLogin = value;
                OnPropertyChanged();
            }
        }
        private string _userRole = string.Empty;
        public string UserRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogOutCommand {  get; set; }
        public ICommand DeleteUserProfile {  get; set; }
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
        }

        public void LoadingProfilePage()
        {
            UserName = authService.CurrentUser.NameUser;
            UserEmail = authService.CurrentUser.Email;
            PhoneNumber = authService.CurrentUser.PhoneNumber;
            UserLogin = authService.CurrentUser.Login;
            UserRole = authService.CurrentUser.Role.Name;
        }
    }
}
