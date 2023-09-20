using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using test1Xamrin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xUtilityPCL;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Stati : ContentPage
    {
        public ICommand cmdStati { get; set; }
        public ICommand cmdSearchBar { get; set; }
        public StatiItemsWrapper myStatiItemsWrapper { get; set; }
        public FEC_CustomersSystem drCustomer { get; set; }

        public Stati(FEC_CustomersSystem _drCustomer)
        {
            InitializeComponent();

            drCustomer = _drCustomer;
            myStatiItemsWrapper = new StatiItemsWrapper();
            cmdStati = new Command<FEC_Countries>(async (item) => await OnSelectedItem(item));
            cmdSearchBar = new Command<string>(async (text) => await cercaStato(text));

            this.BindingContext = this;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            await CreaLista();
        }

        public async Task CreaLista()
        {
            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                var l = cn.Table<FEC_Countries>().OrderBy(x => x.CountryDescription).ToList();

                foreach (var dr in l)
                {
                    if (string.IsNullOrEmpty(dr.iso_3166_2) == false)
                        dr.DescrStatoCompletaUnbound = dr.CountryDescription + " (" + dr.iso_3166_2 + ")";
                }

                this.myStatiItemsWrapper.StatiItems = new ObservableCollection<FEC_Countries>(l);
                this.myStatiItemsWrapper.StatiItemsClone = new ObservableCollection<FEC_Countries>(l);

                cn.Close();
            }
        }

        async Task OnSelectedItem(FEC_Countries item)
        {
            drCustomer.Country_VAL.Value = item.CountryDescription;
            drCustomer.Country_VAL.IsValid = true;
            drCustomer.StateCode2_VAL.Value = item.iso_3166_2;
            drCustomer.StateCode2_VAL.IsValid = true;
            await Navigation.PopAsync(true);
        }

        async Task cercaStato(string myText)
        {
            //https://xamarinhelp.com/xamarin-forms-search-bar/
            await Task.Delay(1);

            if (string.IsNullOrEmpty(myText))
            {
                //ripristino
                this.myStatiItemsWrapper.StatiItems = new ObservableCollection<FEC_Countries>(this.myStatiItemsWrapper.StatiItemsClone);
            }
            else
            {
                //filtro
                var l = this.myStatiItemsWrapper.StatiItemsClone.Where
                                                    (x =>
                                                        (
                                                         (x as FEC_Countries).CountryDescription != null &&
                                                         (x as FEC_Countries).CountryDescription.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                        ).ToList();
                this.myStatiItemsWrapper.StatiItems = new ObservableCollection<FEC_Countries>(l);
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class StatiItemsWrapper
    {
        public StatiItemsWrapper()
        {
            this.StatiItems = new ObservableCollection<FEC_Countries>();
            this.StatiItemsClone = new ObservableCollection<FEC_Countries>();
        }
        public ObservableCollection<FEC_Countries> StatiItems { get; set; }
        public ObservableCollection<FEC_Countries> StatiItemsClone { get; set; }
    }
}