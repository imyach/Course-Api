using CourseDesktopClient.ApiConnection;
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
        public ICommand LoadedCommand { get; set; }
        public ICommand SignInCommand { get; set; }

        private bool FillingVerification(string? loginOrEmail, string? password)
        {
            if (string.IsNullOrEmpty(loginOrEmail) || string.IsNullOrEmpty(password))
            {
                MisstakeText = "Заполните все поля";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            MisstakeText = string.Empty;
            VisibleMisstake = Visibility.Collapsed;
            return true;
        }

        private static void SaveInLocalStorage(string token)
        {
            var cred = new Credential { Target = "JwtToken" , Password = token, PersistanceType = PersistanceType.LocalComputer};
            cred.Save();
        }

        public async Task Login(string? loginOrEmail, string? password)
        {
            if (!FillingVerification(loginOrEmail, password))
                return;
            
            var loginDto = new LoginDto
            {
                Login = loginOrEmail,
                Password = password
            };


            var jsonRequestData = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

            var response = await ClientConfig.Client.PostAsync(ApiPaths.API_LOGIN_USER, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponseData = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<LoginResponseDto>(jsonResponseData);

                var token = responseContent?.Token;
                if (!string.IsNullOrEmpty(token))
                {
                   SaveInLocalStorage(token);
                }
                ClientConfig.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
                AllCourseCommandFunc();
            }
            else
            {
                MisstakeText = "Неправильный логин или пароль";
                VisibleMisstake = Visibility.Visible;
            }
        }




        public LoginPageVm()
        {
            UserPasswordText = string.Empty;
            UserLoginText = string.Empty;

            SignInCommand = new RelayCommand(_ =>
            {
                Login(UserLoginText,UserPasswordText);
            });
            LoadedCommand = new RelayCommand(_ =>
            {
                
            });
        }

    }
}
