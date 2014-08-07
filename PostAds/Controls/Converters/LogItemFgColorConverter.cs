using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NLog;

namespace Motorcycle.Controls.Converters
{
    public class LogItemFgColorConverter : IValueConverter
    {
        private readonly Logger log = NLog.LogManager.GetCurrentClassLogger();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "Info" == value.ToString() ? Brushes.WhiteSmoke : Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}