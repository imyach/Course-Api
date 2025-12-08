using CourseDesktopClient.Utilities;
using CourseDesktopClient.View;
using CourseDesktopClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseDesktopClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Nav.Current = new MainWindowVm() { CurrentView = new RegisterPage(), VisibilitySearch = Visibility.Collapsed };  
            
        }
    }
}
