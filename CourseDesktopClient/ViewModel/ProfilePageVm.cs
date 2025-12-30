using CourseDesktopClient.Interfaces;
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

        public ICommand LogOutCommand {  get; set; }
        public ProfilePageVm(INavigationService navigationService, IAuthService authService) : base(navigationService)
        {
            this.authService = authService;

            LogOutCommand = new RelayCommand(async _ =>
            {
                await authService.LogoutAsync();
            });

            LoadingProfilePage();
        }

        private void LoadingProfilePage()
        {
            UserName = "Имя: " + authService.CurrentUser.NameUser;
            UserEmail = "Почта: " + authService.CurrentUser.Email;
            PhoneNumber = "Номер телефона: " + authService.CurrentUser.PhoneNumber;
            UserLogin = "Логин: " + authService.CurrentUser.Login;
        }
    }
}
