using System;
using System.Globalization;
using System.Windows.Data;

namespace CourseDesktopClient.UI.Converters
{
    public class FirstLettersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string name && !string.IsNullOrEmpty(name))
            {
                var words = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length >= 2)
                {
                    // Берем первую букву первого и второго слова
                    return $"{words[0][0]}{words[1][0]}".ToUpper();
                }
                else if (name.Length >= 2)
                {
                    // Берем первые две буквы
                    return name.Substring(0, 2).ToUpper();
                }
                else if (name.Length == 1)
                {
                    return name.ToUpper();
                }
            }
            return "?";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}