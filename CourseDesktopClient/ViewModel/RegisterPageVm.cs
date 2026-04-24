using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Utilities;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class RegisterPageVm : NavigationVm
    {
        private readonly IAuthService authService;
        public ICommand SignUpCommand { get; set; }

        private string _userPasswordText = string.Empty;
        public string UserPasswordText
        {
            get { return _userPasswordText; }
            set { _userPasswordText = value; SetProperty(ref _userPasswordText, value); }
        }
        private string _userPhoneNumber = string.Empty;
        public string UserPhoneNumber
        {
            get { return _userPhoneNumber; }
            set { _userPhoneNumber = value; SetProperty(ref _userPhoneNumber, value); }
        }
        private string _userRepeedPasswordText = string.Empty;
        public string UserRepeedPasswordText
        {
            get { return _userRepeedPasswordText; }
            set { _userRepeedPasswordText = value; SetProperty(ref _userRepeedPasswordText, value); }
        }
        private string _userEmailText;
        public string UserEmailText
        {
            get { return _userEmailText; }
            set { _userEmailText = value; SetProperty(ref _userEmailText, value); }
        }
        private string _userNameText;
        public string UserNameText
        {
            get { return _userNameText; }
            set { _userNameText = value; SetProperty(ref _userNameText, value); }
        }
        private string _misstakeText = string.Empty;
        public string MisstakeText
        {
            get { return _misstakeText; }
            set { _misstakeText = value; OnPropertyChanged(); }
        }

        private Visibility? _visibleMisstake = Visibility.Collapsed;
        public Visibility? VisibleMisstake
        {

            get { return _visibleMisstake; }
            set { _visibleMisstake = value; OnPropertyChanged(); }
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

        private bool FillingVerificationRegister(RegisterDto registerDto, string repPass)
        {
            if (string.IsNullOrEmpty(registerDto.Login) 
                || string.IsNullOrEmpty(registerDto.NameUser)
                || string.IsNullOrEmpty(registerDto.Email)
                || string.IsNullOrEmpty(repPass)
                || string.IsNullOrEmpty(registerDto.Password))
            {
                MisstakeText = "Заполните все необходимые поля";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (registerDto.NameUser.Length < 2)
            {
                MisstakeText = "Имя не может быть менее 2 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (registerDto.Login.Length < 5)
            {
                MisstakeText = "Логин не может быть менее 5 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (registerDto.Password.Length < 5)
            {
                MisstakeText = "Пароль не может быть менее 5 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (registerDto.Login.Length > 30)
            {
                MisstakeText = "Логин не может быть больше 30 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (registerDto.Email?.Length > 50)
            {
                MisstakeText = "Почта не может быть больше 50 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (registerDto.NameUser.Length > 50)
            {
                MisstakeText = "Имя пользователя не может быть больше 50 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (registerDto.Password.Length > 30)
            {
                MisstakeText = "Пароль не может быть больше 30 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (registerDto.PhoneNumber is not null)
                if (!IsValidPhoneWithLib(registerDto.PhoneNumber))
                {
                    MisstakeText = "Введите корректный номер";
                    VisibleMisstake = Visibility.Visible;
                    return false;
                }

            if (!IsValidEmail(registerDto.Email))
            {
                MisstakeText = "Введите корректную почту";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (registerDto.Password != repPass)
            {
                MisstakeText = "Пароли не совпадают";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            MisstakeText = string.Empty;
            VisibleMisstake = Visibility.Collapsed;
            return true;
        }

        public static bool IsValidPhoneWithLib(string phoneNumber, string region = "RU")
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return true;

            var phoneUtil = PhoneNumberUtil.GetInstance();

            try
            {
                var number = phoneUtil.Parse(phoneNumber, region);
                return phoneUtil.IsValidNumber(number);
            }
            catch (NumberParseException)
            {
                return false;
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            if (!email.Contains("@") || !email.Contains("."))
                return false;

            if (email.IndexOf("@") + 1 >= email.LastIndexOf("."))
                return false;

            if (email.Contains(" "))
                return false;

            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        public async Task Register(RegisterDto registerDto, string repPass)
        {
            if (!FillingVerificationRegister(registerDto, repPass))
                return;

            authService.IsRememberProfile = CheckedSaveUser;
            var mistakeText = await authService.RegisterAsync(registerDto);

            switch (string.IsNullOrEmpty(mistakeText))
            {
                case true:
                    UserLoginText = string.Empty;
                    UserPasswordText = string.Empty;
                    UserPhoneNumber = string.Empty;
                    UserRepeedPasswordText = string.Empty;
                    UserEmailText = string.Empty;
                    UserNameText = string.Empty;
                    MisstakeText = string.Empty;
                    CheckedSaveUser = false;
                    break;
                case false:
                    MisstakeText = mistakeText;
                    VisibleMisstake = Visibility.Visible;
                    return;
            }
        }


        public RegisterPageVm(IAuthService authService, INavigationService navigationService) : base(navigationService)
        {
            this.authService = authService;

            SignUpCommand = new RelayCommand(async _ =>
            {
                var registerDto = new RegisterDto
                {
                    Login = UserLoginText,
                    Password = UserPasswordText,
                    NameUser = UserNameText, 
                    Email = UserEmailText,
                    PhoneNumber = UserPhoneNumber,
                };
                await Register(registerDto, UserRepeedPasswordText);
            });
        }
    }
}
