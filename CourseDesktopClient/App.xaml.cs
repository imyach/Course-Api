using CourseDesktopClient.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Windows;

namespace CourseDesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private IServiceProvider serviceProvider;
        private static Mutex mutex;
        protected void OnStartup(object sender, StartupEventArgs e)
        {
            mutex = new Mutex(true, "CourseDesktopClient", out bool createdNew);
            if (!createdNew)
            {
                MessageBox.Show("Приложение уже запущено!");
                Current.Shutdown();
                return;
            }
            var services = new ServiceCollection();
            StructureProgramm.RegisterServices(services);

            serviceProvider = services.BuildServiceProvider();

            InitializeAndShowMainWindow();
        }

        private async void InitializeAndShowMainWindow()
        {
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            var authService = serviceProvider.GetRequiredService<IAuthService>();
            var navService = serviceProvider.GetRequiredService<INavigationService>();


            var isAuthenticated = await authService.CheckAuthOnStartupAsync();
            if (isAuthenticated)
            {
                navService.NavigateToCourses();
            }
            else
                navService.NavigateToLogin();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            mutex?.ReleaseMutex();   
            mutex?.Dispose();   
            base.OnExit(e);
        }
        }
    }
