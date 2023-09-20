using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using test1Xamrin.Resx;
using Xamarin.Forms;

namespace test1Xamrin
{
    public class lblProdottoConverter : IValueConverter
    {
        //da datarow a controllo
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string msg = AppResources.FormNuovoDocUsareListaPerInsModCodiceProdotto;

            if (value == null)
                return msg;
            else
            {
                return value;
            }
        }

        //da controllo a datarow
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
