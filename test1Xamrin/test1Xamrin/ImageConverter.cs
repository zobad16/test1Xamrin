using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace test1Xamrin
{
    public class ImageConverter : IValueConverter
    {
        //da datarow a controllo
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            else
            {
                var path = value.ToString();
                path = path.TrimStart("IMG-Path:".ToCharArray());
                return path;
            }
        }

        //da controllo a datarow
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;

        }
    }
}
