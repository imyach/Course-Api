using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Services;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class AllUsersPageVm : NavigationVm
    {
        private string _searchProgressCourse = string.Empty;
        public string SearchUsers { get { return _searchProgressCourse; } set { _searchProgressCourse = value; SetProperty(ref _searchProgressCourse, value); Update(); } }

        private IList<UserPanelElementVm>? _getUsers;
        public IList<UserPanelElementVm>? GetUsers { get => _getUsers; set { _getUsers = value; OnPropertyChanged(nameof(GetUsers)); } }
        private ObservableCollection<ButtonItem>? _buttonPanel = [];
        public ObservableCollection<ButtonItem>? ButtonPanel { get => _buttonPanel; set { _buttonPanel = value; OnPropertyChanged(nameof(ButtonPanel)); } }

        private readonly ICourseApiClient courseApiClient;
        private readonly IPagerService pagerService;
        private readonly IAuthService authService;

        public ICommand PagerCommand { get; set; }
        public ICommand LookProfile { get; set; }
        public ICommand AddUserCommand { get; set; }

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
        public Visibility AddUserButtonVisible =>
    authService.CurrentUser.Role.Name == "Admin" ? Visibility.Visible : Visibility.Hidden;
         


        public AllUsersPageVm(INavigationService navigationService, ICourseApiClient courseApiClient, IAuthService authService, IPagerService pagerService) :base(navigationService)
        {
            this.courseApiClient = courseApiClient;
            this.authService = authService;
            this.pagerService = pagerService;


            AddUserCommand = new RelayCommand(async _ =>
            {
                await navigationService.NavigateToCreateUser();
            });
            PagerCommand = new RelayCommand(async parameter =>
            {
                if (parameter is ButtonItem buttonItem && int.TryParse(buttonItem.Text, out int pageNumber))
                {
                    if (ButtonPanel != null)
                    {
                        foreach (var btn in ButtonPanel)
                        {
                            btn.IsSelected = btn.Text == pageNumber.ToString();
                        }
                    }

                    await Update(pageNumber);
                }
            });
            LookProfile = new RelayCommand(async userInfo => 
            {
               await navigationService.NavigateToProfile((userInfo as UserPanelElementVm).Id);
            });
        }

        public async Task Update(int pageNumber = 1)
        {
            var (users, pager) = await courseApiClient.GetUsersAsync(pageNumber,searchText:SearchUsers);
            var usersVm = users.Users.Select(x => new UserPanelElementVm(x, authService.CurrentUser)).ToList();

            GetUsers = usersVm;
            GenerateButtonPanel(pager);

            VisibleButtonPanel = pager.TotalItems <= pager.PageSize
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        

        private void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }
    }
}
