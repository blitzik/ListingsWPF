﻿using Listings.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Listings.Converters
{
    public class TimeToHoursAndMinutesRangeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null) {
                return "";
            }
            Time start = (Time)values[0];
            Time end = (Time)values[1];

            return string.Format("{0} - {1}", start.HoursAndMinutes, end.HoursAndMinutes);
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}