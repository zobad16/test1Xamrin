using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using test1Xamrin;
using test1Xamrin.Resx;
using Xamarin.Forms;
using xUtilityPCL;

namespace test1Xamrin.ViewModels
{
    public class ClientiViewModel
    {
        INavigation navigation { get; set; }
        public ClientiItemsWrapper myClientiItemsWrapper { get; set; }
        public ICommand cmdClienti { get; set; }
        public ICommand cmdSearchBar { get; set; }
        public ICommand cmdAdd { get; set; }
        public ICommand cmdQR { get; set; }
        string abc = "";
        private Boolean lClicked = true;
        private Boolean lClickedQR = true;

        public ClientiViewModel(INavigation _navigation)
        {
            navigation = _navigation;
            myClientiItemsWrapper = new ClientiItemsWrapper();
            cmdClienti = new Command<FEC_CustomersSystem>(async (item) => await OnSelectedItem(item));
            cmdSearchBar = new Command<string>(async (text) => await cercaCliente(text));
            cmdAdd = new Command(async () => await aggiungiCliente());
            cmdQR = new Command(async () => await QRCliente());
        }
        public static void CreaIndirizzoCompleto(FEC_CustomersSystem dr)
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

        public async Task CreaLista()
        {
            //var el1 = new FEC_Customers() { CompanyName = "Alterbit s.r.l.", Address = "Via G. Crespi, 12 - 20134 Milano", TaxIDNumber = "P.IVA 12345678901", Email = "pec@legalmail.it" };
            //this.ClientiItems.Add(el1);

            //var el2 = new FEC_Customers() { CompanyName = "Alterbit s.r.l.", Address = "Via G. Crespi, 12 - 20134 Milano", TaxIDNumber = "P.IVA 12345678901", Email = "pec@legalmail.it" };
            //this.ClientiItems.Add(el2);

            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                var l = cn.Table<FEC_CustomersSystem>().OrderBy(x => x.CompanyName).ToList();
                this.myClientiItemsWrapper.ClientiItems = new ObservableCollection<FEC_CustomersSystem>(l);
                this.myClientiItemsWrapper.ClientiItemsClone = new ObservableCollection<FEC_CustomersSystem>(l);

                foreach (var dr in l)
                {
                    dr.RecapitoUnbound = AppResources.CellaClienteParolaRecapito + ": " + FECUtilita.getRecapito(dr); //20180901

                    //if (string.IsNullOrEmpty(dr.SID) == false)
                    //    dr.RecapitoUnbound = AppResources.CellaClienteParolaRecapito + ": " + dr.SID;
                    //else
                    //    dr.RecapitoUnbound = AppResources.CellaClienteParolaRecapito + ": " + dr.Pec;

                    CreaIndirizzoCompleto(dr);

                    /*
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
                    */
                }

                cn.Close();
            }
        }

        async Task OnSelectedItem(FEC_CustomersSystem item)
        {
            if (item == null)
                return;
            if (lClicked)
            {
                lClicked = false;
                await Task.Delay(1);
                if (item == null) return;
                item.StatoFormRispettoSqlite_IMC = "M";
                if (item.RowState == xUtilityPCL.BaseModel.RowStateEnum.Added)
                {
                    //la mantengo in added: caso non più possibile dato che sincronizziamo il cliente in tempo reale
                }
                else
                {
                    item.RowState = xUtilityPCL.BaseModel.RowStateEnum.Modified;
                }
                var frmCliente = new DettaglioCliente(item, this.myClientiItemsWrapper.ClientiItems,
                                                      this.myClientiItemsWrapper.ClientiItemsClone);
                //var np = new NavigationPage(frmCliente);
                //np.BarBackgroundColor = (Color)Application.Current.Resources["NavigationBarBackgroundColor"];
                //np.BarTextColor = (Color)Application.Current.Resources["NavigationBarTextColor"];

                //await navigation.PushModalAsync(np);

                await navigation.PushAsync(frmCliente, true);

                lClicked = true;
            }
            else
            {
                //var ddd = 1;
            }
        }
        async Task cercaCliente(string myText)
        {
            //https://xamarinhelp.com/xamarin-forms-search-bar/
            await Task.Delay(1);
            if (string.IsNullOrEmpty(myText))
            {
                //ripristino
                this.myClientiItemsWrapper.ClientiItems = new ObservableCollection<FEC_CustomersSystem>(this.myClientiItemsWrapper.ClientiItemsClone);
            }
            else
            {
                //filtro
                var l = this.myClientiItemsWrapper.ClientiItemsClone.Where
                                                    (x =>
                                                        (
                                                         (x as FEC_CustomersSystem).CompanyName != null &&
                                                         (x as FEC_CustomersSystem).CompanyName.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                        ||
                                                        (
                                                         (x as FEC_CustomersSystem).Address != null &&
                                                         (x as FEC_CustomersSystem).Address.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                        ||
                                                        (
                                                         (x as FEC_CustomersSystem).City != null &&
                                                         (x as FEC_CustomersSystem).City.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                        ||
                                                        (
                                                         (x as FEC_CustomersSystem).TaxIDNumber != null &&
                                                         (x as FEC_CustomersSystem).TaxIDNumber.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                        ).ToList();
                this.myClientiItemsWrapper.ClientiItems = new ObservableCollection<FEC_CustomersSystem>(l);
            }

            abc = myText;
        }

        public async Task<DettaglioCliente> aggiungiCliente(QR.RootObject qr = null)
        {
            await Task.Delay(1);
            if (lClicked)
            {
                lClicked = false;
                var drNuovo = new FEC_CustomersSystem();
                drNuovo.StatoFormRispettoSqlite_IMC = "I";
                drNuovo.RowState = xUtilityPCL.BaseModel.RowStateEnum.Added;
                drNuovo.InsertedDate = DateTime.Now;
                //drNuovo.IDCompany = App.K_Attivazione_IDCompany;
                drNuovo.Disabled = false;

                if (qr != null)
                {
                    drNuovo.StateCode = qr.anag.naz;
                    drNuovo.FiscalCode = qr.anag.cf;
                    drNuovo.TaxIDNumber = qr.anag.piva;
                    drNuovo.CompanyName = qr.anag.denom;
                    drNuovo.County = qr.anag.domFisc.prov;
                    drNuovo.ZipCode = qr.anag.domFisc.cap;
                    drNuovo.City = qr.anag.domFisc.com;
                    drNuovo.Address = qr.anag.domFisc.ind;
                    //drNuovo.StateCode = qr.anag.domFisc.naz;

                    //20181211 inizio
                    if (string.IsNullOrEmpty(qr.SDI.pec) == false)
                        drNuovo.Pec = qr.SDI.pec;
                    if (string.IsNullOrEmpty(qr.SDI.cod) == false)
                        drNuovo.SID = qr.SDI.cod;
                    //20181211 fine
                }

                //drNuovo.CompanyName = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var frmCliente = new DettaglioCliente(drNuovo, this.myClientiItemsWrapper.ClientiItems,
                                                    this.myClientiItemsWrapper.ClientiItemsClone
                                                     );
                //var np = new NavigationPage(frmCliente);
                //np.BarBackgroundColor = (Color)Application.Current.Resources["NavigationBarBackgroundColor"];
                //np.BarTextColor = (Color)Application.Current.Resources["NavigationBarTextColor"];
                //await navigation.PushModalAsync(np);
                await navigation.PushAsync(frmCliente, true);
                lClicked = true;
                return frmCliente;
            }
            else
            {
                //var a = 1;
                return null;
            }
        }

        async Task QRCliente()
        {
            if (lClickedQR)
            {
                lClickedQR = false;
                //inizio lettura QR standard AGE
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                var result = await scanner.Scan();

                if (result != null)
                {
                    try
                    {
                        var r = Newtonsoft.Json.JsonConvert.DeserializeObject<QR.RootObject>(result.Text);
                        await aggiungiCliente(r);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        string labelMessaggio = AppResources.AlertLabelMessaggio;
                        var QRNonStand = AppResources.FormClientiMessaggioQRNonStandard;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, QRNonStand + result.Text, "OK");
                    }
                    //await Application.Current.MainPage.DisplayAlert("DOne", "Scanned Barcode: " + result.Text, "OK");
                }

                lClickedQR = true;
                return;
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class ClientiItemsWrapper
    {
        public ClientiItemsWrapper()
        {
            this.ClientiItems = new ObservableCollection<FEC_CustomersSystem>();
            this.ClientiItemsClone = new ObservableCollection<FEC_CustomersSystem>();
        }
        public ObservableCollection<FEC_CustomersSystem> ClientiItems { get; set; }
        public ObservableCollection<FEC_CustomersSystem> ClientiItemsClone { get; set; }
    }
}
