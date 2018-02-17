using Listings.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Listings.Converters
{
    public class ViewModelToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) {
                return Binding.DoNothing;
            }

            ScreenBaseViewModel viewModel = (ScreenBaseViewModel)value;
            string checkValue = viewModel.GetType().Name;
            string targetValue = (string)parameter;

            return checkValue.Equals(targetValue, StringComparison.InvariantCultureIgnoreCase);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
