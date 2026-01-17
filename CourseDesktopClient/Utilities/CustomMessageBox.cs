using CourseDesktopClient.UI.Elements;
using CourseDesktopClient.UI.Elements.ElementVM;

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CourseDesktopClient.Utilities
{
    public static class CustomMessageBox
    {
        public static DialogResult Show(string message, string caption = "",
                       MessageBoxButton buttons = MessageBoxButton.OK,
                       MessageBoxImage icon = MessageBoxImage.Information)
        {
            var dialog = new CustomMessageBoxWindow();
            var vm = new CustomMessageBoxWindowVm();

            vm.Initialize(dialog, message, caption, buttons, icon);
            dialog.DataContext = vm;

            // Безопасная установка Owner
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null && mainWindow.IsLoaded && mainWindow.IsVisible)
            {
                dialog.Owner = mainWindow;
            }
            else
            {
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            dialog.ShowDialog();
            return vm.Result;
        }

        private static void AnimateOpen(Window window)
        {
            if (window == null || !window.IsLoaded) return;

            if (!window.Dispatcher.CheckAccess())
            {
                window.Dispatcher.Invoke(() => AnimateOpen(window));
                return;
            }

            var scaleTransform = new ScaleTransform(0.8, 0.8);
            window.RenderTransform = scaleTransform;
            window.RenderTransformOrigin = new Point(0.5, 0.5);

            window.Dispatcher.BeginInvoke(new Action(() =>
            {
                var storyboard = new Storyboard();

                var animationX = new DoubleAnimation
                {
                    Duration = TimeSpan.FromMilliseconds(300),
                    From = 0.8,
                    To = 1.0,
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                var animationY = new DoubleAnimation
                {
                    Duration = TimeSpan.FromMilliseconds(300),
                    From = 0.8,
                    To = 1.0,
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                storyboard.Children.Add(animationX);
                storyboard.Children.Add(animationY);

                Storyboard.SetTargetProperty(animationX, new PropertyPath("RenderTransform.ScaleX"));
                Storyboard.SetTarget(animationX, scaleTransform);

                Storyboard.SetTargetProperty(animationY, new PropertyPath("RenderTransform.ScaleY"));
                Storyboard.SetTarget(animationY, scaleTransform);

                storyboard.Begin();
            }), System.Windows.Threading.DispatcherPriority.Render);
        }


        public static DialogResult ShowYesNo(string message, string caption = "Подтверждение")
        {
            return Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static DialogResult ShowOkCancel(string message, string caption = "")
        {
            return Show(message, caption, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        public static DialogResult ShowError(string message, string caption = "Ошибка")
        {
            return Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static DialogResult ShowWarning(string message, string caption = "Предупреждение")
        {
            return Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static DialogResult ShowInfo(string message, string caption = "Информация")
        {
            return Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}