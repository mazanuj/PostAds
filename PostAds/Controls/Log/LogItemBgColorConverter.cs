﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Motorcycle.Controls.Log
{
    public class LogItemBgColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "Debug":
                    return Brushes.White;
                case "Warn":
                    return Brushes.Yellow;
                case "Error":
                    return Brushes.Tomato;
                case "Info":
                    return Brushes.Purple;
                default:
                    return Brushes.Plum;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}