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

            // Проверяем, является ли parameter типом
            if (parameter is Type targetTypeToCompare)
            {
                // Сравниваем тип текущего объекта с целевым типом
                return value.GetType() == targetTypeToCompare;
            }

            // Если parameter - это строка с именем типа
            if (parameter is string typeName)
            {
                // Пытаемся найти тип по имени
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