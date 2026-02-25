using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CourseDesktopClient.UI.Converters
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public static readonly StringToImageSourceConverter Instance = new StringToImageSourceConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                try
                {
                    return new ImageSourceConverter().ConvertFromString(path);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}