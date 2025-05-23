﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace FinancialTracker.Converters
{
    public class BoolToSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "Megtakarítás" : "Egyenleg";
            }
            return "Egyenleg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}