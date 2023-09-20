using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace xUtilityPCL
{
    public class NumericConverter : IValueConverter
    {

        public NumericConverter()
        {

        }

        //dalla datatow al controllo
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Object retVal = 0;
            if (value == null)
                return value; // retVal;//20180617
            if (string.IsNullOrEmpty(value.ToString()))
                return value; // retVal; //20180617

            if (value is Decimal)
                retVal = (Decimal)value;
            if (value is Int32)
                retVal = (Int32)value;
            if (value is Double)        //20160627
                retVal = (Double)value; //20160627
            return retVal;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (string.IsNullOrEmpty(value.ToString()))
            {
                if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                {
                    Decimal v = 0;
                    return v;
                }
                if (targetType == typeof(Int32) || targetType == typeof(Int32?))
                {
                    Int32 v = 0;
                    return v;
                }
                //20160627 inizio
                if (targetType == typeof(Double) || targetType == typeof(Double?))
                {
                    Double v = 0;
                    return v;
                }
                //20160627 fine
            }

            var trimmedValue = value.ToString();
            if (targetType == typeof(decimal) || targetType == typeof(decimal?))
            {
                decimal result;
                if (decimal.TryParse(trimmedValue, out result))
                    return result;
                else
                    return value;
            }
            //20160627 inizio
            if (targetType == typeof(double) || targetType == typeof(double?))
            {
                double result;
                if (double.TryParse(trimmedValue, out result))
                    return result;
                else
                    return value;
            }
            //20160627 fine
            if (targetType == typeof(Int32) || targetType == typeof(Int32?))
            {
                Int32 result;
                if (Int32.TryParse(trimmedValue.Replace(",", "").Replace(".", ""), out result))
                    return result;
                else
                    return value;
            }
            return value;



        }
    }

    public class EmptyStringConverter : IValueConverter //20150618
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;
            if (value.ToString() == "") //20180526
                return null;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;
            if (value.ToString() == "") //20180526
                return null;
            return value;
        }
    }

    public class BooleanToIntConverter : IValueConverter
    {

        public BooleanToIntConverter()
        {

        }

        public object Convert(object value, Type targetType,
                             object parameter, CultureInfo culture)
        {
            return (int)value != 0;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }

    //20180211 inizio
    public class EmptyDateConverter : IValueConverter
    {
        //utile per il binding delle celle della listview
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //dalla datarow al controllo
            if (value == null)
                return "";
            else
                return ((DateTime)value).ToString(Utility.getDateFormat()); //20180630 "dd/MM/yyyy");

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }


    //20180225 inizio
    //https://forums.xamarin.com/discussion/74258/bind-an-entry-with-a-int-value
    public class StringToNullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //dalla datatow al controllo
            Object retVal = "";
            if (value == null)
                return retVal;
            if (string.IsNullOrEmpty(value.ToString()))
                return retVal;

            if (value is Decimal)
                retVal = (Decimal)value;
            if (value is Int32)
                retVal = (Int32)value;
            if (value is Double)        //20160627
                retVal = (Double)value; //20160627
            return retVal;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;

            if (string.IsNullOrEmpty(strValue))
                return null;

            var trimmedValue = value.ToString();
            if (targetType == typeof(decimal) || targetType == typeof(decimal?))
            {
                decimal result;
                if (decimal.TryParse(trimmedValue, out result))
                    return result;
                else
                    return value;
            }
            //20160627 inizio
            if (targetType == typeof(double) || targetType == typeof(double?))
            {
                double result;
                if (double.TryParse(trimmedValue, out result))
                    return result;
                else
                    return value;
            }
            //20160627 fine
            if (targetType == typeof(Int32) || targetType == typeof(Int32?))
            {
                Int32 result;
                if (Int32.TryParse(trimmedValue.Replace(",", "").Replace(".", ""), out result))
                    return result;
                else
                    return value;
            }

            return value;
        }
    }
    //20180225 fine


    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueAsString = value.ToString();
            switch (valueAsString)
            {
                case (""):
                    {
                        return Color.Default;
                    }
                case ("Accent"):
                    {
                        return Color.Accent;
                    }
                default:
                    {
                        return Color.FromHex(value.ToString());
                    }
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }






}

