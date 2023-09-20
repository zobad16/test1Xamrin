using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using test1Xamrin;
using test1Xamrin.Resx;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xUtilityPCL;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Province : ContentPage
    {
        public ICommand cmdProvince { get; set; }
        public ICommand cmdPulisci { get; set; }
        public ICommand cmdSearchBar { get; set; }
        public ProvinceItemsWrapper myProvinceItemsWrapper { get; set; }
        public FEC_CustomersSystem drCustomer { get; set; }

        public Province(FEC_CustomersSystem _drCustomer)
        {
            InitializeComponent();

            drCustomer = _drCustomer;
            myProvinceItemsWrapper = new ProvinceItemsWrapper();
            cmdProvince = new Command<FEC_Counties>(async (item) => await OnSelectedItem(item));
            cmdPulisci = new Command(async () => await PulisciProvincia());
            cmdSearchBar = new Command<string>(async (text) => await cercaProvincia(text));

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
                var l = cn.Table<FEC_Counties>().OrderBy(x => x.CountyDescription).ToList();

                foreach (var dr in l)
                {
                    if (string.IsNullOrEmpty(dr.CountyCode) == false)
                        dr.DescrProvinciaCompletaUnbound = dr.CountyDescription + " (" + dr.CountyCode + ")";
                }

                this.myProvinceItemsWrapper.ProvinceItems = new ObservableCollection<FEC_Counties>(l);
                this.myProvinceItemsWrapper.ProvinceItemsClone = new ObservableCollection<FEC_Counties>(l);

                cn.Close();
            }
        }

        async Task OnSelectedItem(FEC_Counties item)
        {
            drCustomer.County2_VAL.Value = item.CountyCode;
            drCustomer.County2_VAL.IsValid = true;
            await Navigation.PopAsync(true);
        }

        async Task PulisciProvincia()
        {
            drCustomer.County2_VAL.Value = AppResources.FormClientiTestoSelezionaProvincia;
            //drCustomer.County2_VAL.IsValid = false;
            await Navigation.PopAsync(true);
        }

        async Task cercaProvincia(string myText)
        {
            //https://xamarinhelp.com/xamarin-forms-search-bar/
            await Task.Delay(1);

            if (string.IsNullOrEmpty(myText))
            {
                //ripristino
                this.myProvinceItemsWrapper.ProvinceItems = new ObservableCollection<FEC_Counties>(this.myProvinceItemsWrapper.ProvinceItemsClone);
            }
            else
            {
                //filtro
                var l = this.myProvinceItemsWrapper.ProvinceItemsClone.Where
                                                    (x =>
                                                        (
                                                         (x as FEC_Counties).CountyDescription != null &&
                                                         (x as FEC_Counties).CountyDescription.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                        ).ToList();
                this.myProvinceItemsWrapper.ProvinceItems = new ObservableCollection<FEC_Counties>(l);
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class ProvinceItemsWrapper
    {
        public ProvinceItemsWrapper()
        {
            this.ProvinceItems = new ObservableCollection<FEC_Counties>();
            this.ProvinceItemsClone = new ObservableCollection<FEC_Counties>();
        }
        public ObservableCollection<FEC_Counties> ProvinceItems { get; set; }
        public ObservableCollection<FEC_Counties> ProvinceItemsClone { get; set; }
    }
}