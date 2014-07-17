using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Motorcycle.Controls.Log
{
    public class LogItemFgColorConverter : IValueConverter
    {
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