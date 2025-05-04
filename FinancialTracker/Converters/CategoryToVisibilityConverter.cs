using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FinancialTracker.Models;

namespace FinancialTracker.Converters
{
    public class CategoryToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ExpenseCategory category)
            {
                return category == ExpenseCategory.Egyéb ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}