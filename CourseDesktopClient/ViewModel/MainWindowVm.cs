using CourseDesktopClient.ApiConnection;
using CourseDesktopClient.Utilities;
using CourseDesktopClient.View;
using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class MainWindowVm(MainWindow main) : NavigationVm
    {
        public ICommand Loaded => new RelayCommand(x => {
            var cred = new Credential { Target = "JwtToken" };
            if (cred.Load())
            {
                ClientConfig.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cred.Password);
                AllCourseCommandFunc();
            }
        });

        public ICommand LogOut => new RelayCommand(x => {
            var cred = new Credential { Target = "JwtToken" };
            if (cred.Load())
            {   
                cred.Delete();
            }
            ClientConfig.Client.DefaultRequestHeaders.Authorization = null;
            AllCourseCommandFunc();
        });
    }
}
