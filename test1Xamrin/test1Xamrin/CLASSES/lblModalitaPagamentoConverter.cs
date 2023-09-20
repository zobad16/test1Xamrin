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
    public class lblModalitaPagamentoConverter : IValueConverter
    {
        //da datarow a controllo
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string msg = AppResources.FormNuovoDocTapparePerInsModPag;

            if (value == null)
                return msg;
            else
            {
                //GUARDARE
                Int32 valueToSearch = System.Convert.ToInt32(value);
                FEC_PaymentTerms drPagamento;
                using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                {
                    drPagamento = cn.Table<FEC_PaymentTerms>().FirstOrDefault(y => y.IDPaymentTerms == valueToSearch);
                    cn.Close();
                }

                return drPagamento.PaymentDescription;
            }
        }

        //da controllo a datarow
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == AppResources.FormNuovoDocTapparePerInsModPag)
                return null;
            else
            {
                string descriptionToSearch = value.ToString();
                FEC_PaymentTerms drPagamento;
                using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                {
                    drPagamento = cn.Table<FEC_PaymentTerms>().FirstOrDefault(y => y.PaymentDescription == descriptionToSearch);
                    cn.Close();
                }
                if (drPagamento != null)
                    return drPagamento.IDPaymentTerms;
                else
                    return null;
            }
        }
    }
}
