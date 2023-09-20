using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using xUtilityPCL;

namespace test1Xamrin
{
    public class FakeConverterSettaEnabled : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) { }

            else
            {
                var sl = parameter as StackLayout;
                var val = System.Convert.ToBoolean(value);
                if (val == false)
                    sl.SettaEnabled(val);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
