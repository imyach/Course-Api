using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CourseDesktopClient.UI.Converters
{
    public class ActiveStateToColorConverter : IValueConverter
    {
        public static readonly ActiveStateToColorConverter Instance = new ActiveStateToColorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string targetType_param = parameter as string;
            bool isActive = value as string == "Active";

            if (targetType_param == "Background")
            {
                return isActive ? (Color)ColorConverter.ConvertFromString("#4CAF50") : Colors.Transparent;
            }
            else if (targetType_param == "Foreground")
            {
                return isActive ? Colors.White : (Color)ColorConverter.ConvertFromString("#333333");
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}