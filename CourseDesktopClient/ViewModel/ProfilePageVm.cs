using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Auth;
using CourseDesktopClient.Models.DtosModel.Auth.RequestDto;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.Entities.RequestDto;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
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

        private string _originalUserName;
        private string _originalUserEmail;
        private string _originalPhoneNumber;
        private string _originalUserLogin;
        private RoleDto _originalUserRole;

        public bool HasCourses => GetProgressUsers?.Any() == true;
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName)); Update();
            }
        }
            
        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value;
                OnPropertyChanged(nameof(UserEmail)); Update();
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));Update();
            }
        }
        private string _userLogin;
        public string UserLogin
        {
            get { return _userLogin; }
            set
            {
                _userLogin = value; 
                OnPropertyChanged(nameof(UserLogin));Update();
            }
        }
        private RoleDto _userRole;
        public RoleDto UserRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                OnPropertyChanged(nameof(UserRole)); Update();
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

        private bool _isVisible = false;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
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

        private Visibility _visibleButtonPanel;
        public Visibility VisibleButtonPanel
        {
            get { return _visibleButtonPanel; }
            set
            {
                _visibleButtonPanel = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleUpdatePassword;
        public Visibility VisibleUpdatePassword
        {
            get { return _visibleUpdatePassword; }
            set
            {
                _visibleUpdatePassword = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleEmptyPage;
        public Visibility VisibleEmptyPage
        {
            get { return _visibleEmptyPage; }
            set
            {
                _visibleEmptyPage = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleEditPanel;
        public Visibility VisibleEditPanel
        {
            get { return _visibleEditPanel; }
            set
            {
                _visibleEditPanel = value;
                OnPropertyChanged();
            }
        }


        private Visibility _visibleOutPanel;
        public Visibility VisibleOutPanel
        {
            get { return _visibleOutPanel; }
            set
            {
                _visibleOutPanel = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleRoleUpdate;
        public Visibility VisibleRoleUpdate
        {
            get { return _visibleRoleUpdate; }
            set
            {
                _visibleRoleUpdate = value;
                OnPropertyChanged();
            }
        }

        private IList<RoleDto>? _getRoles;
        public IList<RoleDto>? GetRoles { get => _getRoles; set { _getRoles = value; OnPropertyChanged(); } }

        private IList<MyCoursePanelForProfileElement>? _getProgressUsers;
        public IList<MyCoursePanelForProfileElement>? GetProgressUsers { get => _getProgressUsers; set { _getProgressUsers = value; OnPropertyChanged(nameof(GetProgressUsers)); OnPropertyChanged(nameof(HasCourses)); } }

        private ObservableCollection<ButtonItem>? _buttonPanel = [];
        public ObservableCollection<ButtonItem>? ButtonPanel { get => _buttonPanel; set { _buttonPanel = value; OnPropertyChanged(nameof(ButtonPanel)); } }
        private int _complitedCourses;
        public int ComplitedCourses { get { return _complitedCourses; } set { _complitedCourses = value; OnPropertyChanged(); } }
        private int _courseInPassage;
        public int CourseInPassage { get { return _courseInPassage; } set { _courseInPassage = value; OnPropertyChanged(); } }

        private readonly IPagerService pagerService;
        private  UserDto ViewedUser { get; set; } = new UserDto();
        public ICommand LogOutCommand {  get; set; }
        public ICommand DeleteUserProfile {  get; set; }
        public ICommand UpdateUserProfile {  get; set; }
        public ICommand PagerCommand { get; set; }

        public ICommand CancelEditCommand { get; set; }

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

        public ProfilePageVm(INavigationService navigationService, IAuthService authService, ICourseApiClient courseApiClient, IPagerService pagerService) : base(navigationService)
        {
            this.authService = authService;
            
            this.pagerService = pagerService;
            this.courseApiClient = courseApiClient;


        CancelEditCommand = new RelayCommand(_ =>
{
            UserName = _originalUserName;
            UserEmail = _originalUserEmail;
            PhoneNumber = _originalPhoneNumber;
            UserLogin = _originalUserLogin;
            UserRole = _originalUserRole;

            Update();
        });

            LogOutCommand = new RelayCommand(async _ =>
            {
                if (CustomMessageBox.ShowYesNo("Вы дейстивительно хотите выйти?") == DialogResult.Yes)
                {
                    await authService.LogoutAsync();
                }
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
                    Role = UserRole,
                };

                if (!FillingVerification(userDto))
                    return;

                await authService.UpdateUserAsync(userDto);

                ViewedUser.NameUser = UserName;
                ViewedUser.Email = UserEmail;
                ViewedUser.PhoneNumber = PhoneNumber;
                ViewedUser.Login = UserLogin;
                ViewedUser.Role = UserRole;

                _originalUserName = UserName;
                _originalUserEmail = UserEmail;
                _originalPhoneNumber = PhoneNumber;
                _originalUserLogin = UserLogin;
                _originalUserRole = UserRole;

                await LoadingProfilePage(userDto.Id);
            });
            PagerCommand = new RelayCommand(async pageNumberStr =>
            {
                if (int.TryParse((pageNumberStr as ButtonItem).Text, out int pageNumber))
                {
                    await LoadingProfilePage(ViewedUser.Id, pageNumber);
                }
            });
        }


        private bool FillingVerification(UpdateUserRequestDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Login)
              || string.IsNullOrEmpty(userDto.NameUser)
              || string.IsNullOrEmpty(userDto.Email))
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

            if (userDto.PhoneNumber is not null)
                if (!IsValidPhoneWithLib(userDto.PhoneNumber))
                {
                    MisstakeText = "Введите корректный номер";
                    VisibleMisstake = Visibility.Visible;
                    return false;
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

        public async Task LoadingProfilePage(Guid idUser, int pageNumber = 1)
        {
            if (idUser == authService.CurrentUser.Id)
            {
                ViewedUser = new UserDto
                {
                    Id = authService.CurrentUser.Id,
                    Login = authService.CurrentUser.Login,
                    Email = authService.CurrentUser.Email,
                    PhoneNumber = authService.CurrentUser.PhoneNumber,
                    NameUser = authService.CurrentUser.NameUser,
                    Role = authService.CurrentUser.Role,
                };

                UserName = authService.CurrentUser.NameUser;
                UserEmail = authService.CurrentUser.Email;
                PhoneNumber = authService.CurrentUser.PhoneNumber;
                UserLogin = authService.CurrentUser.Login;
                UserRole = authService.CurrentUser.Role;

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
                UserRole = user.Role;
                IsCurrentUSer = false;
            }

            _originalUserName = ViewedUser.NameUser;
            _originalUserEmail = ViewedUser.Email;
            _originalPhoneNumber = ViewedUser.PhoneNumber;
            _originalUserLogin = ViewedUser.Login;
            _originalUserRole = ViewedUser.Role;

            if (authService.CurrentUser.Role.Name == "Admin")
            {
                IsAccessUpdate = true;
            }
            else if (authService.CurrentUser.Id != ViewedUser.Id)
            {
                IsAccessUpdate = false;
            }
            if(authService.CurrentUser.Id == ViewedUser.Id)
            {
                IsVisible = false;
            }
            else if(authService.CurrentUser.Id != ViewedUser.Id && authService.CurrentUser.Role.Name == "Admin")
            {
                IsVisible = true;
                var roles = await courseApiClient.GetRolesAsync();
                GetRoles = roles.Roles;
                UserRole = GetRoles.FirstOrDefault(r => r.Id == ViewedUser.Role.Id) ?? ViewedUser.Role;
            }

            var (progeresCourses, progressinfo, pager) = await courseApiClient.GetProgressUsersAsync(ViewedUser.Id, pageNumber : pageNumber);


            var courseProgressViewModel = progeresCourses.ProgressUsers.Select(x => new MyCoursePanelForProfileElement(x)).ToList();

            ComplitedCourses = progressinfo.CompletedCourse;
            CourseInPassage = progressinfo.CourseInPassage;

            GetProgressUsers = courseProgressViewModel;
            GenerateButtonPanel(pager);



            VisibleButtonPanel = pager.TotalItems <= pager.PageSize
                ? Visibility.Collapsed
                : Visibility.Visible;

            VisibleEmptyPage = pager.TotalItems <= 0 && ViewedUser.Id == authService.CurrentUser.Id
                ? Visibility.Visible
                : Visibility.Collapsed;

            VisibleEditPanel = ViewedUser.Id == authService.CurrentUser.Id || authService.CurrentUser.Role.Name == "Admin"
                ? Visibility.Visible
                : Visibility.Collapsed;

            VisibleOutPanel = ViewedUser.Id == authService.CurrentUser.Id
                ? Visibility.Visible
                : Visibility.Hidden;

            VisibleRoleUpdate = (ViewedUser.Id == authService.CurrentUser.Id && authService.CurrentUser.Role.Name == "Admin") || authService.CurrentUser.Role.Name != "Admin"
                 ? Visibility.Collapsed
                : Visibility.Visible;

            VisibleUpdatePassword = ViewedUser.Id == authService.CurrentUser.Id
                 ? Visibility.Visible
                : Visibility.Collapsed;
            Update();

        }

        private void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }



        private void Update()
        {
            if (UserName == _originalUserName
                && UserEmail == _originalUserEmail
                && PhoneNumber == _originalPhoneNumber
                && UserLogin == _originalUserLogin
                && UserRole?.Name == _originalUserRole?.Name)
            {
                IsEnable = false;
            }
            else
                IsEnable = true;
        }
    }
}
