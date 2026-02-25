using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CourseDesktopClient.UI.Converters
{
    public class BooleanToTextColorConverter : IValueConverter
    {
        public Brush TrueBrush { get; set; } = new SolidColorBrush(Colors.DarkGreen);
        public Brush FalseBrush { get; set; } = new SolidColorBrush(Colors.DarkRed);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? TrueBrush : FalseBrush;
            }
            return FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}