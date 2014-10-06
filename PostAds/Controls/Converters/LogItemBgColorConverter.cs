﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NLog;

namespace Motorcycle.Controls.Converters
{
    public class LogItemBgColorConverter : IValueConverter
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "Debug":
                    return Brushes.Plum;
                case "Warn":
                    return Brushes.Yellow;
                case "Error":
                    return Brushes.Tomato;
                case "Info":
                    return Brushes.White;
                default:
                    return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}