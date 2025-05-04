using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using FinancialTracker.Models;

namespace FinancialTracker.Converters
{
    public class TotalPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<CategorySettingItem> settings)
            {
                return settings.Sum(s => s.MaxPercentage);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LessThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            decimal number1;
            decimal number2;

            if (decimal.TryParse(value.ToString(), out number1) && decimal.TryParse(parameter.ToString(), out number2))
            {
                return number1 < number2;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GreaterThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            decimal number1;
            decimal number2;

            if (decimal.TryParse(value.ToString(), out number1) && decimal.TryParse(parameter.ToString(), out number2))
            {
                return number1 > number2;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}