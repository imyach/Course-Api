using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Auth.RequestDto;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.Entities.RequestDto;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    class ProfilePageVm : NavigationVm
    {
        private readonly IAuthService authService;
        private readonly ICourseApiClient courseApiClient;

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
                SetProperty(ref _userName, value); Update();
            }
        }
            
        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value;
                OnPropertyChanged(nameof(UserEmail));
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
                OnPropertyChanged(nameof(PhoneNumber));
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
                OnPropertyChanged(nameof(UserLogin));
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

        private bool _isCurrentUSer = false;
        public bool IsCurrentUSer
        {
            get { return _isCurrentUSer; }
            set
            {
                _isCurrentUSer = value;
                OnPropertyChanged();
            }
        }

        private bool _isAccessUpdate = false;
        public bool IsAccessUpdate
        {
            get { return _isAccessUpdate; }
            set
            {
                _isAccessUpdate = value;
                OnPropertyChanged();
            }
        }

        private  UserDto ViewedUser { get; set; } = new UserDto();
        public ICommand LogOutCommand {  get; set; }
        public ICommand DeleteUserProfile {  get; set; }
        public ICommand UpdateUserProfile {  get; set; }
        public ProfilePageVm(INavigationService navigationService, IAuthService authService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            this.authService = authService;
            this.courseApiClient = courseApiClient;

            LogOutCommand = new RelayCommand(async _ =>
            {
                await authService.LogoutAsync();
            });


            DeleteUserProfile = new RelayCommand(async _ =>
            {
                await authService.DeleteProfile(ViewedUser.Id);
            });

            UpdateUserProfile = new RelayCommand(async _ =>
            {
                var userDto = new UpdateUserRequestDto
                {
                    Id = ViewedUser.Id,
                    Login = UserLogin,
                    Email = UserEmail,
                    PhoneNumber = PhoneNumber,
                    NameUser = UserName,
                    Role = ViewedUser.Role,
                };
                
                await authService.UpdateUserAsync(userDto);
            });
        }

        public async Task LoadingProfilePage(Guid idUser)
        {
            if (idUser == authService.CurrentUser.Id)
            {
                ViewedUser = new UserDto 
                {
                    Id = authService.CurrentUser.Id,
                    Login = UserLogin= authService.CurrentUser.Login,
                    Email = UserEmail = authService.CurrentUser.Email,
                    PhoneNumber = PhoneNumber= authService.CurrentUser.PhoneNumber,
                    NameUser = UserName = authService.CurrentUser.NameUser,
                    Role = authService.CurrentUser.Role,
                };
                
                UserRole = authService.CurrentUser.Role.Name;
                IsCurrentUSer = true;
            }
            else
            {
                var user = await courseApiClient.GetUserProfileAsync(idUser);

                ViewedUser = user;

                UserName = user.NameUser;
                UserEmail = user.Email;
                PhoneNumber = user.PhoneNumber;
                UserLogin = user.Login;
                UserRole = user.Role.Name;
                IsCurrentUSer = false;
            }

            if (authService.CurrentUser.Role.Name == "Admin")
            {
                IsAccessUpdate = true;
            }
            else if (authService.CurrentUser.Id != ViewedUser.Id)
            {
                IsAccessUpdate = false;
            }
            Update();

        }

        private void Update()
        {
            if(UserName == ViewedUser.NameUser
                && UserEmail == ViewedUser.Email
                && PhoneNumber == ViewedUser.PhoneNumber
                && UserLogin == ViewedUser.Login)
            {
                IsEnable = false;
            }
            else
                IsEnable = true;
        }
    }
}
