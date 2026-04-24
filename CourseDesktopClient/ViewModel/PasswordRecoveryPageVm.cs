using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Services;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.UI.Elements.ElementVM.RecoveryPasswordElementsVm;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CourseDesktopClient.ViewModel
{
    public class PasswordRecoveryPageVm : NavigationVm
    {
        private string _misstakeText;
        public string MisstakeText
        {
            get => _misstakeText;
            set
            {
                _misstakeText = value;
                OnPropertyChanged();
            }
        }

        private string _userEmail;
        public string UserEmail
        {
            get => _userEmail;
            set
            {
                _userEmail = value;
                OnPropertyChanged();
            }
        }

        private string _verificationCode;
        public string VerificationCode
        {
            get => _verificationCode;
            set
            {
                _verificationCode = value;
                OnPropertyChanged();
            }
        }

        private Visibility _sendEmailElementVisible;
        public Visibility SendEmailElementVisible
        {
            get => _sendEmailElementVisible;
            set
            {
                _sendEmailElementVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility _codeEntryElementVisible;
        public Visibility CodeEntryElementVisible
        {
            get => _codeEntryElementVisible;
            set
            {
                _codeEntryElementVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility _accountSelectionElementVisible;
        public Visibility AccountSelectionElementVisible
        {
            get => _accountSelectionElementVisible;
            set
            {
                _accountSelectionElementVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleMisstake;
        public Visibility VisibleMisstake
        {
            get => _visibleMisstake;
            set
            {
                _visibleMisstake = value;
                OnPropertyChanged();
            }
        }

        private IList<SelectionProfileElementVm>? _users;
        public IList<SelectionProfileElementVm>? Users { get => _users; set { _users = value; OnPropertyChanged(nameof(Users)); } }

        private SelectionProfileElementVm CurrentUser;
        public ICommand SendCodeCommand { get; set; }
        public ICommand GoToEmailCommand { get; set; }
        public ICommand ToConfirmCodeCommand { get; set; }
        public ICommand ConfirmCodeCommand { get; set; }
        public ICommand ResendCodeCommand { get; set; }

        public PasswordRecoveryPageVm(INavigationService navigationService,ICourseApiClient courseApiClient) : base(navigationService)
        {
            VisibleMisstake = Visibility.Collapsed;

            SendEmailElementVisible = Visibility.Visible;
            AccountSelectionElementVisible = Visibility.Collapsed;
            CodeEntryElementVisible = Visibility.Collapsed;

            GoToEmailCommand = new RelayCommand(async x =>
            {
                SendEmailElementVisible = Visibility.Visible;
                AccountSelectionElementVisible = Visibility.Collapsed;
                CodeEntryElementVisible = Visibility.Collapsed;
            });

            SendCodeCommand = new RelayCommand(async _=>
            {
                await GetUsersByEmail(courseApiClient);
            });

            ResendCodeCommand = new RelayCommand(async _ =>
            {
                await SendCodeOnEmail(courseApiClient);
                CustomMessageBox.ShowInfo("На ваш email был повторно отправлен код подтверждения");
            });

            ToConfirmCodeCommand = new RelayCommand(async x =>
            {
                CurrentUser = x as SelectionProfileElementVm;

                await SendCodeOnEmail(courseApiClient);
            });

            ConfirmCodeCommand = new RelayCommand(async x =>
            {
                await ConfirmCodeAndSendCodeOnEmail(courseApiClient, navigationService);
            });
        }




        private async Task ConfirmCodeAndSendCodeOnEmail(ICourseApiClient courseApiClient, INavigationService navigationService)
        {
            var result = await courseApiClient.SendNewPasswordOnEmailAsync(VerificationCode, CurrentUser.Id);

            if (result)
            {
                SendEmailElementVisible = Visibility.Visible;
                AccountSelectionElementVisible = Visibility.Collapsed;
                CodeEntryElementVisible = Visibility.Collapsed;
                VisibleMisstake = Visibility.Collapsed;

                CustomMessageBox.ShowInfo("На ваш email отправлен новый пароль от аккаунта");
                navigationService.NavigateToLogin();
            }
            else
            {
                VisibleMisstake = Visibility.Visible;
                MisstakeText = "Код введен неверно или срок его действия истек";
                return;
            }

            
        }

        private async Task SendCodeOnEmail(ICourseApiClient courseApiClient)
        {
            await courseApiClient.SendCodeEmailAsync(UserEmail, CurrentUser.Id);

            SendEmailElementVisible = Visibility.Collapsed;
            AccountSelectionElementVisible = Visibility.Collapsed;
            CodeEntryElementVisible = Visibility.Visible;
        }



        private async Task GetUsersByEmail(ICourseApiClient courseApiClient)
        {
            VisibleMisstake = Visibility.Collapsed;

            if (!IsValidEmail(UserEmail))
            {
                MisstakeText = "Введите корректную почту";
                VisibleMisstake = Visibility.Visible;
                return;
            }

            var users =  await courseApiClient.GetUsersByEmailAsync(UserEmail);
           
            if(users.Users.Count == 0 )
            {
                MisstakeText = "Аккаунтов с такой почтой нет";
                VisibleMisstake = Visibility.Visible;
                return;
            }

            var usersVm = users.Users.Select(x => new SelectionProfileElementVm(x)).ToList();
            Users = usersVm;

            SendEmailElementVisible = Visibility.Collapsed;
            AccountSelectionElementVisible = Visibility.Visible;
            CodeEntryElementVisible = Visibility.Collapsed;

            VisibleMisstake = Visibility.Collapsed;
        }

        private static bool IsValidEmail(string email)
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
    }
}
