using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth.RequestDto;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class UpdateUserPasswordPageVm : NavigationVm
    {
        private string _userOldPasswordText = string.Empty;
        public string UserOldPasswordText
        {
            get { return _userOldPasswordText; }
            set { _userOldPasswordText = value; SetProperty(ref _userOldPasswordText, value); }
        }

        private string _userNewPasswordText = string.Empty;
        public string UserNewPasswordText
        {
            get { return _userNewPasswordText; }
            set { _userNewPasswordText = value; SetProperty(ref _userNewPasswordText, value); }
        }

        private string _userNewRepPasswordText = string.Empty;
        public string UserNewRepPasswordText
        {
            get { return _userNewRepPasswordText; }
            set { _userNewRepPasswordText = value; SetProperty(ref _userNewRepPasswordText, value); }
        }

        private string? _mistakeText = string.Empty;
        public string? MistakeText
        {
            get { return _mistakeText; }
            set { _mistakeText = value; OnPropertyChanged(); }
        }

        public ICommand SaveNewPassword { get; set; }


        private Visibility? _visibleMisstake = Visibility.Collapsed;
        public Visibility? VisibleMisstake
        {

            get { return _visibleMisstake; }
            set { _visibleMisstake = value; OnPropertyChanged(); }
        }
        public UpdateUserPasswordPageVm(INavigationService navigationService, IAuthService authService): base(navigationService) 
        {
            SaveNewPassword = new RelayCommand(async _ =>
            {
                var resultСheck = CheckFilling(UserOldPasswordText, UserNewPasswordText, UserNewRepPasswordText);
                if(resultСheck)
                {
                    var currentUser = authService.CurrentUser;
                    var request = new UpdateUserRequestDto
                    {
                        Id = currentUser.Id,
                        Role = currentUser.Role,
                        Email = currentUser.Email,
                        Login = currentUser.Login,
                        NameUser = currentUser.NameUser,
                        PhoneNumber = currentUser.PhoneNumber,
                        OldPassword = UserOldPasswordText,
                        NewPassword = UserNewRepPasswordText
                    };

                    await authService.UpdateUserPasswordAsync(request);
                }    
            });
        }

        private bool CheckFilling(string oldPassword, string newPassword, string newRepPassword )
        {
            if (string.IsNullOrEmpty(newPassword)
               || string.IsNullOrEmpty(newRepPassword)
               || string.IsNullOrEmpty(oldPassword))
            {
                MistakeText = "Заполните все поля";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            else if (newPassword != newRepPassword)
            {
                MistakeText = "Пароль введеный повторно не совпадает";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            else if (oldPassword == newPassword)
            {
                MistakeText = "Введите разные пароли";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            
            MistakeText = string.Empty;
            VisibleMisstake = Visibility.Collapsed;
            return true;
            
        }
    }
}
