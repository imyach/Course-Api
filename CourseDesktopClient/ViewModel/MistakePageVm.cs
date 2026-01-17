using CourseDesktopClient.Interfaces;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class MistakePageVm : NavigationVm
    {
        private string _mistakeText;

        public string MistakeText
        {
            get { return _mistakeText; }
            set
            {
                _mistakeText = value;
                OnPropertyChanged();
            }
        }


        public ICommand RestartCommand { get; set; }

        public MistakePageVm(INavigationService navigationService) : base(navigationService)
        {
            RestartCommand = new RelayCommand(_ =>
            {
                if (CustomMessageBox.ShowYesNo("Перезапустить приложение?") == DialogResult.Yes)
                {

                    var executablePath = Process.GetCurrentProcess().MainModule.FileName;


                    var startInfo = new ProcessStartInfo(executablePath);

                    Process.Start(startInfo);

                    Application.Current.Shutdown();
                }
            });
        }

        public void SetException(Exception exception)
        {
            if (exception is HttpRequestException httpEx)
            {
                if (httpEx.InnerException is SocketException)
                {
                    MistakeText = "Не удалось подключиться к серверу";
                }
                else
                {
                    MistakeText = httpEx.Message;
                }
            }
            else if (exception is TimeoutException)
            {
                MistakeText = "Время ожидания ответа истекло";
            }
            else
            {
                MistakeText = exception.Message;
            }
        }
    }
}
