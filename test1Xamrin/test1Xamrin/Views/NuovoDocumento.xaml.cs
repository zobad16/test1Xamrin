using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    public partial class NuovoDocumento : ContentPage
    {
        private Boolean lClicked { get; set; } = true;
        private Boolean lClickedSalva { get; set; } = true;
        private Boolean lClickedQR { get; set; } = true;
        public FEC_InvoiceRequests drTestata { get; set; }
        public FEC_InvoiceRequests drTestataClone { get; set; }
        public FEC_InvoiceRequests drUltimaRichiestaFattura { get; set; }
        public ObservableCollection<FEC_CustomersSystem> Clienti { get; set; }
        public ObservableCollection<string> ClientiDDL { get; set; }
        public ObservableCollection<FEC_PaymentTerms> ModalitaPagamento { get; set; }
        public ObservableCollection<string> ModalitaPagamentoDDL { get; set; }
        public ICommand cmdbtnSalva { set; get; }
        public ICommand cmdbtnAnnulla { set; get; }
        public ICommand cmdbtnChiudi { set; get; }
        public Boolean slSalvaAnnullaVisible { set; get; } = true;
        public Boolean slChiudiVisible { set; get; } = false;
        public Boolean slPrincipaleEnabled { set; get; } = true;
        public Boolean lChiamataDaDisappearing { get; set; } = false; //20180630
        public string ultimobtnPremutoSalva_Annulla_Chiudi { get; set; } = ""; //20180630

        public NuovoDocumento(FEC_InvoiceRequests _drTestata, string _statoFormRispettoSqlite_IMC)
        {
            InitializeComponent();

            //GUARDARE
            using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
            {
                var l = cn.Table<FEC_CustomersSystem>().ToList();
                this.Clienti = new ObservableCollection<FEC_CustomersSystem>(l);
                this.ClientiDDL = new ObservableCollection<string>();

                foreach (var drCliente in l)
                {
                    this.ClientiDDL.Add(drCliente.CompanyName);
                }

                var ll = cn.Table<FEC_InvoiceRequests>().OrderByDescending(y => y.IDInvoiceRequest).ToList();
                if (ll.Count > 0)
                {
                    drUltimaRichiestaFattura = ll.FirstOrDefault();
                    imgRecuperaUltimaTarga.IsVisible = true;
                }
                else
                    imgRecuperaUltimaTarga.IsVisible = false;

                //var l2 = cn.Table<FEC_PaymentTerms>().ToList();
                //this.ModalitaPagamento = new ObservableCollection<FEC_PaymentTerms>(l2);
                //this.ModalitaPagamentoDDL = new ObservableCollection<string>();

                //foreach (var drPagamento in l2)
                //{
                //    this.ModalitaPagamentoDDL.Add(drPagamento.PaymentDescription);
                //}

                //var l3 = cn.Table<FEC_Items>().ToList();
                //this.Items = new ObservableCollection<FEC_Items>(l3);

                cn.Close();
            }

            this.ModalitaPagamento = new ObservableCollection<FEC_PaymentTerms>();
            this.ModalitaPagamentoDDL = new ObservableCollection<string>();
            //this.ModalitaPagamentoDDL.Add("Carta credito");
            //this.ModalitaPagamentoDDL.Add("Bancomat");

            this.cmdbtnSalva = new Command(async () => await SalvaFattura());
            this.cmdbtnAnnulla = new Command(async () => await AnnullaFattura());
            this.cmdbtnChiudi = new Command(async () => await ChiudiFattura());

            if (_statoFormRispettoSqlite_IMC == "I")
            {
                this.drTestata = new FEC_InvoiceRequests();
                this.drTestata.RowState = BaseModel.RowStateEnum.Added;
                this.drTestata.Receiptdate = DateTime.Today;
                //this.drTestata.FuelDate = DateTime.Today; //non usato perchè coincide con Receiptdate
                this.drTestata.InsertedDate = DateTime.Now;
                this.drTestata.IDCompanyTaxIDNumberCompanyNameUnbound = AppResources.FormNuovoDocTestoSelezionaBenzinaio;
                this.drTestata.IDCompanyCompanyName = AppResources.FormNuovoDocTestoSelezionaBenzinaio;
                this.drTestata.DeviceID = App.K_DeviceID;

                this.slSalvaAnnullaVisible = true;
                this.slChiudiVisible = false;
                this.slPrincipaleEnabled = true;
            }

            if (_statoFormRispettoSqlite_IMC == "M")
            {
                var titoloModDoc = AppResources.TitoloFormModDoc;
                this.Title = titoloModDoc;

                this.drTestata = _drTestata;
                this.drTestata.IDCompanyTaxIDNumberCompanyNameUnbound = this.drTestata.IDCompanyCompanyName + "-" + this.drTestata.IDCompanyTaxIDNumber;

                if (drTestata.SaleDocDate != null)
                {
                    //il benzinaio ha già fatto fattura
                    this.slSalvaAnnullaVisible = false;
                    this.slChiudiVisible = true;
                    this.slPrincipaleEnabled = false;
                }
                else
                {
                    //il benzinaio NON ha ancora fatto fattura
                    this.slSalvaAnnullaVisible = true;
                    this.slChiudiVisible = false;
                    this.slPrincipaleEnabled = true;
                }

                FEC_Activation drActivation;
                using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                {
                    drActivation = cn.Table<FEC_Activation>().FirstOrDefault(Y => Y.IDCompany == drTestata.IDCompany);

                    //cn.CreateTable<FEC_PaymentTerms>();
                    cn.Close();
                }

                Benzinai.popolaTabelleRestDopoSceltaBenzinaio(drActivation, this.ModalitaPagamento, this.ModalitaPagamentoDDL, this.ddlModalitaPagamento, false);
            }

            pickerReceiptDate.SelectedItem = this.pickerReceiptDate.date2SFPickerFormat(this.drTestata.Receiptdate.Value);

            if (Clienti.Count == 0)
            {
                //20181122 commentato e gestito nel menù (TestPage)
                //string labelMessaggio = AppResources.AlertLabelMessaggio;
                //var msgNonModPIVAEsistFatt = AppResources.FormNuovoDocMsgInserireAlmenoUnCliente;
                //Application.Current.MainPage.DisplayAlert(labelMessaggio, msgNonModPIVAEsistFatt, "OK");
                //return;
            }
            else
            {
                if (Clienti.Count > 1)
                {
                    //nell'app sono registrate più partite IVA
                    slClienteConTap.IsVisible = true;
                    slClienteSenzaTap.IsVisible = false;
                    this.txtCliente2.RemoveBinding(CustomEntry.TextProperty);
                }

                if (Clienti.Count == 1)
                {
                    //nell'app è registrata una sola partita IVA
                    slClienteConTap.IsVisible = false;
                    slClienteSenzaTap.IsVisible = true;
                    this.txtCliente.RemoveBinding(CustomEntry.TextProperty);
                    this.ddlCliente.RemoveBinding(Syncfusion.SfPicker.XForms.SfPicker.ItemsSourceProperty);

                    var drCliente = Clienti[0];
                    drTestata.TaxIDNumber = drCliente.TaxIDNumber;
                    settaCliente(drCliente);
                }
            }

            this.drTestata.StatoFormRispettoSqlite_IMC = _statoFormRispettoSqlite_IMC;
            this.drTestataClone = this.drTestata.Clone() as FEC_InvoiceRequests;

            var ci = System.Globalization.CultureInfo.CurrentCulture;
            numUDTotalAmount.Culture = ci;

            var currency = ci.NumberFormat.CurrencySymbol;
            lblCurrencyTotalAmount.Text = currency;
            txtKM.Keyboard = Keyboard.Numeric;

            this.BindingContext = this;

            if (drTestata.ReceiptPicture1 == null)
            {
                this.imgVediFotoScontrino.Source = "ViewPhotoNotPresentIcon.png";
            }
            else
            {
                this.imgVediFotoScontrino.Source = "ViewPhotoIcon.png";
            }
        }

        async void AltriDati_Tapped(object sender, System.EventArgs e)
        {
            if (lClicked)
            {
                lClicked = false;
                await Navigation.PushAsync(new NuovoDocumentoAltriDati(this.drTestata));
                lClicked = true;
            }
        }

        async void SceltaBenzinaio_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (this.drTestata.TaxIDNumber == null)
            {
                string msg = AppResources.FormNuovoDocMsgIntrodurreRichiedentePrimaSelBenzinaio;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msg, "OK");
                return;
            }

            if (lClicked)
            {
                lClicked = false;

                //drTestata.PaymentTermsID = null; //20181128

                await Navigation.PushAsync(new Benzinai(this.drTestata, true, this.ModalitaPagamento, this.ModalitaPagamentoDDL, this.ddlModalitaPagamento));

                lClicked = true;
            }
        }

        void RecuperaUltimaTarga_Tapped(object sender, System.EventArgs e)
        {
            drTestata.Plate = drUltimaRichiestaFattura.Plate;
        }

        void txtReceiptDate_Tapped(object sender, System.EventArgs e)
        {
            pickerReceiptDate.IsOpen = true;
        }

        void pickerReceiptDate_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            var d = this.pickerReceiptDate.SFPickerFormat2Date(this.pickerReceiptDate.SelectedItem as ObservableCollection<object>);
            this.drTestata.Receiptdate = d;
            //txtReceiptDate.Text = d.Value.ToString(Utility.getDateFormat()); //20180630"dd/MM/yyyy");
        }

        async void FotoScontrino_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (string.IsNullOrEmpty(this.drTestata.ReceiptPicture1) == false)
            {
                var dresult = await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.FormNuovoDocDomandaSovrasciviImmagine, "Si", "No");

                if (dresult == false)
                    return;
            }

            if (Device.RuntimePlatform == Device.iOS) //in android la camera non causa il disappearing della form chiamante
                this.ultimobtnPremutoSalva_Annulla_Chiudi = "FOTO";

            await TakePicture(this.drTestata, 1);

            this.imgVediFotoScontrino.Source = "ViewPhotoIcon.png";
        }

        async void VediFotoScontrino_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (string.IsNullOrEmpty(this.drTestata.ReceiptPicture1))
            {
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.FormNuovoDocMessaggioNessunaImmPresDaVisualizzare, "OK");
                return;
            }

            byte[] arr = Convert.FromBase64String(this.drTestata.ReceiptPicture1);

            await Navigation.PushAsync(new ImageZoomer(arr) { BindingContext = new zoom.TransformImageViewModel() });
        }

        async private Task SalvaFattura()
        {
            if (lClickedSalva)
            {
                lClickedSalva = false;

                string msgValidazioni = "";
                string labelMessaggio = AppResources.AlertLabelMessaggio;

                if (string.IsNullOrEmpty(drTestata.TaxIDNumber))
                {
                    msgValidazioni = AppResources.FormNuovoDocValidazioniCliente;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                    lClickedSalva = true;
                    return;
                }

                if (drTestata.IDCompany == null)
                {
                    msgValidazioni = AppResources.FormNuovoDocValidazioniBenzinaio;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                    lClickedSalva = true;
                    return;
                }

                if (string.IsNullOrEmpty(drTestata.Plate))
                {
                    msgValidazioni = AppResources.FormNuovoDocValidazioniTarga;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                    lClickedSalva = true;
                    return;
                }

                if (drTestata.Plate.Length < 3)
                {
                    msgValidazioni = AppResources.FormNuovoDocValidazioniTargaLunghezza;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                    lClickedSalva = true;
                    return;
                }

                FEC_Activation drActivation;
                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    drActivation = cn.Table<FEC_Activation>().FirstOrDefault(Y => Y.IDCompany == drTestata.IDCompany);

                    cn.Close();
                }

                if (drActivation.IsReceiptNumberMandatory.Value == true)
                {
                    if (string.IsNullOrEmpty(drTestata.ReceiptNumber))
                    {
                        msgValidazioni = AppResources.FormNuovoDocValidazioniNumScontrino;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                        lClickedSalva = true;
                        return;
                    }
                }

                if (drActivation.IsReceiptPicture1Mandatory.Value == true)
                {
                    if (App.baseURL.Contains("192.168") == false)
                    {
                        if (string.IsNullOrEmpty(drTestata.ReceiptPicture1))
                        {
                            msgValidazioni = AppResources.FormNuovoDocValidazioniFotoScontrino;
                            await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                            lClickedSalva = true;
                            return;
                        }
                    }
                }

                if (drActivation.IsReceiptPicture2Mandatory.Value == true)
                {
                    if (App.baseURL.Contains("192.168") == false)
                    {
                        if (string.IsNullOrEmpty(drTestata.ReceiptPicture2))
                        {
                            msgValidazioni = AppResources.FormNuovoDocValidazioniFotoScontrino;
                            await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                            lClickedSalva = true;
                            return;
                        }
                    }
                }

                if (drActivation.IsReceiptPicture3Mandatory.Value == true)
                {
                    if (App.baseURL.Contains("192.168") == false)
                    {
                        if (string.IsNullOrEmpty(drTestata.ReceiptPicture3))
                        {
                            msgValidazioni = AppResources.FormNuovoDocValidazioniFotoScontrino;
                            await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                            lClickedSalva = true;
                            return;
                        }
                    }
                }

                if (drActivation.IsReceiptdateMandatory.Value == true)
                {
                    if (drTestata.Receiptdate.HasValue == false)
                    {
                        msgValidazioni = AppResources.FormNuovoDocValidazioniDataScontrino;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                        lClickedSalva = true;
                        return;
                    }
                }

                if (drActivation.IsReceiptTimeMandatory.Value == true)
                {
                    if (string.IsNullOrEmpty(drTestata.ReceiptTime))
                    {
                        msgValidazioni = AppResources.FormNuovoDocValidazioniOraScontrino;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                        lClickedSalva = true;
                        return;
                    }
                }

                if (drActivation.IsReceiptNoteMandatory.Value == true)
                {
                    if (string.IsNullOrEmpty(drTestata.ReceiptNote))
                    {
                        msgValidazioni = AppResources.FormNuovoDocValidazioniNoteScontrino;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                        lClickedSalva = true;
                        return;
                    }
                }

                if (drActivation.IsReceiptPointOfSaleMandatory.Value == true)
                {
                    if (string.IsNullOrEmpty(drTestata.ReceiptPointOfSale))
                    {
                        msgValidazioni = AppResources.FormNuovoDocValidazioniPuntoVenditaScontrino;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                        lClickedSalva = true;
                        return;
                    }
                }

                if (drActivation.IsReceiptTerminalNumberMandatory.Value == true)
                {
                    if (string.IsNullOrEmpty(drTestata.ReceiptTerminalNumber))
                    {
                        msgValidazioni = AppResources.FormNuovoDocValidazioniTerminaleScontrino;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                        lClickedSalva = true;
                        return;
                    }
                }

                if (drActivation.IsReceiptPumpNumberMandatory.Value == true)
                {
                    if (string.IsNullOrEmpty(drTestata.ReceiptPumpNumber))
                    {
                        msgValidazioni = AppResources.FormNuovoDocValidazioniPompaScontrino;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                        lClickedSalva = true;
                        return;
                    }
                }

                if (drActivation.IsReceiptQRMandatory.Value == true)
                {
                    if (string.IsNullOrEmpty(drTestata.ReceiptQR))
                    {
                        msgValidazioni = AppResources.FormNuovoDocValidazioniQRScontrino;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                        lClickedSalva = true;
                        return;
                    }
                }

                if (drTestata.PaymentTermsID.HasValue == false)
                {
                    msgValidazioni = AppResources.FormNuovoDocValidazioniTipoPag;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                    lClickedSalva = true;
                    return;
                }

                if (drTestata.TotalAmount.HasValue == false)
                {
                    msgValidazioni = AppResources.FormNuovoDocValidazioniTotale;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                    lClickedSalva = true;
                    return;
                }

                if (drTestata.TotalAmount.Value == 0)
                {
                    msgValidazioni = AppResources.FormNuovoDocValidazioniTotaleMaggZero;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                    lClickedSalva = true;
                    return;
                }

                var codProdottoText = AppResources.FormNuovoDocUsareListaPerInsModCodiceProdotto;
                if (drTestata.ItemCode.ToLower() == codProdottoText.ToLower())
                {
                    msgValidazioni = AppResources.FormNuovoDocValidazioniProdotto;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                    lClickedSalva = true;
                    return;
                }

                //Acr.UserDialogs.UserDialogs.Instance.ShowLoading(); 
                var myHud = new ChatBotLoadIndicator(); //20181125
                myHud.SettaLabel(AppResources.NuovoHudColoratoMessaggioSalvataggio); //20181125
                //myHud.SettaTextColor(Color.FromHex("ffffff")); //20181125
                //myHud.SettaBackgroundColor(Color.FromHex("1b9ed6")); //20181125

                DependencyService.Get<ILodingPageService>().InitLoadingPage(myHud); //20181125
                DependencyService.Get<ILodingPageService>().ShowLoadingPage(); //20181125

                if (drTestata.StatoFormRispettoSqlite_IMC == "M" && drTestata.ReceiptNumber == drTestataClone.ReceiptNumber
                    && drTestata.IDCompany == drTestataClone.IDCompany && drTestata.Receiptdate.Value.Year == drTestataClone.Receiptdate.Value.Year)
                {
                    //se siamo in modifica di una richiesta e non sono stati modificati né il benzinaio né il numero scontrino
                    //non faccio il controllo di scontrino già presente per quel benzinaio perchè chiaramente c'è
                }
                else
                {
                    var lReceiptNumberGiaPresIDCompany = await WSChiamateEF.getControlloReceiptNumberGiaPresenteIDCompany(drTestata.ReceiptNumber, drTestata.IDCompany.Value, drTestata.Receiptdate.Value.Year);
                    if (lReceiptNumberGiaPresIDCompany == "SI")
                    {
                        var msgNumScontrinoGiaUsatoBenz = AppResources.FormNuovoDocMessaggioNumScontrinoGiaPresenteBenzinaio;
                        //Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                        DependencyService.Get<ILodingPageService>().HideLoadingPage(); //20181125
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgNumScontrinoGiaUsatoBenz, "OK");
                        lClickedSalva = true;
                        return;
                    }
                    if (lReceiptNumberGiaPresIDCompany == "KO")
                    {
                        var msgErrServer = AppResources.FormDettClienteMessaggioErroreControlloNumScontrSuServer;
                        //Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                        DependencyService.Get<ILodingPageService>().HideLoadingPage(); //20181125
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErrServer, "OK");
                        lClickedSalva = true;
                        return;
                    }
                }

                //salvataggio
                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    if (drTestata.StatoFormRispettoSqlite_IMC == "I")
                    {
                        cn.Insert(drTestata);

                        //20181121 inizio commento
                        //this.slChiudiVisible = true;
                        //this.slSalvaAnnullaVisible = false;
                        //this.slPrincipaleEnabled = false;

                        //await Navigation.PushAsync(new TestSincronizzazione(drTestata));
                        //20181121 fine commento
                    }

                    if (drTestata.StatoFormRispettoSqlite_IMC == "M") //chiamata dalla lista
                    {
                        cn.Update(drTestata);

                        settaCampiUnboundTestata(drTestata);

                        //20181121 inizio commento
                        //this.slChiudiVisible = true;
                        //this.slSalvaAnnullaVisible = false;
                        //this.slPrincipaleEnabled = false;

                        //await Navigation.PushAsync(new TestSincronizzazione(drTestata));
                        //20181121 fine commento
                    }
                }

                var tSincro = await WSChiamateEF.getTabelleAnagrafichexFatturazione();

                if (tSincro.Item5 != "")
                {
                    var descr = AppResources.SincroMessErrore;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, descr, "OK");

                    try
                    {
                        await WSChiamateEF.writeSqlLiteLOG("SINCRO", descr + Environment.NewLine + tSincro.Item5);
                    }
                    catch
                    {

                    }

                    lClickedSalva = true;
                    //Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                    DependencyService.Get<ILodingPageService>().HideLoadingPage(); //20181125
                    return;
                }
                else
                {
                    //Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                    DependencyService.Get<ILodingPageService>().HideLoadingPage(); //20181125
                    this.ultimobtnPremutoSalva_Annulla_Chiudi = "Salva";
                }

                if (this.lChiamataDaDisappearing == false) //20180630
                    await Navigation.PopAsync(true);

                lClickedSalva = true;
            }
            else
            {
               // var aa = 1;
            }
        }

        async private Task AnnullaFattura()
        {
            this.ultimobtnPremutoSalva_Annulla_Chiudi = "Annulla";

            if (drTestata.StatoFormRispettoSqlite_IMC == "I")
            {
                if (this.lChiamataDaDisappearing == false) //20180630
                    await Navigation.PopAsync(true);
            }

            if (drTestata.StatoFormRispettoSqlite_IMC == "M") //chiamata dalla lista
            {
                drTestata.RejectChanges(this.drTestataClone);

                if (this.lChiamataDaDisappearing == false) //20180630
                    await Navigation.PopAsync(true);
            }
        }

        async private Task ChiudiFattura()
        {
            this.ultimobtnPremutoSalva_Annulla_Chiudi = "Chiudi";

            if (drTestata.StatoFormRispettoSqlite_IMC == "I")
            {
                if (this.lChiamataDaDisappearing == false) //20180630
                    await Navigation.PopAsync(true);
            }

            if (drTestata.StatoFormRispettoSqlite_IMC == "M") //chiamata dalla lista
            {
                //if (drTestataNEW != null) //ho fatto la sincro
                //{
                //    drTestata.SaleDocNr = drTestataNEW.SaleDocNr;
                //    drTestata.SyncroDate = drTestataNEW.SyncroDate;
                //    drTestata.IDSaleDoc = drTestataNEW.IDSaleDoc;
                //    settaCampiUnboundTestata(drTestata);
                //}

                if (this.lChiamataDaDisappearing == false)//20180630
                    await Navigation.PopAsync(true);
            }
        }

        async void QRSceltaBenzinaio_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (this.drTestata.TaxIDNumber == null)
            {
                string msg = AppResources.FormNuovoDocMsgIntrodurreRichiedentePrimaSelBenzinaio;
                Application.Current.MainPage.DisplayAlert(labelMessaggio, msg, "OK");
                return;
            }
            //Device.RuntimePlatform == Device.iOS
            if (Device.RuntimePlatform == Device.iOS) //in android la camera non causa il disappearing della form chiamante
                this.ultimobtnPremutoSalva_Annulla_Chiudi = "QR";

            if (lClickedQR)
            {
                lClickedQR = false;
                //inizio lettura QR standard AGE
                //this.ultimobtnPremutoSalva_Annulla_Chiudi_QR = "QR"; //20180701
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var QRNonStand = AppResources.FormClientiMessaggioQRNonStandard;

                var result = await scanner.Scan();

                if (result != null)
                {
                    try
                    {
                        //var r = Newtonsoft.Json.JsonConvert.DeserializeObject<QR.RootObject>(result.Text);
                        var r = result.Text;
                        var arrQR = r.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //prendere elemento 0
                        //verificare che sia numerico e che sia presente nella activation (sqllite); altrimenti errore
                        //se supera i 2 controlli sopra avremo in una variabile item la drActivation
                        //quindi chiamiamo il codice della uscita dalla combo di scelta del benzinaio

                        var el0 = arrQR[0];

                        Int32 n;
                        var isNumeric = Int32.TryParse(el0 as string, out n);
                        if (isNumeric)
                        {
                            FEC_Activation drActivation;
                            using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                            {
                                drActivation = cn.Table<FEC_Activation>().FirstOrDefault(y => y.IDCompany == n);
                                cn.Close();
                            }

                            if (drActivation != null)
                            {
                                Benzinai.settaBenzinaio(this.drTestata, drActivation);
                                Benzinai.popolaTabelleRestDopoSceltaBenzinaio(drActivation, this.ModalitaPagamento, this.ModalitaPagamentoDDL, this.ddlModalitaPagamento);
                            }
                            else
                            {
                                //non presente nella activation
                                lClickedQR = true;
                                await Application.Current.MainPage.DisplayAlert(labelMessaggio, QRNonStand + result.Text, "OK");
                                return;
                            }
                        }
                        else
                        {
                            //non numerico
                            lClickedQR = true;
                            await Application.Current.MainPage.DisplayAlert(labelMessaggio, QRNonStand + result.Text, "OK");
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, QRNonStand + result.Text, "OK");
                    }
                    //await Application.Current.MainPage.DisplayAlert("DOne", "Scanned Barcode: " + result.Text, "OK");
                }

                lClickedQR = true;
            }
        }

        void txtReceiptNumber_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e) //20190110
        {
            System.Diagnostics.Debug.WriteLine("txtReceiptNumber_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 16) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtReceiptNumber.Text = e.OldTextValue;
                else
                    this.txtReceiptNumber.Text = "";
            }
            else
                this.txtReceiptNumber.Text = e.NewTextValue;
        }

        void PlateHandle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //20190107 inizio
            System.Diagnostics.Debug.WriteLine("PlateHandle_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 20) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtPlate.Text = e.OldTextValue.ToUpper();
                else
                    this.txtPlate.Text = "";
            }
            else
                this.txtPlate.Text = e.NewTextValue.ToUpper();
            //20190107 fine

            //if (e.NewTextValue != null && e.NewTextValue != "") //20190107
            //txtPlate.Text = e.NewTextValue.ToUpper();
        }

        void txtModalitaPagamento_Tapped(object sender, System.EventArgs e)
        {
            if (this.drTestata.IDCompany == null)
            {
                string labelMessaggio = AppResources.AlertLabelMessaggio;
                string msg = AppResources.FormNuovoDocMsgIntrodurreBenzinaioPrimaSelModPag;
                Application.Current.MainPage.DisplayAlert(labelMessaggio, msg, "OK");
                return;
            }

            if (drTestata.PaymentTermsID != null)
            {
                var drPagamento = this.ModalitaPagamento.First(x => x.IDPaymentTerms == drTestata.PaymentTermsID);
                this.ddlModalitaPagamento.SelectedItem = drPagamento.PaymentDescription;
            }
            else
            {
                this.ddlModalitaPagamento.SelectedIndex = 0; //lo setto sul primo elemento
                this.ddlModalitaPagamento.SelectedItem = this.ModalitaPagamentoDDL[0];
            }

            this.ddlModalitaPagamento.IsOpen = true;
        }

        void txtCliente_Tapped(object sender, System.EventArgs e)
        {
            if (drTestata.TaxIDNumber != null)
            {
                var drCliente = this.Clienti.First(x => x.TaxIDNumber == drTestata.TaxIDNumber);
                this.ddlCliente.SelectedItem = drCliente.CompanyName;
            }
            else
            {
                this.ddlCliente.SelectedIndex = 0; //lo setto sul primo elemento
                this.ddlCliente.SelectedItem = this.ClientiDDL[0];
            }

            this.ddlCliente.IsOpen = true;
        }

        void numUDTotalAmountEventHandler(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
            {
                if (drTestata.TaxCode != null)
                {
                    var drTaxCode = cn.Table<FEC_TaxCode>().FirstOrDefault(y => y.TaxCode == drTestata.TaxCode);
                    decimal aliquota = 0;
                    if (drTaxCode != null)
                    {
                        aliquota = drTaxCode.TaxPerc.Value;
                    }

                    cn.Close();

                    drTestata.TotalAmount = Convert.ToDecimal(e.Value);
                    SceltaProdotti.RicalcolaPrezzo(aliquota, drTestata);
                    //FEC_SaleDocViewModel.CalcolaTotalAmountTestata(this.myModel);
                }
            }
        }

        void txtProdotto_Tapped(object sender, System.EventArgs e)
        {
            if (this.drTestata.IDCompany == null)
            {
                string labelMessaggio = AppResources.AlertLabelMessaggio;
                string msg = AppResources.FormNuovoDocMsgIntrodurreBenzinaioPrimaSelProdotto;
                Application.Current.MainPage.DisplayAlert(labelMessaggio, msg, "OK");
                return;
            }

            if (this.drTestata.TotalAmount.HasValue == false || this.drTestata.TotalAmount == 0)
            {
                string labelMessaggio = AppResources.AlertLabelMessaggio;
                string msg = AppResources.FormNuovoDocMsgIntrodurreTotalePrimaSelProdotto;
                Application.Current.MainPage.DisplayAlert(labelMessaggio, msg, "OK");
                return;
            }

            Navigation.PushAsync(new SceltaProdotti(this.drTestata));
        }

        void ddlModalitaPagamentoHandle_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            if (this.ddlModalitaPagamento.SelectedItem != null) //.SelectedIndex != null)
            {
                var sel = this.ddlModalitaPagamento.SelectedItem.ToString();
                var drPagamento = this.ModalitaPagamento.First(x => x.PaymentDescription == sel);
                this.drTestata.PaymentTermsID = drPagamento.IDPaymentTerms;
                this.drTestata.PaymentName = drPagamento.PaymentDescription;
            }
        }

        void ddlClienteHandle_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            if (this.ddlCliente.SelectedItem != null) //.SelectedIndex != null)
            {
                var sel = this.ddlCliente.SelectedItem.ToString();
                var drCliente = this.Clienti.First(x => x.CompanyName == sel);
                this.drTestata.TaxIDNumber = drCliente.TaxIDNumber;
                settaCliente(drCliente);
            }
        }

        public static void settaCampiUnboundTestata(FEC_InvoiceRequests drTestata)
        {
            drTestata.DescrizioneTipoUnbound = AppResources.DescrRichiestaFatturaNum;

            drTestata.NumeroEDataUnbound = "";
            if (string.IsNullOrEmpty(drTestata.ReceiptNumber) == false)
                drTestata.NumeroEDataUnbound += drTestata.ReceiptNumber;
            drTestata.NumeroEDataUnbound += "del " + drTestata.Receiptdate.Value.ToString(Utility.getDateFormat());//20180630 "dd/MM/yyyy");

            drTestata.GenerataFtRicNumDelUnbound = "";
            if (drTestata.SaleDocType.HasValue)
            {
                drTestata.GenerataFtRicNumDelUnbound += AppResources.CellaDocumentoParolaGenerata;

                if (drTestata.SaleDocType.Value == 1)
                    drTestata.GenerataFtRicNumDelUnbound += " " + AppResources.CellaDocumentoParolaFattura;

                if (drTestata.SaleDocType.Value == 3)
                    drTestata.GenerataFtRicNumDelUnbound += " " + AppResources.CellaDocumentoParolaRicevuta;

                drTestata.GenerataFtRicNumDelUnbound += " n.";

                if (drTestata.SaleDocNr.HasValue)
                {
                    drTestata.GenerataFtRicNumDelUnbound += " " + drTestata.SaleDocNr.Value;

                    if (string.IsNullOrEmpty(drTestata.SeriesDoc) == false)
                        drTestata.GenerataFtRicNumDelUnbound += "/" + drTestata.SeriesDoc;
                }

                drTestata.GenerataFtRicNumDelUnbound += " del";

                if (drTestata.SaleDocDate.HasValue)
                    drTestata.GenerataFtRicNumDelUnbound += " " + drTestata.SaleDocDate.Value.ToString(Utility.getDateFormat());

                if (string.IsNullOrEmpty(drTestata.GenerataFtRicNumDelUnbound))
                    drTestata.IsCellaDocRigaGenerataFtRicNumDelVisibleUnbound = false;
                else
                    drTestata.IsCellaDocRigaGenerataFtRicNumDelVisibleUnbound = true;
            }

            drTestata.IDCompanyIndirizzoCittaUnbound = "";
            if (string.IsNullOrEmpty(drTestata.IDCompanyAddress) == false)
                drTestata.IDCompanyIndirizzoCittaUnbound += drTestata.IDCompanyAddress + " - ";
            if (string.IsNullOrEmpty(drTestata.IDCompanyCity) == false)
                drTestata.IDCompanyIndirizzoCittaUnbound += drTestata.IDCompanyCity + " ";
        }

        private void settaCliente(FEC_CustomersSystem sel)
        {
            drTestata.CompanyName = sel.CompanyName;

            if (string.IsNullOrEmpty(sel.Address) == false)
                drTestata.Address = sel.Address;

            if (string.IsNullOrEmpty(sel.City) == false)
                drTestata.City = sel.City;

            if (string.IsNullOrEmpty(sel.ZipCode) == false)
                drTestata.ZipCode = sel.ZipCode;

            if (string.IsNullOrEmpty(sel.County) == false)
                drTestata.County = sel.County;

            if (string.IsNullOrEmpty(sel.Country) == false)
                drTestata.Country = sel.Country;

            if (string.IsNullOrEmpty(sel.StateCode) == false)
                drTestata.StateCode = sel.StateCode;

            if (string.IsNullOrEmpty(sel.FiscalCode) == false)
                drTestata.FiscalCode = sel.FiscalCode;

            if (string.IsNullOrEmpty(sel.Telephone) == false)
                drTestata.Telephone = sel.Telephone;

            if (string.IsNullOrEmpty(sel.Email) == false)
                drTestata.Email = sel.Email;

            if (string.IsNullOrEmpty(sel.Pec) == false)
                drTestata.PEC = sel.Pec;

            //CalcolaTotalAmountTestata(this.drTestata, this.list_righeDettaglio);
            //settaCampiUnboundTestata(drTestata);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();



            //this.ddlModalitaPagamento.PickerMode = Syncfusion.SfPicker.XForms.PickerMode.Default;

            //this.ddlModalitaPagamento.RemoveBinding(Syncfusion.SfPicker.XForms.SfPicker.ItemsSourceProperty);

            //this.ddlModalitaPagamento.SelectedIndex = 0;
            //this.ModalitaPagamentoDDL.Add("pippo");
            //this.ModalitaPagamentoDDL.Add("pluto");

            //this.ddlModalitaPagamento.PickerMode = Syncfusion.SfPicker.XForms.PickerMode.Dialog;
        }

        protected async override void OnDisappearing() //20180630
        {
            base.OnDisappearing();

            var nCount = 0;
            if (drTestata.StatoFormRispettoSqlite_IMC == "I")
                nCount = 2;
            if (drTestata.StatoFormRispettoSqlite_IMC == "M")
                nCount = 3;

            //if (App.NuovoDocumentoSistemaNavigazione == App.SistemaNavigazioneEnum.Standard)
            //{
            //    if (Navigation.NavigationStack[0] is NuovoDocumento)
            //    {
            //        return; //aperta dal menu
            //    }
            //    else
            //    {
            //        nCount -= 1;
            //    }
            //}

            if (Navigation.NavigationStack.Count == nCount) //in uscita dal dettaglio alla lista clienti
            {
                //entra qui quando facciamo back o annulla dalla form NuovoDocumento, MA anche quando si apre la camera per foto o QR
                if (this.ultimobtnPremutoSalva_Annulla_Chiudi == "") //'Salva' non possibile perchè va in avanti
                {
                    this.lChiamataDaDisappearing = true;
                    if (slChiudiVisible)
                        await ChiudiFattura();
                    else
                        await AnnullaFattura();
                }
                else
                {
                    //nCount non è incrementato quando apre la camera per qr o foto
                    this.ultimobtnPremutoSalva_Annulla_Chiudi = "";
                }

            }
        }

        public static async Task<Boolean> TakePicture(FEC_InvoiceRequests drTestata, Int32 numeroCampo)
        {
            var lSilent = true;
            var path_and_nome = "";
            var w = 500;
            var path = App.K_currentPlatform.getLocalDatabasePath();
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            path_and_nome = System.IO.Path.Combine(path, "test.png");
            var cameraService = CrossMedia.Current;
            await cameraService.Initialize();

            if (!cameraService.IsCameraAvailable || !cameraService.IsTakePhotoSupported)
            {
                var camNonDisp = AppResources.FormMenuMessaggioErroreCameraNonDisp;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, camNonDisp, "OK");
                return false;
            }
            else
            {
                MediaFile result;
                try
                {
                    result = await cameraService.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = true, //mi serve il file nella gallery (poi verrà cancellato) per determinare se l'immagine è stata scattata orrizontalmente o verticalmente
                                            //CompressionQuality=92,//implementare25: lasciare commentato anche dopo il passaggio a 2.5
                                            //PhotoSize = PhotoSize.Medium,//implementare25: lasciare commentato anche dopo il passaggio a 2.5
                        RotateImage = false, //implementare25 //20180121
                    });
                }
                catch (Exception ex)
                { //20161119 gestito errore
                    Console.WriteLine(ex.ToString());
                    var camOcc = AppResources.FormMenuMessaggioErroreCameraOccupata;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, camOcc, "OK");
                    return false;
                }

                if (result == null)
                {
                    var opAnn = AppResources.FormMenuMessaggioOperazioneAnnullata;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, opAnn, "OK");
                    return false;
                }
                else
                {
                    byte[] bytes;
                    using (var ms = new MemoryStream())
                    {
                        using (Stream s = result.GetStream())
                        {
                            s.Seek(0, SeekOrigin.Begin);
                            s.CopyTo(ms);
                            bytes = ms.ToArray();

                            bytes = App.K_currentPlatform.resizeImage(bytes, w, -1, true, result.Path, false, false);

                            if (numeroCampo == 1)
                                drTestata.ReceiptPicture1 = Convert.ToBase64String(bytes);

                            if (numeroCampo == 2)
                                drTestata.ReceiptPicture2 = Convert.ToBase64String(bytes);

                            if (numeroCampo == 3)
                                drTestata.ReceiptPicture3 = Convert.ToBase64String(bytes);

                            //App.K_currentPlatform.savefile(path_and_nome, bytes);
                            //try
                            //{
                            //    App.K_currentPlatform.DeleteFile(result.Path); //cancello il file nella gallery
                            //}
                            //catch (Exception ex)
                            //{

                            //}
                        }

                        if (lSilent == false)
                        {
                            var fotoCorretta = AppResources.FormMenuMessaggioFotoEsegCorrettamente;
                            await Application.Current.MainPage.DisplayAlert(labelMessaggio,
                                fotoCorretta + bytes.Length.ToString(), "OK");
                        }

                        bytes = null;
                    }

                    return true;
                }
            }
        }
    }
}