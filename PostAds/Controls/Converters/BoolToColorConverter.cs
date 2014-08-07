using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NLog;

namespace Motorcycle.Controls.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        private readonly Logger log = NLog.LogManager.GetCurrentClassLogger();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush(Colors.Blue);
            }

            return System.Convert.ToBoolean(value) ?
                new SolidColorBrush(Colors.Red)
                : new SolidColorBrush(Colors.Aqua);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}