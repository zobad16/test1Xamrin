using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using xUtilityPCL;

namespace test1Xamrin
{
    public class lblDataScontrinoConverter : IValueConverter
    {
        //da datarow a controllo
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = (value as DateTime?).Value;
            var strData = data.ToString(Utility.getDateFormat());
            return strData;
        }

        //da controllo a datarow
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = System.Convert.ToDateTime(value);
            return date;
        }
    }
}
