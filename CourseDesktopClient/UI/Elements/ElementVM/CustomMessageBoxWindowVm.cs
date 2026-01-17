using CourseDesktopClient.Utilities;
using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class CustomMessageBoxWindowVm : ViewModelBase
    {
            private Window _dialogWindow;
            private DialogResult _result;

            private string _title;
            public string Title
            {
                get => _title;
                set => SetProperty(ref _title, value);
            }

            private string _message;
            public string Message
            {
                get => _message;
                set => SetProperty(ref _message, value);
            }

            private string _okButtonText = "OK";
            public string OkButtonText
            {
                get => _okButtonText;
                set => SetProperty(ref _okButtonText, value);
            }

            private string _cancelButtonText = "Отмена";
            public string CancelButtonText
            {
                get => _cancelButtonText;
                set => SetProperty(ref _cancelButtonText, value);
            }

            private Visibility _cancelButtonVisibility = Visibility.Visible;
            public Visibility CancelButtonVisibility
            {
                get => _cancelButtonVisibility;
                set => SetProperty(ref _cancelButtonVisibility, value);
            }

            private Brush _iconColor = Brushes.Gray;
            public Brush IconColor
            {
                get => _iconColor;
                set => SetProperty(ref _iconColor, value);
            }

            private string _iconKind = "QuestionMark";
            public string IconKind
            {
                get => _iconKind;
                set => SetProperty(ref _iconKind, value);
            }

            public RelayCommand OkCommand { get; }
            public RelayCommand CancelCommand { get; }

            public CustomMessageBoxWindowVm()
            {
                OkCommand = new RelayCommand(() => CloseDialog(DialogResult.Yes));
                CancelCommand = new RelayCommand(() => CloseDialog(DialogResult.No));
            }

            public void Initialize(Window window, string message, string title,
                                 MessageBoxButton buttons, MessageBoxImage icon)
            {
                _dialogWindow = window;
                Message = message;
                Title = string.IsNullOrEmpty(title) ? GetDefaultTitle(icon) : title;

                ConfigureButtons(buttons);
                ConfigureIcon(icon);
            }

            private void ConfigureButtons(MessageBoxButton buttons)
            {
                switch (buttons)
                {
                    case MessageBoxButton.OK:
                        OkButtonText = "OK";
                        CancelButtonVisibility = Visibility.Collapsed;
                        break;
                    case MessageBoxButton.OKCancel:
                        OkButtonText = "OK";
                        CancelButtonText = "Отмена";
                        CancelButtonVisibility = Visibility.Visible;
                        break;
                    case MessageBoxButton.YesNo:
                        OkButtonText = "Да";
                        CancelButtonText = "Нет";
                        CancelButtonVisibility = Visibility.Visible;
                        break;
                    case MessageBoxButton.YesNoCancel:
                        OkButtonText = "Да";
                        CancelButtonText = "Нет";
                        // Для YesNoCancel нужна третья кнопка - упрощаем до YesNo
                        CancelButtonVisibility = Visibility.Visible;
                        break;
                }
            }

            private void ConfigureIcon(MessageBoxImage icon)
            {
                switch (icon)
                {
                    case MessageBoxImage.Error:
                        IconKind = "Error";
                        IconColor = Brushes.DarkRed;
                        break;
                    case MessageBoxImage.Information:
                        IconKind = "InfoCircle";
                        IconColor = Brushes.Blue;
                        break;
                    case MessageBoxImage.Warning:
                        IconKind = "Warning";
                        IconColor = Brushes.Orange;
                        break;
                    case MessageBoxImage.Question:
                        IconKind = "QuestionMark";
                        IconColor = Brushes.Gray;
                        break;
                    default:
                        IconKind = "Information";
                        IconColor = Brushes.Blue;
                        break;
                }
            }

            private string GetDefaultTitle(MessageBoxImage icon)
            {
                return icon switch
                {
                    MessageBoxImage.Error => "Ошибка",
                    MessageBoxImage.Warning => "Предупреждение",
                    MessageBoxImage.Question => "Подтверждение",
                    _ => "Сообщение"
                };
            }

            private void CloseDialog(DialogResult result)
            {
                _result = result;
                AnimateClose();
            }

        private void AnimateClose()
        {
            var scale = new ScaleTransform(1.0, 1.0);
            _dialogWindow.RenderTransformOrigin = new Point(0.5, 0.5);
            _dialogWindow.RenderTransform = scale;

            var storyboard = new Storyboard();

            // Анимация для ScaleX
            var animationX = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(200),
                From = 1,
                To = 0,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            // Анимация для ScaleY
            var animationY = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(200),
                From = 1,
                To = 0,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            storyboard.Children.Add(animationX);
            storyboard.Children.Add(animationY);

            Storyboard.SetTargetProperty(animationX, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(animationX, _dialogWindow);

            Storyboard.SetTargetProperty(animationY, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTarget(animationY, _dialogWindow);

            animationX.Completed += (s, e) =>
            {
                _dialogWindow.DialogResult = true;
                _dialogWindow.Close();
            };

            storyboard.Begin();
        }

        public DialogResult Result => _result;
        }

        public enum DialogResult
        {
            None,
            Yes,
            No,
            Cancel,
            OK
        }
    }