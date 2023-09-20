using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using test1Xamrin.Resx;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xUtilityPCL;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SceltaProdotti : ContentPage
    {
        public Boolean isLoaded { get; set; }
        public SceltaProdottiItemsWrapper mySceltaProdottiItemsWrapper { get; set; }
        public FEC_InvoiceRequests drTestata { get; set; }
        //public ICommand cmdSceltaPrezzoServito { get; set; }
        //public ICommand cmdSceltaPrezzoSelf { get; set; }
        public ICommand cmdSelezionaCarburante { get; set; }
        public ICommand cmdSearchBar { get; set; }
        //private Boolean lClicked = true;

        public SceltaProdotti(FEC_InvoiceRequests _drTestata)
        {
            InitializeComponent();

            this.mySceltaProdottiItemsWrapper = new SceltaProdottiItemsWrapper();
            this.drTestata = _drTestata;
            //this.cmdSceltaPrezzoServito = new Command<FEC_Items>(async (item) => await OnClickServito(item));
            //this.cmdSceltaPrezzoSelf = new Command<FEC_Items>(async (item) => await OnClickSelf(item));
            this.cmdSelezionaCarburante = new Command<FEC_Items>(async (item) => await OnSelectedItem(item));
            this.cmdSearchBar = new Command<string>(async (text) => await cercaProdotto(text));

            this.BindingContext = this;
        }

        async Task cercaProdotto(string myText)
        {
            //https://xamarinhelp.com/xamarin-forms-search-bar/
            await Task.Delay(1);

            if (string.IsNullOrEmpty(myText))
            {
                //ripristino
                this.mySceltaProdottiItemsWrapper.ProdottiItems = new ObservableCollection<FEC_Items>(this.mySceltaProdottiItemsWrapper.ProdottiItemsClone);
            }
            else
            {
                //filtro
                var l = this.mySceltaProdottiItemsWrapper.ProdottiItemsClone.Where
                                                    (x =>
                                                        (
                                                         (x as FEC_Items).ItemDescription != null &&
                                                         (x as FEC_Items).ItemDescription.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                        ).ToList();
                this.mySceltaProdottiItemsWrapper.ProdottiItems = new ObservableCollection<FEC_Items>(l);
            }
        }

        public async Task CreaLista()
        {
            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                List<FEC_Items> l = null;

                l = cn.Table<FEC_Items>().Where(y => y.IsFuel == true && y.CodiceValore != null).OrderBy(X => X.ItemDescription).ToList();

                this.mySceltaProdottiItemsWrapper.ProdottiItems = new ObservableCollection<FEC_Items>(l);
                this.mySceltaProdottiItemsWrapper.ProdottiItemsClone = new ObservableCollection<FEC_Items>(l);

                foreach (var dr in l)
                {
                    if (string.IsNullOrEmpty(dr.CodiceTipo) == false)
                    {
                        if (dr.CodiceTipo == "CARB" && string.IsNullOrEmpty(dr.CodiceValore) == false) //20180924
                            dr.selfColumnIsVisibleUnbound = true;
                        else
                            dr.selfColumnIsVisibleUnbound = false;
                    }
                    else
                        dr.selfColumnIsVisibleUnbound = false;

                    if (dr.selfColumnIsVisibleUnbound == true)
                        dr.titoloColonnaServitoUnbound = AppResources.FormPrezziLblServito;
                    else
                        dr.titoloColonnaServitoUnbound = AppResources.FormPrezziLblServitoVersioneNoSelf;
                }

                cn.Close();
            }
        }

        async Task OnSelectedItem(FEC_Items item)
        {
            await gestisciCalcoli(item.UnitPriceB, item);
        }

        //public async Task OnClickSelf(FEC_Items item)
        //{
        //    await gestisciCalcoli(item.UnitPriceB, item);
        //}

        //public async Task OnClickServito(FEC_Items item)
        //{
        //    await gestisciCalcoli(item.UnitPriceA, item);
        //}

        private async Task gestisciCalcoli(Decimal? prezzoCarburante, FEC_Items item)
        {
            this.drTestata.ItemCode = item.ItemCode;
            this.drTestata.ItemCodeDescription = item.ItemDescription;
            this.drTestata.TaxCode = item.TaxCode;

            //20180830 inizio
            if (drTestata.TaxIDNumber != null)
            {
                using (var cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    var drCustomer = cn.Table<FEC_CustomersSystem>().FirstOrDefault(y => y.TaxIDNumber == drTestata.TaxIDNumber);
                    if (string.IsNullOrEmpty(drCustomer.TaxCodeDefault) == false)
                    {
                        if (drCustomer.TaxCodeDefault == "N3")
                            this.drTestata.TaxCode = drCustomer.TaxCodeDefault;
                    }

                    cn.Close();
                }
            }
            //20180830 fine

            if (string.IsNullOrEmpty(item.UnitaMisura) == false)
                this.drTestata.UnitaMisuraUnbound = "(" + item.UnitaMisura + ")";

            //20180901 inizio
            if (string.IsNullOrEmpty(item.ItemDescription) == false)
                this.drTestata.DescrPrdottoUnbound = item.ItemDescription;
            //20180901 fine

            //abcd
            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                var drTaxCode = cn.Table<FEC_TaxCode>().FirstOrDefault(y => y.TaxCode == drTestata.TaxCode); //20180830 era item.TaxCode
                decimal aliquota = 0;
                if (drTaxCode != null)
                {
                    aliquota = drTaxCode.TaxPerc.Value;
                }
                drTestata.UnitPriceVAT = prezzoCarburante.Value;
                cn.Close();

                RicalcolaPrezzo(aliquota, drTestata);
            }

            await this.Navigation.PopAsync(true);
        }

        public static void RicalcolaPrezzo(Decimal aliquota, FEC_InvoiceRequests drTestata)
        {
            var imponibile = drTestata.TotalAmount / ((aliquota / 100) + 1);
            drTestata.TaxableAmount = Math.Round(imponibile.Value, 5);
            drTestata.VATAmount = drTestata.TotalAmount - imponibile;
            var prezzounitariosenzaIVA = drTestata.UnitPriceVAT / ((aliquota / 100) + 1);
            drTestata.UnitPrice = Math.Round(prezzounitariosenzaIVA.Value, 5);
            if (drTestata.UnitPriceVAT != 0) //20181004
                drTestata.Qty = Math.Round(drTestata.TotalAmount.Value / drTestata.UnitPriceVAT.Value, 5);
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            if (isLoaded == false)
            {
                await CreaLista();
                isLoaded = true;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //this.lstProdotti.SelectedItem = null;
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class SceltaProdottiItemsWrapper
    {
        public SceltaProdottiItemsWrapper()
        {
            this.ProdottiItems = new ObservableCollection<FEC_Items>();
            this.ProdottiItemsClone = new ObservableCollection<FEC_Items>();
        }

        public ObservableCollection<FEC_Items> ProdottiItems { get; set; }
        public ObservableCollection<FEC_Items> ProdottiItemsClone { get; set; }
    }
}