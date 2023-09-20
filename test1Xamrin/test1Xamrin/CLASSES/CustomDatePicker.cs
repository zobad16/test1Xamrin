using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Syncfusion.SfPicker.XForms;
using System.Collections;
using test1Xamrin.Resx;
using System.Diagnostics;

namespace test1Xamrin
{
    public class CustomDatePicker : SfPicker

    {

        #region Public Properties

        // Months API is used to modify the Day collection as per change in Month

        internal Dictionary<string, string> Months { get; set; }

        /// <summary>

        /// Date is the actual DataSource for SfPicker control which will holds the collection of Day ,Month and Year

        /// </summary>

        /// <value>The date.</value>

        public ObservableCollection<object> Date { get; set; }

        //Day is the collection of day numbers

        internal ObservableCollection<object> Day { get; set; }

        //Month is the collection of Month Names

        internal ObservableCollection<object> Month { get; set; }

        //Year is the collection of Years from 1990 to 2042

        internal ObservableCollection<object> Year { get; set; }


        public ObservableCollection<string> Headers { get; set; }

        #endregion

        public CustomDatePicker()

        {

            Months = new Dictionary<string, string>();

            Date = new ObservableCollection<object>();

            Day = new ObservableCollection<object>();

            Month = new ObservableCollection<object>();

            Year = new ObservableCollection<object>();

            PopulateDateCollection();

            this.ItemsSource = Date;
            //hook selection changed event

            this.SelectionChanged += CustomDatePicker_SelectionChanged;

            Headers = new ObservableCollection<string>();

            var pickerParolaMese = AppResources.DatePickerParolaMese;
            var pickerParolaAnno = AppResources.DatePickerParolaAnno;
            var pickerParolaGiorno = AppResources.DatePickerParolaGiorno;
            var pickerTitolo = AppResources.DatePickerTitolo;

            Headers.Add(pickerParolaMese);

            Headers.Add(pickerParolaGiorno);

            Headers.Add(pickerParolaAnno);

            //SfPicker header text

            HeaderText = pickerTitolo;



            // Column header text collection

            this.ColumnHeaderText = Headers;


            //Enable Footer

            ShowFooter = true;

            //Enable SfPicker Header

            ShowHeader = true;

            //Enable Column Header of SfPicker

            ShowColumnHeader = true;


        }

        private void PopulateDateCollection()

        {

            //populate months

            for (int i = 1; i < 13; i++)

            {

                if (!Months.ContainsKey(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3)))

                    Months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3), CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i));

                Month.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3));

            }

            //populate year

            for (int i = 1990; i < 2050; i++)

            {

                Year.Add(i.ToString());

            }

            //populate Days

            //for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            for (int i = 1; i <= 31; i++)
            {

                if (i < 10)

                {

                    Day.Add("0" + i);

                }

                else

                    Day.Add(i.ToString());

            }

            Date.Add(Month);

            Date.Add(Day);

            Date.Add(Year);

        }



        private void CustomDatePicker_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)

        {

            var newdate = (sender as CustomDatePicker).SFPickerFormat2Date(e.NewValue as ObservableCollection<object>);
            if (newdate.HasValue == false)
            {
                var gg = Convert.ToInt32(((sender as CustomDatePicker).SelectedItem as ObservableCollection<object>)[1]);
                var mmm = ((sender as CustomDatePicker).SelectedItem as ObservableCollection<object>)[0].ToString();
                var mm = GetMonthFromMMM(mmm);
                var aaaa = Convert.ToInt32(((sender as CustomDatePicker).SelectedItem as ObservableCollection<object>)[2]);
                //if dei casi
                if (mm == 4 || mm == 6 || mm == 9 || mm == 11)
                    gg = 30;
                if (mm == 2)
                {
                    if (DateTime.IsLeapYear(aaaa))
                        gg = 29;
                    else
                        gg = 28;
                }
                var correctDate = new DateTime(aaaa, mm, gg);

                (sender as CustomDatePicker).SelectedItem = (sender as CustomDatePicker).date2SFPickerFormat(correctDate);

                //UpdateDays(Date, e);

            }
        }

        //Update days method is used to alter the Date collection as per selection change in Month column(if Feb is Selected day collection has value from 1 to 28)

        public void UpdateDays(ObservableCollection<object> Date, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)

        {

            Device.BeginInvokeOnMainThread(() =>

            {

                try

                {

                    bool update = false;

                    if (e.OldValue != null && e.NewValue != null && (e.OldValue as IList).Count > 0 && (e.NewValue as IList).Count > 0)

                    {

                        if ((e.OldValue as IList)[0] != (e.NewValue as IList)[0])

                        {

                            update = true;

                        }

                        if ((e.OldValue as IList)[2] != (e.NewValue as IList)[2])

                        {

                            update = true;

                        }

                    }

                    if (update)

                    {

                        ObservableCollection<object> days = new ObservableCollection<object>();

                        int month = DateTime.ParseExact(Months[(e.NewValue as IList)[0].ToString()], "MMMM", CultureInfo.InvariantCulture).Month;

                        int year = int.Parse((e.NewValue as IList)[2].ToString());

                        for (int j = 1; j <= DateTime.DaysInMonth(year, month); j++)

                        {

                            if (j < 10)

                            {

                                days.Add("0" + j);

                            }

                            else

                                days.Add(j.ToString());

                        }

                        if (days.Count > 0)

                        {

                            Date.RemoveAt(1);

                            Date.Insert(1, days);

                        }

                    }

                }

                catch (Exception ex)

                {
                    //var abc = 1;
                    Debug.WriteLine("Errore: ", ex.ToString());
                }

            });

        }


        public ObservableCollection<object> date2SFPickerFormat(DateTime mydate)
        {
            ObservableCollection<object> todaycollection = new ObservableCollection<object>();

            //Select today dates
            todaycollection.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mydate.Month).Substring(0, 3));
            if (mydate.Day < 10)
                todaycollection.Add("0" + mydate.Day);
            else
                todaycollection.Add(mydate.Day.ToString());

            todaycollection.Add(mydate.Year.ToString());

            return todaycollection;
        }

        public DateTime? SFPickerFormat2Date(ObservableCollection<object> obs)
        {

            //Dictionary<string, Int32> myDict = new Dictionary<string, int>();
            //for (int i = 1; i < 12; i++)
            //{
            //  var str = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3);
            //  myDict.Add(str, i);
            //}

            //var mm = myDict[obs[0].ToString()];

            var mm = GetMonthFromMMM(obs[0].ToString());
            var dd = Convert.ToInt32(obs[1]);
            var yy = Convert.ToInt32(obs[2]);

            DateTime? d = null;

            try
            {
                d = new DateTime(yy, mm, dd);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: ", ex.ToString());

            }


            return d;

        }


        Int32 GetMonthFromMMM(string mmm)
        {

            Dictionary<string, Int32> myDict = new Dictionary<string, int>();
            for (int i = 1; i <= 12; i++)
            {
                var str = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3);
                myDict.Add(str, i);
            }

            var mm = myDict[mmm];
            return mm;
        }
    }
}
