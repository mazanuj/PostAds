using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NLog;

namespace Motorcycle.Controls.Converters
{
    public class LogItemFgColorConverter : IValueConverter
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "Debug" == value.ToString() ? Brushes.Black : Brushes.WhiteSmoke;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}