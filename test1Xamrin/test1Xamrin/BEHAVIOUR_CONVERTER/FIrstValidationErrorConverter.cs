using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace xUtilityPCL
{
    public class FirstValidationErrorConverter : IValueConverter //20180510
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ICollection<string> errors = value as ICollection<string>;
            //20180515 inizio
            var messaggio = "";
            if (errors != null && errors.Count > 0)
            {
                foreach (var s in errors)
                {
                    messaggio += s + Environment.NewLine;
                }
            }
            return messaggio;
            //20180515 fine
            //20180515 return errors != null && errors.Count > 0 ? errors.ElementAt(0) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
