using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace xUtilityPCL
{
    public class Utility
    {
        public Utility()
        {
        }


        public static double GetFactor()
        {
            double factor = 1;
            if (Device.RuntimePlatform == Device.Android)
            {
                var densi = DependencyService.Get<platformSpecific>().getDensity();
                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    factor = 1.807;
                }
                else
                {
                    if (densi >= 2)
                    {
                        factor = 1.125;
                    }
                }
            }
            if (Device.RuntimePlatform == Device.iOS)
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    factor = 2.089;
                }
                else
                {
                    if (DependencyService.Get<platformSpecific>().isiphone5())
                    {
                        factor = 1.096;
                    }
                    else
                    {
                        factor = 0.904;
                    }
                }
            }
            if (Device.OS == TargetPlatform.WinPhone)
                factor = 1.72;

            return factor;
        }

        public static Label Getwp8Title(string title)
        {
            var wp8title = new Label
            {
                Text = title,
                //Font = Font.SystemFontOfSize(20 * GetFactor()),// (42),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * GetFactor(),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                //HeightRequest = 25
                VerticalOptions = LayoutOptions.Start,
            };
            wp8title.HeightRequest = wp8title.FontSize;
            return wp8title;
        }

        public static NavigationPage getParentNavigationPage(View obj)
        {
            Int32 i = 1;
            VisualElement ve = obj;
            if (ve is NavigationPage)
                return ve as NavigationPage;

            while (ve is NavigationPage == false)
            {
                ve = ve.Parent as NavigationPage;
                i++;
                if (i > 20)
                    return null;
            }
            return ve as NavigationPage;

            /*
			((obj.ParentView.ParentView.Parent.ParentView.ParentView.ParentView.
				ParentView.ParentView.ParentView) as NavigationPage).CurrentPage.DisplayAlert ("Messaggio", "cliccato", "OK", "Annulla");
*/
        }

        public static Type GetPropertyTypeFromObject(Object obj, string propertyNameToFind)
        {
            var r = obj.GetType().GetRuntimeProperties();
            PropertyInfo p = null;
            foreach (PropertyInfo pi in r)
            {
                if (pi.Name.ToLower() == propertyNameToFind.ToLower())
                    p = pi;
            }
            if (p == null)
            {
                throw new Exception("Non trovato campo " + propertyNameToFind);
            }
            if (p.PropertyType.ToString().Contains("System.DateTime"))
                return typeof(DateTime);
            if (p.PropertyType.ToString() == "System.String")
                return typeof(string);
            if (p.PropertyType.ToString() == "System.Boolean")
                return typeof(Boolean);
            if (p.PropertyType.ToString().Contains("System.Decimal"))
                return typeof(Decimal);
            if (p.PropertyType.ToString().Contains("System.Int32"))
                return typeof(Int32);
            if (p.PropertyType.ToString().Contains("System.Double")) //20160627
                return typeof(Double);
            return null;
        }

        public static PropertyInfo GetPropertyInfoFromObject(Object obj, string propertyNameToFind)
        {
            var r = obj.GetType().GetRuntimeProperties();
            PropertyInfo p = null;
            foreach (PropertyInfo pi in r)
            {
                if (pi.Name.ToLower() == propertyNameToFind.ToLower())
                    p = pi;
            }

            return p;
        }

        public static Boolean isInternetAvailable()
        {
            //			var i = DependencyService.Get<Acr.XamForms.Mobile.Net.INetworkService> ();
            //			return i.IsConnected;
            return CrossConnectivity.Current.IsConnected;

        }

        public static Task<Boolean> isInternetAvailable2()
        {
            return CrossConnectivity.Current.IsRemoteReachable("https://www.google.com", 80, 5000);

        }

        public static bool CarattereSpeciale(char content)
        {
            bool bRes = false;
            try
            {
                int asciiCode = Asc(content);

                if ((asciiCode == 8) | (asciiCode == 95) | (asciiCode == 10) | (asciiCode == 32) | (asciiCode > 38 & asciiCode < 60) | (asciiCode > 64 & asciiCode < 91) | (asciiCode > 96 & asciiCode < 123))
                {
                    bRes = false;
                }
                else
                {
                    bRes = true;
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: ", ex.ToString());
                bRes = true;

            }

            return bRes;
        }

        public static bool CarattereSpecialeSoloNumeriLettere(char content) //20170119
        {
            bool bRes = false;
            try
            {
                int asciiCode = Asc(content);

                if ((asciiCode > 47 & asciiCode < 58) | (asciiCode > 64 & asciiCode < 91) | (asciiCode > 96 & asciiCode < 123))
                {
                    bRes = false;
                }
                else
                {
                    bRes = true;
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: ", ex.ToString());
                bRes = true;

            }

            return bRes;
        }

        public static bool CarattereSpecialeSoloNumeriLetterePiuCaratteriSpecialiTargheStraniere(char content) //20170119
        {
            //ammette, oltre a numeri e lettere, anche i caratteri ',' '-' '.' e spazio
            bool bRes = false;
            try
            {
                int asciiCode = Asc(content);

                if ((asciiCode > 47 & asciiCode < 58) | (asciiCode > 64 & asciiCode < 91) | (asciiCode > 96 & asciiCode < 123) |
                    asciiCode == 44 | asciiCode == 45 | asciiCode == 46 | asciiCode == 32)
                {
                    bRes = false;
                }
                else
                {
                    bRes = true;
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: ", ex.ToString());
                bRes = true;

            }

            return bRes;
        }




        static short Asc(char s)
        {
            return Encoding.UTF8.GetBytes(s.ToString())[0];
        }

        static string Chr(int CharCode)
        {
            //da provare; non sicuro che funzioni
            if (CharCode > 255)
                throw new ArgumentOutOfRangeException("CharCode", CharCode, "CharCode must be between 0 and 255.");
            List<byte> arr = new List<byte>();
            arr.Add(Convert.ToByte(CharCode));

            return Encoding.UTF8.GetString(arr.ToArray(), 0, 1);
        }





        public static DateTime getDateSenzaTimezone(DateTime d)
        {
            DateTime d2 = DateTime.MinValue;
            if (d.Kind == DateTimeKind.Utc)
            {
                d2 = new DateTime(
                    d.ToLocalTime().Year,
                    d.ToLocalTime().Month,
                    d.ToLocalTime().Day);
            }
            if (d.Kind == DateTimeKind.Local)
            {
                d2 = new DateTime(
                    d.Year,
                    d.Month,
                    d.Day);
            }
            if (d.Kind == DateTimeKind.Unspecified)
            {
                d2 = new DateTime(
                    d.Year,
                    d.Month,
                    d.Day);
            }
            return d2;
        }


        #region Validation methods



        public static bool isEmail(string value)
        {
            var pattern = @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            //var pattern = @"^([\w\d\-\.]+)@{1}(([\w\d\-]{1,67})| ([\w\d\-]+\.[\w\d\-]{1,67}))\.(([a-zA-Z\d]{2,4})(\.[a-zA-Z\d]{2})?)$";
            return Regex.IsMatch(value, pattern);
        }

        public static bool isText(string value)
        {
            var pattern = @"^[a-zA-Z áéíóúAÉÍÓÚÑñ]+$";
            return Regex.IsMatch(value, pattern);
        }

        public static bool IsUrlValid(string url)
        {
            return Regex.IsMatch(url, @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }

        public static bool isNumeric(string value)
        {
            var pattern = @"^[0-9]+$";
            return Regex.IsMatch(value, pattern);
        }

        public static bool isAlphanumeric(string value)
        {
            var pattern = @"^[a-zA-Z0-9 áéíóúAÉÍÓÚÑñ]+$";
            return Regex.IsMatch(value, pattern);
        }

        public static bool isCreditCardNumber(string value)
        {
            if (value == null)
                return false;
            return (isNumeric(value) && value.Length <= 16);
        }

        public static bool isCreditCardCode(string value)
        {
            if (value == null)
                return false;
            return (isNumeric(value) && value.Length <= 3);
        }

        public static bool isEmpty(string value)
        {
            return (value.Trim().Length == 0);
        }

        public static bool isDate(string value)
        {
            var pattern = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";
            return Regex.IsMatch(value, pattern);
        }

        #endregion


        public static string getDateFormat() //20180630
        {
            var finalFormat = "";
            var dtfi = CultureInfo.CurrentCulture.DateTimeFormat as DateTimeFormatInfo;
            var separator = "/";
            var arrDate = dtfi.ShortDatePattern.Split(separator.ToCharArray());

            if (arrDate[0].ToLower().Contains("m"))
                finalFormat += "MM";
            if (arrDate[0].ToLower().Contains("d"))
                finalFormat += "dd";
            if (arrDate[0].ToLower().Contains("y"))
                finalFormat += "yyyy";
            finalFormat += separator;

            if (arrDate[1].ToLower().Contains("m"))
                finalFormat += "MM";
            if (arrDate[1].ToLower().Contains("d"))
                finalFormat += "dd";
            if (arrDate[1].ToLower().Contains("y"))
                finalFormat += "yyyy";
            finalFormat += separator;


            if (arrDate[2].ToLower().Contains("m"))
                finalFormat += "MM";
            if (arrDate[2].ToLower().Contains("d"))
                finalFormat += "dd";
            if (arrDate[2].ToLower().Contains("y"))
                finalFormat += "yyyy";


            return finalFormat;
        }

    }

    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
                               System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                   System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }


}

