using System;
using System.Globalization;
using System.Windows.Data;

namespace CourseDesktopClient.UI.Converters
{
    public class TypeEqualsConverter : IValueConverter
    {
        public static readonly TypeEqualsConverter Instance = new TypeEqualsConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            if (parameter is Type targetTypeToCompare)
            {
                return value.GetType() == targetTypeToCompare;
            }

            if (parameter is string typeName)
            {
                var type = Type.GetType(typeName);
                if (type != null)
                {
                    return value.GetType() == type;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}