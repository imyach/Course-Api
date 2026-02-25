using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.Entities.RequestDto;
using CourseDesktopClient.Utilities;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class CreateUserPageVm : ViewModelBase
    {

        private readonly IAuthService authService;
        private readonly INavigationService navigationService;
        private readonly ICourseApiClient courseApiClient;

        public ICommand CreateUserCommand { get; set; }
        public ICommand BackBtnCommand { get; set; }

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

        private RoleDto _userRole;
        public RoleDto UserRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                OnPropertyChanged(nameof(UserRole));
                SetProperty(ref _userRole, value); 
            }
        }

        private IList<RoleDto>? _getRoles;
        public IList<RoleDto>? GetRoles { get => _getRoles; set { _getRoles = value; OnPropertyChanged(); } }

        private bool FillingVerification(UserRequestDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Login)
                || string.IsNullOrEmpty(userDto.NameUser)
                || string.IsNullOrEmpty(userDto.Password))
            {
                MisstakeText = "Заполните все необходимые поля";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (userDto.NameUser.Length < 2)
            {
                MisstakeText = "Имя не может быть менее 2 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (userDto.Login.Length < 5)
            {
                MisstakeText = "Логин не может быть менее 5 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }


            if (userDto.Login.Length > 30)
            {
                MisstakeText = "Логин не может быть больше 30 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (userDto.Email?.Length > 50)
            {
                MisstakeText = "Почта не может быть больше 50 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (userDto.NameUser.Length > 50)
            {
                MisstakeText = "Имя пользователя не может быть больше 50 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (userDto.Password.Length < 5)
            {
                MisstakeText = "Пароль не может быть менее 5 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (userDto.Password.Length > 30)
            {
                MisstakeText = "Пароль не может быть больше 30 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (userDto.PhoneNumber is not null)
                if (!IsValidPhoneWithLib(userDto.PhoneNumber))
                {
                    MisstakeText = "Введите корректный номер";
                    VisibleMisstake = Visibility.Visible;
                    return false;
                }

            if (userDto.Email is not null)
            {

            }
                if (!IsValidEmail(userDto.Email))
                {
                    MisstakeText = "Введите корректную почту";
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
                return true;

            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        public async Task CreateUser(UserRequestDto userDto)
        {
            if (!FillingVerification(userDto))
                return;

            var userId = await courseApiClient.CreateUserAsync(userDto);

            if(userId == Guid.Empty || userId == null)
            {

                VisibleMisstake = Visibility.Visible;
                MisstakeText = "Внутреняя ошибка сервера при создании данного пользователя";
            }
            else
            {
                VisibleMisstake = Visibility.Collapsed;
                CustomMessageBox.ShowInfo("Пользователь создан");
                await navigationService.NavigateToUsers();
            }
        }

        public async Task LoadCreateUserPage()
        {
            var roles = await courseApiClient.GetRolesAsync();
            GetRoles = roles.Roles;
        }

        public  CreateUserPageVm(IAuthService authService, INavigationService navigationService, ICourseApiClient courseApiClient)
        {
            this.authService = authService;
            this.navigationService = navigationService;
            this.courseApiClient = courseApiClient;

            CreateUserCommand = new RelayCommand(async _ =>
            {
                var userDto = new UserRequestDto
                {
                    Login = UserLoginText,
                    Password = UserPasswordText,
                    NameUser = UserNameText,
                    Email = UserEmailText,
                    PhoneNumber = UserPhoneNumber,
                    Role = UserRole
                };
                await CreateUser(userDto);
            });

            BackBtnCommand = new RelayCommand(async () => 
            {
                await navigationService.NavigateToUsers();
            });

        }
    }
}
