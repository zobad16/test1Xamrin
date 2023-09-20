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
    public partial class Benzinai : ContentPage
    {
        public ICommand cmdBenzinai { get; set; }
        public ICommand cmdPulisci { get; set; }
        public ICommand cmdSearchBar { get; set; }
        public BenzinaiItemsWrapper myBenzinaiItemsWrapper { get; set; }
        public FEC_InvoiceRequests drInvoiceRequest { get; set; }
        public Boolean leggiProdottiModPagamentoRESTAPI { get; set; }
        public ObservableCollection<FEC_PaymentTerms> ModalitaPagamento { get; set; }
        public ObservableCollection<string> ModalitaPagamentoDDL { get; set; }
        Syncfusion.SfPicker.XForms.SfPicker ddlModalitaPagamento { get; set; }
        public DaPassareBenzinaiWrapper myDaPassareBenzinaiWrapper { get; set; }

        public Benzinai(FEC_InvoiceRequests _drInvoiceRequest, Boolean _leggiProdottiModPagamentoRESTAPI,
                         ObservableCollection<FEC_PaymentTerms> _ModalitaPagamento, ObservableCollection<string> _ModalitaPagamentoDDL,
                         Syncfusion.SfPicker.XForms.SfPicker _ddlModalitaPagamento)
        {
            InitializeComponent();

            this.drInvoiceRequest = _drInvoiceRequest;
            this.leggiProdottiModPagamentoRESTAPI = _leggiProdottiModPagamentoRESTAPI;
            this.ModalitaPagamento = _ModalitaPagamento;
            this.ModalitaPagamentoDDL = _ModalitaPagamentoDDL;
            this.ddlModalitaPagamento = _ddlModalitaPagamento;
            this.myBenzinaiItemsWrapper = new BenzinaiItemsWrapper();
            this.cmdBenzinai = new Command<FEC_Activation>(async (item) => await OnSelectedItem(item));
            this.cmdPulisci = new Command(async () => await PulisciBenzinaio());
            this.cmdSearchBar = new Command<string>(async (text) => await cercaBenzinaio(text));

            this.BindingContext = this;
        }
        public Benzinai(DaPassareBenzinaiWrapper _myDaPassareBenzinaiWrapper)
        {
            InitializeComponent();

            this.myDaPassareBenzinaiWrapper = _myDaPassareBenzinaiWrapper;
            this.myBenzinaiItemsWrapper = new BenzinaiItemsWrapper();
            this.cmdBenzinai = new Command<FEC_Activation>(async (item) => await OnSelectedItem(item));
            this.cmdPulisci = new Command(async () => await PulisciBenzinaio());
            this.cmdSearchBar = new Command<string>(async (text) => await cercaBenzinaio(text));

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
                var l = cn.Table<FEC_Activation>().OrderBy(x => x.CompanyName).ToList();

                foreach (var dr in l)
                {
                    dr.TaxIDNumberCompanyNameUnbound = dr.CompanyName + "-" + dr.TaxIDNumber;
                    CreaIndirizzoCompleto(dr);
                }

                this.myBenzinaiItemsWrapper.BenzinaiItems = new ObservableCollection<FEC_Activation>(l);
                this.myBenzinaiItemsWrapper.BenzinaiItemsClone = new ObservableCollection<FEC_Activation>(l);

                cn.Close();
            }
        }

        async Task OnSelectedItem(FEC_Activation item)
        {
            if (drInvoiceRequest != null)
            {
                settaBenzinaio(drInvoiceRequest, item);

                if (leggiProdottiModPagamentoRESTAPI == true)
                {
                    popolaTabelleRestDopoSceltaBenzinaio(item, this.ModalitaPagamento, this.ModalitaPagamentoDDL, this.ddlModalitaPagamento);
                }
            }
            else
            {
                this.myDaPassareBenzinaiWrapper.BenzinaioParam = item.CompanyName;
            }

            await Navigation.PopAsync(true);
        }

        async public static void popolaTabelleRestDopoSceltaBenzinaio(FEC_Activation item, ObservableCollection<FEC_PaymentTerms> ModalitaPagamento,
                                                                      ObservableCollection<string> ModalitaPagamentoDDL,
                                                                      Syncfusion.SfPicker.XForms.SfPicker ddlModalitaPagamento,
                                                                     Boolean lChiamaRest = true)
        {
            if (lChiamaRest)
            {
                await WSChiamateEF.getItemsBenzinaio(item.IDCompany);
                await WSChiamateEF.getPaymentTermsBenzinaio(item.IDCompany);
                await WSChiamateEF.getTaxCodesBenzinaio(item.IDCompany);
            }
            using (var cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                var l = cn.Table<FEC_Items>().ToList();
                var ll = cn.Table<FEC_PaymentTerms>().ToList();
                var lll = cn.Table<FEC_TaxCode>().ToList();


                var nElementi = 0;
                if (ModalitaPagamento.Count == 0) //20181128 introdotto IF e ELSE
                {
                    ModalitaPagamento.Clear();
                    ModalitaPagamentoDDL.Clear();
                }
                else
                {
                    nElementi = ModalitaPagamento.Count;
                }

                if (Device.RuntimePlatform == Device.iOS)
                {
                    //ddlModalitaPagamento.SelectedItem = null; //20181128
                    //ddlModalitaPagamento.SelectedIndex = null; //20181128
                    ddlModalitaPagamento.SelectedIndex = -1;
                }

                foreach (var drPaymentTerm in ll)
                {
                    ModalitaPagamento.Add(drPaymentTerm);
                    ModalitaPagamentoDDL.Add(drPaymentTerm.PaymentDescription);
                }

                //20181128 inizio
                if (nElementi > 0)
                {
                    for (int i = 0; i < nElementi; i++)
                    {
                        ModalitaPagamento.RemoveAt(0);
                        ModalitaPagamentoDDL.RemoveAt(0);
                    }
                }
                //20181128 fine

                cn.Close();
            }
        }

        async Task PulisciBenzinaio()
        {
            if (drInvoiceRequest != null)
            {
                drInvoiceRequest.IDCompany = null;
                drInvoiceRequest.IDCompanyTaxIDNumber = null;
                drInvoiceRequest.IDCompanyCompanyName = AppResources.FormNuovoDocTestoSelezionaBenzinaio;
                drInvoiceRequest.IDCompanyTaxIDNumberCompanyNameUnbound = AppResources.FormNuovoDocTestoSelezionaBenzinaio;
                drInvoiceRequest.IDCompanyFiscalCode = null;
                drInvoiceRequest.IDCompanyAddress = null;
                drInvoiceRequest.IDCompanyCity = null;
                drInvoiceRequest.IDCompanyIndirizzoCittaUnbound = null;
                drInvoiceRequest.IDCompanyZipCode = null;
                drInvoiceRequest.IDCompanyCounty = null;
                drInvoiceRequest.IDCompanyCountry = null;
                drInvoiceRequest.IDCompanyStateCode = null;
                drInvoiceRequest.IDCompanyTelephone = null;
                drInvoiceRequest.IDCompanyEmail = null;
                drInvoiceRequest.IDCompanyPEC = null;
            }
            else
            {
                this.myDaPassareBenzinaiWrapper.BenzinaioParam = "";
            }

            await Navigation.PopAsync(true);
        }

        async Task cercaBenzinaio(string myText)
        {
            //https://xamarinhelp.com/xamarin-forms-search-bar/
            await Task.Delay(1);

            if (string.IsNullOrEmpty(myText))
            {
                //ripristino
                this.myBenzinaiItemsWrapper.BenzinaiItems = new ObservableCollection<FEC_Activation>(this.myBenzinaiItemsWrapper.BenzinaiItemsClone);
            }
            else
            {
                //filtro
                var l = this.myBenzinaiItemsWrapper.BenzinaiItemsClone.Where
                                                    (x =>
                                                        (
                                                         (x as FEC_Activation).TaxIDNumberCompanyNameUnbound != null &&
                                                         (x as FEC_Activation).TaxIDNumberCompanyNameUnbound.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                     ||
                                                        (
                                                         (x as FEC_Activation).Address != null &&
                                                         (x as FEC_Activation).Address.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                     ||
                                                        (
                                                         (x as FEC_Activation).City != null &&
                                                         (x as FEC_Activation).City.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                        ).ToList();
                this.myBenzinaiItemsWrapper.BenzinaiItems = new ObservableCollection<FEC_Activation>(l);
            }
        }

        public static void CreaIndirizzoCompleto(FEC_Activation dr)
        {
            dr.IndirizzoCompleto = "";
            if (string.IsNullOrEmpty(dr.Address) == false)
                dr.IndirizzoCompleto += dr.Address;

            if (dr.IndirizzoCompleto != "")
                dr.IndirizzoCompleto += " - ";

            if (string.IsNullOrEmpty(dr.ZipCode) == false)
                dr.IndirizzoCompleto += dr.ZipCode;

            if (string.IsNullOrEmpty(dr.City) == false)
                dr.IndirizzoCompleto += " " + dr.City;

            if (string.IsNullOrEmpty(dr.County) == false)
                dr.IndirizzoCompleto += " (" + dr.County + ")";
        }

        public static void settaBenzinaio(FEC_InvoiceRequests drTestata, FEC_Activation drBenzinaio)
        {
            //IDCompanyIndirizzoCittaUnbound
            drTestata.IDCompany = drBenzinaio.IDCompany;
            drTestata.IDCompanyTaxIDNumber = drBenzinaio.TaxIDNumber;
            drTestata.IDCompanyCompanyName = drBenzinaio.CompanyName;
            drTestata.IDCompanyTaxIDNumberCompanyNameUnbound = drBenzinaio.TaxIDNumberCompanyNameUnbound;
            drTestata.IDCompanyFiscalCode = drBenzinaio.FiscalCode;
            drTestata.IDCompanyAddress = drBenzinaio.Address;
            drTestata.IDCompanyCity = drBenzinaio.City;

            NuovoDocumento.settaCampiUnboundTestata(drTestata);

            drTestata.IDCompanyZipCode = drBenzinaio.ZipCode;
            drTestata.IDCompanyCounty = drBenzinaio.County;
            drTestata.IDCompanyCountry = drBenzinaio.Country;
            drTestata.IDCompanyStateCode = drBenzinaio.StateCode;
            drTestata.IDCompanyTelephone = drBenzinaio.Telephone;
            drTestata.IDCompanyEmail = drBenzinaio.Email;
            drTestata.IDCompanyPEC = drBenzinaio.Pec;
        }

    }

    [AddINotifyPropertyChangedInterface]
    public class BenzinaiItemsWrapper
    {
        public BenzinaiItemsWrapper()
        {
            this.BenzinaiItems = new ObservableCollection<FEC_Activation>();
            this.BenzinaiItemsClone = new ObservableCollection<FEC_Activation>();
        }
        public ObservableCollection<FEC_Activation> BenzinaiItems { get; set; }
        public ObservableCollection<FEC_Activation> BenzinaiItemsClone { get; set; }
    }
}