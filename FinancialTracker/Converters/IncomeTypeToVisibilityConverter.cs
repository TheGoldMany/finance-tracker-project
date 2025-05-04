using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FinancialTracker.Models;

namespace FinancialTracker.Converters
{
    public class IncomeTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IncomeType type)
            {
                return type == IncomeType.Egyéb ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}