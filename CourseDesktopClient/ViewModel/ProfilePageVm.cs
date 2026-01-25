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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private RoleDto _userRole;
        public RoleDto UserRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                OnPropertyChanged(nameof(UserRole));
                SetProperty(ref _userRole, value); Update();
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


        private IList<RoleDto>? _getRoles;
        public IList<RoleDto>? GetRoles { get => _getRoles; set { _getRoles = value; OnPropertyChanged(); } }

        private IList<MyCoursePanelForProfileElement>? _getProgressUsers;
        public IList<MyCoursePanelForProfileElement>? GetProgressUsers { get => _getProgressUsers; set { _getProgressUsers = value; OnPropertyChanged(nameof(GetProgressUsers)); } }

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


        public ProfilePageVm(INavigationService navigationService, IAuthService authService, ICourseApiClient courseApiClient, IPagerService pagerService) : base(navigationService)
        {
            this.authService = authService;
            
            this.pagerService = pagerService;
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
                    Role = UserRole,
                };
                await authService.UpdateUserAsync(userDto);
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

        public async Task LoadingProfilePage(Guid idUser, int pageNumber = 1)
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

            Update();

        }

        private void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }

        private void Update()
        {
            if(UserName == ViewedUser.NameUser
                && UserEmail == ViewedUser.Email
                && PhoneNumber == ViewedUser.PhoneNumber
                && UserLogin == ViewedUser.Login
                && UserRole?.Name == ViewedUser.Role.Name)
            {
                IsEnable = false;
            }
            else
                IsEnable = true;
        }
    }
}
