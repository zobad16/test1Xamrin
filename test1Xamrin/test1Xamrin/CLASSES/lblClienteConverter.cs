using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using test1Xamrin;
using test1Xamrin.Resx;
using Xamarin.Forms;
using xUtilityPCL;

namespace test1Xamrin
{
    public class lblClienteConverter : IValueConverter
    {
        //da datarow a controllo
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string msg = AppResources.FormNuovoDocTapparePerInsModPag;

            if (value == null)//|| value.ToString() == AppResources.FormNuovoDocTapparePerInsModPag)
                return msg;
            else
            {
                var strTaxIDNumber = value.ToString();

                FEC_CustomersSystem drCliente;
                using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                {
                    drCliente = cn.Table<FEC_CustomersSystem>().FirstOrDefault(y => y.TaxIDNumber == strTaxIDNumber);
                    cn.Close();
                }

                if (drCliente != null)
                    return drCliente.CompanyName;
                else
                    return value;
            }
        }

        //da controllo a datarow
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == AppResources.FormNuovoDocTapparePerInsModPag)
                return null;
            else
            {
                var strCompanyName = value.ToString();
                FEC_CustomersSystem drCliente;
                using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                {
                    drCliente = cn.Table<FEC_CustomersSystem>().FirstOrDefault(y => y.CompanyName == strCompanyName);
                    cn.Close();
                }
                if (drCliente != null)
                    return drCliente.TaxIDNumber;
                else
                    return null;
            }
        }
    }
}
