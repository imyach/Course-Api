using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace CourseDesktopClient.UI.Converters
{
    public static class TextBoxExtensions
    {
        public  static readonly DependencyProperty IconSourceProperty =
       DependencyProperty.RegisterAttached("IconSource", typeof(ImageSource), typeof(TextBoxExtensions));

        public static void SetIconSource(UIElement element, ImageSource value) =>
            element.SetValue(IconSourceProperty, value);

        public static ImageSource GetIconSource(UIElement element) =>
            (ImageSource)element.GetValue(IconSourceProperty);
    }
}
