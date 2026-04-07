
using CourseDesktopClient.Api;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Utilities;
using CourseDesktopClient.View;
using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class LoginPageVm : NavigationVm
    {
        private readonly IAuthService authService;
        public ICommand SignInCommand { get; set; }

        private string _userPasswordText = string.Empty;
        public string  UserPasswordText
        {
            get { return _userPasswordText; }
            set { _userPasswordText = value; SetProperty(ref _userPasswordText, value); }
        }
        private string _misstakeText = string.Empty;
        public string MisstakeText
        {
            get { return _misstakeText; }
            set { _misstakeText = value; OnPropertyChanged();}
        }

        private Visibility? _visibleMisstake = Visibility.Collapsed;
        public Visibility? VisibleMisstake 
        {

            get { return _visibleMisstake; }
            set { _visibleMisstake = value; OnPropertyChanged();}
        } 

        private string _userLoginText = string.Empty;
        public string UserLoginText
        {
            get { return _userLoginText; }
            set { _userLoginText = value; SetProperty(ref _userLoginText, value); }
        }

        public bool _checkedSaveUser;
        public bool CheckedSaveUser
        {
            get { return _checkedSaveUser; }
            set { _checkedSaveUser = value; SetProperty(ref _checkedSaveUser, value); }
        }

        private bool FillingVerificationLogin(string? loginOrEmail, string? password)
        {
            if (string.IsNullOrEmpty(loginOrEmail) || string.IsNullOrEmpty(password))
            {
                MisstakeText = "Заполните все поля";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (loginOrEmail.Length < 5)
            {
                MisstakeText = "Логин не может быть менее 5 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (password.Length < 5)
            {
                MisstakeText = "Пароль не может быть менее 5 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (loginOrEmail.Length>30)
            {
                MisstakeText = "Логин не может быть больше 30 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (password.Length > 30)
            {
                MisstakeText = "Пароль не может быть больше 30 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            MisstakeText = string.Empty;
            VisibleMisstake = Visibility.Collapsed;
            return true;
        }

        public async Task Login(string? loginOrEmail, string? password)
        {
            authService.IsRememberProfile = CheckedSaveUser;
            if (!FillingVerificationLogin(loginOrEmail, password))
                return;
            
            var loginDto = new LoginDto
            {
                Login = loginOrEmail,
                Password = password
            };

            var mistakeText = await authService.LoginAsync(loginDto);

            switch (string.IsNullOrEmpty(mistakeText))
            {
                case true:
                    UserLoginText = string.Empty;
                    UserPasswordText = string.Empty;
                    MisstakeText = string.Empty;
                    VisibleMisstake = Visibility.Collapsed;
                    CheckedSaveUser = false;
                    break;
                case false:
                    MisstakeText = mistakeText;
                    VisibleMisstake = Visibility.Visible;
                    break;
            }
        }

       public LoginPageVm(IAuthService authService, INavigationService navigationService) : base(navigationService)
        {
            this.authService = authService;

            SignInCommand = new RelayCommand(async _ =>
            {
                await Login(UserLoginText,UserPasswordText);
            });
        }
    }
}
