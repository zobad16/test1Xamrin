using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test1Xamrin.Resx;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xUtilityPCL;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestSincronizzazione : ContentPage
    {
        public FEC_InvoiceRequests drTestata { get; set; }
        public FEC_Activation drBenzinaio { get; set; }
        //public FEC_CustomersSystem drCustomer { get; set; }
        public Boolean modalitaSingola = false;

        public TestSincronizzazione(FEC_InvoiceRequests _drTestata)
        {
            InitializeComponent();

            var titolo = AppResources.TitoloFormSincroRiepilogo;
            this.Title = titolo;
            this.drTestata = _drTestata;
            modalitaSingola = true;
            slRiepilogoSingolo.IsVisible = true;

            this.btnSincro.Text = AppResources.FormSincroBottoneSincroLabelInvia; //20180901

            this.lblTitoloRiepilogoDoc.Text = AppResources.FormSincroRiepilogoLabelRiepilgoRichiesta;

            using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
            {
                //drCustomer = cn.Table<FEC_CustomersSystem>().FirstOrDefault(y => y.TaxIDNumber == drTestata.TaxIDNumber);
                drBenzinaio = cn.Table<FEC_Activation>().FirstOrDefault(y => y.TaxIDNumber == drTestata.IDCompanyTaxIDNumber);

                //cn.Close();

                var _myBindingContext = new myBindingContext();
                //_myBindingContext.drCustomer = drCustomer;
                _myBindingContext.drTestata = drTestata;

                //if (string.IsNullOrEmpty(drCustomer.NumeroOrdine) == false)
                //    this.slNumDataOrdine.IsVisible = true;
                //if (string.IsNullOrEmpty(drCustomer.NumeroContratto) == false)
                //    this.slNumDataContratto.IsVisible = true;
                //if (string.IsNullOrEmpty(drCustomer.NumeroConvenzione) == false)
                //    this.slNumDataConvenzione.IsVisible = true;
                //if (string.IsNullOrEmpty(drCustomer.NumeroRicezione) == false)
                //this.slNumDataRicezione.IsVisible = true;

                this.BindingContext = _myBindingContext;

                //this.drTestata.RecapitoUnbound = FECUtilita.getRecapito(drBenzinaio); //20180901
                this.drTestata.RecapitoUnbound = drTestata.IDCompanyPEC;

                //20180901 inizio valorizzazione lblProdotti
                this.lblProdotti.Text = "";

                if (string.IsNullOrEmpty(this.drTestata.DescrPrdottoUnbound))
                {
                    var drItem = cn.Table<FEC_Items>().FirstOrDefault(y => y.ItemCode == this.drTestata.ItemCode);

                    if (drItem != null)
                        this.drTestata.DescrPrdottoUnbound = drItem.ItemDescription;
                }

                this.lblProdotti.Text += this.drTestata.DescrPrdottoUnbound + "\t" + this.drTestata.TotalAmount.Value.ToString("C2");
                this.lblProdottiIntestazione.Text = AppResources.FormSincroRiepilogoLabelProdotto;

                cn.Close();
            }
        }

        async void btnSincro_Clicked(object sender, System.EventArgs e)
        {
            /*
            string labelMessaggio = AppResources.AlertLabelMessaggio;
            Boolean lInternetAvailable = await Utility.isInternetAvailable2();

            if (lInternetAvailable == false)
            {
                string msgConnessioneInternet = AppResources.SincroConnessioneInternet;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgConnessioneInternet, "OK");
                return;
            }

            var myHudClass = new SettaBusyHud(this);
            string labelHud = AppResources.FormSincronizzaioneMessaggioHud;
            myHudClass.SettaVisible((Color)Application.Current.Resources["NavigationBarBackgroundColor"], labelHud);
            //await Task.Delay(10000);
            //myHudClass.settaInvisible();
            //return;

            try
            {
                var lTableExist = sqliteHelper.sqlite_TableExistSYNC<FEC_Customers>(App.sqliteDbName);
                if (lTableExist == true)
                {
                    var g = new MailParams();
                    g.Recipients.Add("alfacchini@alice.it");//se non si aggiungono domini è l'unica e-mail possibile
                    var oggettoMail = "SINCRO xFatturazioneAttiva" + " " + DateTime.Now + " - " + App.K_FEC_Users.Username + " " + App.K_currentPlatform.getApplicationVersion();

                    g.Subject = oggettoMail;
                    g.Body = "";
                    g.IsResponseRequested = true;
                    g.IsReadReceiptRequested = false;
                    g.mailgun_Key = "key-254d37b7b2c671845a765e7d0bf8ab50";//"key-c813baf3a313eb22a6d63d1dec8c6ba8";
                    g.mailgun_Domain = "sandbox4f20cf856f5c437ca07529032964dfdc.mailgun.org";//"sandboxee693156474e40f889e7494451b846f3.mailgun.org";

                    g.Attachements.Add(new Attachement { NomeFileSenzaPath = App.sqliteDbName, NomeAllegato = App.sqliteDbName });
                    var msg = await App.K_currentPlatform.sendEmailNEW(g, "", "", Email_Type.MailGun);
                }
            }
            catch (Exception ex)
            {
                //trap
            }

            if (modalitaSingola)
            {
                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    drTestata.SelectedToSyncronize = true;
                    cn.Update(drTestata);
                    cn.Close();
                }
            }

            var t = await WSChiamateEF.getTabelleAnagrafichexFatturazione(modalitaSingola);
            if (modalitaSingola)
            {
                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    drTestata.SelectedToSyncronize = false;
                    cn.Update(drTestata);
                    cn.Close();

                }
            }

            if (t.Item5 != "")
            {
                string msgClienti = AppResources.SincroClientiOK;
                string msgProdotti = AppResources.SincroProdottiOK;
                string msgFatture = AppResources.SincroFattureOK;
                string msgPassaggi = AppResources.SincroMessPassaggiCorretti;

                myHudClass.settaInvisible();
                //this.IsBusyHud = false;
                var msgAggiuntivo = "";
                if (t.Item2 > 0)
                    msgAggiuntivo += msgClienti + Environment.NewLine;
                if (t.Item3 > 0)
                    msgAggiuntivo += msgProdotti + Environment.NewLine;
                if (t.Item4 > 0)
                    msgAggiuntivo += msgFatture + Environment.NewLine;

                if (msgAggiuntivo != "")
                    msgAggiuntivo = msgPassaggi + Environment.NewLine + msgAggiuntivo;

                var descr = AppResources.SincroMessErrore;

                await Application.Current.MainPage.DisplayAlert(labelMessaggio, descr + ":" + Environment.NewLine + t.Item5 + Environment.NewLine + msgAggiuntivo, "OK");
                try
                {
                    await WSChiamateEF.writeSqlLiteLOG("SINCRO", descr + Environment.NewLine + t.Item5 + Environment.NewLine + msgAggiuntivo);
                }
                catch
                {

                }
                //lMenu6 = true;

                return;
            }

            var r = t.Item1;
            var nRecDaSincronizzareCustomers = t.Item2;
            var nRecDaSincronizzareItems = t.Item3;
            var nRecDaSincronizzareSaleDocs = t.Item4;

            myHudClass.settaInvisible();

            var ss = "";

            string nFattureTrasmesse = AppResources.SincroNumeroFattureTrasmesse;
            string nClientiAggiornati = AppResources.SincroNumeroClientiAggiornati;
            string nProdottiAggiornati = AppResources.SincroNumeroProdottiAggiornati;

            ss += nFattureTrasmesse + nRecDaSincronizzareSaleDocs + Environment.NewLine;
            ss += nClientiAggiornati + nRecDaSincronizzareCustomers + Environment.NewLine;
            ss += nProdottiAggiornati + nRecDaSincronizzareItems + Environment.NewLine;

            string msgOperazioneEffettuata = "";
            if (modalitaSingola == false)
            {
                msgOperazioneEffettuata = AppResources.SincroMessOperazioneEffettuata;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgOperazioneEffettuata + Environment.NewLine +
                                                                ss, "OK");
                                            //+"numero di tabelle lette: " + strNumeroTabelleLette, "OK");
            }
            else
            {
                msgOperazioneEffettuata = AppResources.SincroMessInvioAvvenutoCorrettamente;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgOperazioneEffettuata, "OK");
            }

            //la sincro è avvenuta correttamente: ricerco la fattura appena immessa
            if (modalitaSingola)
            {
                this.btnAnteprima.IsVisible = true;

                this.btnSincro.IsVisible = false;
                //this.btnSincro.IsEnabled = false; //20180901

                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    var itemtype = drTestata.SaleDocType;
                    this.drTestataNEW = cn.Table<FEC_SaleDoc>()
                                         .Where(y => y.SaleDocType == itemtype && y.SaleDocNr != null)
                                         .OrderByDescending(y => y.SaleDocNr).FirstOrDefault();
                    if (vmSaleDoc != null)
                        vmSaleDoc.drTestataNEW = this.drTestataNEW;
                    else
                        myWrapperTestataNEW.drTestataNEW = this.drTestataNEW;

                    cn.Close();
                }
            }
            else
            {
                var nDocSincronizzabili = AppResources.FormSincroNumDocSincronizzabili;
                lblNumeroDocumenti.Text = nDocSincronizzabili;
                lblNumeroDocumenti.Text += "0";
            }

            try
            {
                await WSChiamateEF.writeSqlLiteLOG("SINCRO", ss);
            }
            catch
            {
                var abc = 1;
            }
            */
        }

         void HandleAnteprimaFattura(object sender, System.EventArgs e)
        {
            /*
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            var itemtype = drTestata.SaleDocType;
            FEC_SaleDocDocuments drDocumento;
            FEC_Customers drCustomer;
            //if (drTestata.SyncroDate == null)
            //{
            //  await Application.Current.MainPage.DisplayAlert("Messaggio", "Il documento sarà disponibile solo dopo avere effettuato una sincronizzazione", "OK");
            //  return;
            //}

            using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
            {

                //ricerco la fattura appena immessa
                //drTestataNEW = cn.Table<FEC_SaleDoc>()
                //                   .Where(y => y.SaleDocType == itemtype && y.SaleDocNr != null)
                //                   .OrderByDescending(y => y.SaleDocNr).FirstOrDefault();

                //cerco la riga documento 
                drDocumento = cn.Table<FEC_SaleDocDocuments>().FirstOrDefault(y => y.IDSaleDoc == drTestataNEW.IDSaleDoc && y.DocumentType == itemtype);
                drCustomer = cn.Table<FEC_Customers>().FirstOrDefault(y => y.TaxIDNumber == drTestata.TaxIDNumber);
                cn.Close();
            }
            if (String.IsNullOrEmpty(drDocumento.DocumentContent) == false)
            {
                byte[] bytes = System.Convert.FromBase64String(drDocumento.DocumentContent);
                await Navigation.PushAsync(new Pdfviewpage(bytes, drCustomer, drTestata));
                //nota: apro NON in modal altrimenti non si apre l'invio e-mail in iOS
            }

            else
            {
                //chiamata rest service (inizio)
                var ret = await WSChiamateEF.getPDF(App.K_Attivazione_IDCompany, drDocumento.IDSaleDocDocument);
                if (ret == "KO")
                {
                    var msgPrb = AppResources.FormDocMessaggioPrbConnRichiestaDoc;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgPrb, "OK");
                    return;
                }
                if (ret == "")
                {
                    var msgDisp = AppResources.FormDocMessaggioDocNonDisp;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgDisp, "OK");
                    return;
                }
                byte[] bytes = System.Convert.FromBase64String(ret);
                await Navigation.PushAsync(new Pdfviewpage(bytes, drCustomer, drTestata));
                //nota: apro NON in modal altrimenti non si apre l'invio e-mail in iOS
                //chiamata rest service (fine)



            }

            //else
            //{
            //  await Application.Current.MainPage.DisplayAlert("Messaggio", "Documento non disponibile", "OK");
            //}
            */
        }

        async void btnEsci_Clicked(object sender, System.EventArgs e)
        {
            await this.Navigation.PopAsync();
        }
    }

    public class myBindingContext
    {
        public FEC_CustomersSystem drCustomer { get; set; }
        public FEC_InvoiceRequests drTestata { get; set; }
    }
}