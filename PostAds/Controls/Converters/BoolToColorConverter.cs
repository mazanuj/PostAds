using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NLog;

namespace Motorcycle.Controls.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush(Colors.Brown);
            }

            return System.Convert.ToBoolean(value) ?
                new SolidColorBrush(Colors.Green)
                : new SolidColorBrush(Colors.Brown);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}