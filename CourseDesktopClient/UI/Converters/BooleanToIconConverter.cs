using System;
using System.Globalization;
using System.Windows.Data;

namespace CourseDesktopClient.UI.Converters
{
    public class BooleanToIconConverter : IValueConverter
    {
        public string TrueIcon { get; set; } = "✓";
        public string FalseIcon { get; set; } = "✗";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? TrueIcon : FalseIcon;
            }
            return FalseIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}