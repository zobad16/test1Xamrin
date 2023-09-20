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
    public partial class MenuSelf : ContentPage
    {
        private MenuItem drCorrente { get; set; }
        private Boolean lClicked = true;
        public Boolean isLoaded { get; set; }
        public string ErroreVersione { get; set; }
        public Boolean quartavoce { get; set; } //20190120
        public MenuSelf()
        {
            //Application.Current.Resources["K_BOTVisibile"] = false;
            InitializeComponent();

            //App.baseURL = "http://192.168.1.10/xFatturazioneElettronicaWebApi/api/";
            //App.baseURL = "http://192.168.1.115/xFatturazioneElettronicaWebApi/api/";
            //App.baseURL = "http://54.38.180.44/xFatturazioneElettronicaWebApi/api/";
            //App.baseURL = "http://sviluppoback.sabicom.cloud/xFatturazioneElettronicaWebApi/api/";
            //App.baseURL = "http://srvvending.westeurope.cloudapp.azure.com:8095/xFatturazioneElettronciaWebApi/api/";

            xUtilityPCL.Global.BaseURL = App.baseURL;
            this.drCorrente = new MenuItem();
            this.drCorrente.cmdbtnRegistrazione = new Command(btnRegistrazioneOnClick);
            this.drCorrente.cmdbtnRichiestaFattura = new Command(btnRichiestaFatturaOnClick);
            this.drCorrente.cmdbtnElencoRichieste = new Command(btnElencoRichiesteOnClick);
            this.drCorrente.cmdbtnChatBot = new Command(btnChatBotOnClick);
            this.drCorrente.cmdbtnChiudi = new Command(btnChiudiOnClick);

            //this.BindingContext = new ChatBot.MenuItem();
            this.BindingContext = this.drCorrente;
        }
        async void btnRegistrazioneOnClick()
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (lClicked)
            {
                lClicked = false;

                if (string.IsNullOrEmpty(ErroreVersione) == false)
                {
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, ErroreVersione, "OK");
                    lClicked = true;
                    return;
                }

                var f = new Clienti();

                await Navigation.PushAsync(f, true);

                lClicked = true;
            }
            else
            {
                //var aa = 1;
            }
        }

        async void btnRichiestaFatturaOnClick()
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (lClicked)
            {
                lClicked = false;

                if (string.IsNullOrEmpty(ErroreVersione) == false)
                {
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, ErroreVersione, "OK");
                    lClicked = true;
                    return;
                }

                //20181122 inizio
                using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                {
                    var l = cn.Table<FEC_CustomersSystem>().ToList();
                    cn.Close();

                    if (l.Count == 0)
                    {
                        var msgNonModPIVAEsistFatt = AppResources.FormNuovoDocMsgInserireAlmenoUnCliente;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgNonModPIVAEsistFatt, "OK");
                        lClicked = true;
                        return;
                    }
                }
                //20181122 fine

                var f = new NuovoDocumento(null, "I");

                await Navigation.PushAsync(f, true);

                lClicked = true;
            }
            else
            {
                //var aa = 1;
            }
        }

        async void btnElencoRichiesteOnClick()
        {
            /*
            var myHud = new ChatBotLoadIndicator();
            myHud.SettaLabel("Test caricamento");
            //myHud.SettaTextColor(Color.FromHex("ffffff")); //20181125
            //myHud.SettaBackgroundColor(Color.FromHex("1b9ed6")); //20181125

            DependencyService.Get<ILodingPageService>().InitLoadingPage(myHud);
            DependencyService.Get<ILodingPageService>().ShowLoadingPage();

            // just to showcase a delay...
            await Task.Delay(5000);

            // close the loading page...
            DependencyService.Get<ILodingPageService>().HideLoadingPage();
            return;
            */

            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (lClicked)
            {
                lClicked = false;

                if (string.IsNullOrEmpty(ErroreVersione) == false)
                {
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, ErroreVersione, "OK");
                    lClicked = true;
                    return;
                }

                var f = new Documenti();

                await Navigation.PushAsync(f, true);

                lClicked = true;
            }
            else
            {
                //var aa = 1;
            }
        }

        async void btnChatBotOnClick() //20190122
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (lClicked)
            {
                lClicked = false;

                if (string.IsNullOrEmpty(ErroreVersione) == false)
                {
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, ErroreVersione, "OK");
                    lClicked = true;
                    return;
                }

                //20181122 inizio
                using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                {
                    var l = cn.Table<FEC_CustomersSystem>().ToList();
                    cn.Close();

                    if (l.Count == 0)
                    {
                        var msgNonModPIVAEsistFatt = AppResources.FormNuovoDocMsgInserireAlmenoUnCliente;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgNonModPIVAEsistFatt, "OK");
                        lClicked = true;
                        return;
                    }
                }
                //20181122 fine

                var f = new SabicomChatMain();

                await Navigation.PushAsync(f, true);

                lClicked = true;
            }
            else
            {
                //var aa = 1;
            }
        }

        void btnChiudiOnClick()
        {

        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            if (isLoaded == false)
            {
                //chiamare la restapi getCheckVersioneSYSTEM dando un messaggio di errore: questo messaggio di errore 
                //oltre ad essere visualizzato con alert viene messo nella variabiule this.ErroreVersione...al click dei vari
                //bottoni se ErroreVersione != null, mandiamo una alert con l'errore




                while (true) //20181129
                {
                    if (string.IsNullOrEmpty(App.baseURL) == false)
                        break;
                }



                var str_versione_build = App.K_currentPlatform.getApplicationVersion();
                var int_versione_build = Convert.ToInt32(str_versione_build.Trim().Replace(".", ""));

                var myCheckVersione = await WSChiamateEF.getCheckVersione(App.K_DeviceID, int_versione_build);
                if (myCheckVersione.esitoOK_KO == "KO")
                {
                    string labelMessaggio = AppResources.AlertLabelMessaggio;
                    ErroreVersione = myCheckVersione.errorMessageKO;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, ErroreVersione, "OK");
                    return;
                }

                if (myCheckVersione.esitoOK_KO == "OK")
                {
                    //20190130 ulteriore condizione di visibilità della chatbot (inizio)
                    try
                    {
                        using (SQLite.SQLiteConnection cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                        {
                            var lTableExist = await sqliteHelper.sqlite_TableExist<FEC_CustomersSystem>(App.sqliteDbName);
                            if (lTableExist)
                            {
                                var l = cn.Table<FEC_CustomersSystem>().ToList();
                                cn.Close();

                                if (l.Count == 1)
                                {
                                    if (l[0].TaxIDNumber == "03516641200" || l[0].TaxIDNumber == "07483880964" || l[0].TaxIDNumber == "02480891205") //francesca,alessandro,alterbit
                                        this.slChatBot.IsVisible = true;
                                }
                            }
                        }
                        //20190130 ulteriore condizione di visibilità della chatbot (fine)
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        //trap
                    }

                    //20190123 inizio
                    if (this.slChatBot.IsVisible == false) //se non è già diventato true per effetto di una delle 3 partite iva
                    {
                        if (myCheckVersione.contentOK.Contains("selfself"))
                            this.slChatBot.IsVisible = true;
                        else
                        {
                            if (this.slChatBot.IsVisible == true)
                                this.slChatBot.IsVisible = false;
                        }
                    }
                    //20190123 fine
                    ErroreVersione = "";
                }




            }

            //20181129NavigationPage.SetHasNavigationBar(this, false);
            this.Title = "Menu"; //20181129
            if (isLoaded == false)
            {
                var np = Application.Current.MainPage as NavigationPage;
                var myHudClass = new SettaBusyHud(np.CurrentPage as ContentPage);
                //string labelHud = "Sincronizzazione tabelle con il server...";
                //myHudClass.SettaVisible((Color)Application.Current.Resources["NavigationBarBackgroundColor"], labelHud);
                var tSincro = await WSChiamateEF.getTabelleAnagrafichexFatturazione();

                if (tSincro.Item5 != "")
                {
                    //myHudClass.settaInvisible();
                    //this.IsBusyHud = false;

                    string labelMessaggio = AppResources.AlertLabelMessaggio;
                    var descr = AppResources.SincroMessErrore;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, descr, "OK");

                    try
                    {
                        await WSChiamateEF.writeSqlLiteLOG("SINCRO", descr + Environment.NewLine + tSincro.Item5);
                    }
                    catch
                    {

                    }

                    return;
                }
                else
                {
                    //myHudClass.settaInvisible();
                }

                WSChiamateEF.Delete_and_PerformVacuum(false); //20181129: volutamente sincrona

                await WSChiamateEF.writeSqlLiteLOG("ENTRATAMENU", App.K_DeviceID + App.K_Platform + App.K_Version + Environment.NewLine, true); //20181128 manda email mailgun (unico caso)



                isLoaded = true;
            }
            SincronizzaCONTimer();
        }

        private void SincronizzaCONTimer()
        {

            // Start a timer that runs after 5 minute.
            Device.StartTimer(TimeSpan.FromMinutes(5), () =>
            {
                Boolean retVal = true;
                Task.Factory.StartNew(async () =>
                {
                    //if (App.K_IsLoggedIn == false)
                    //{
                    //    retVal = false;
                    //    return false;
                    //}
                    try
                    {
                        //Boolean lInternetAvailable = await Utility.isInternetAvailable2();
                        var tSincro = await WSChiamateEF.getTabelleAnagrafichexFatturazione();
                        //var abc = 1;
                        //Nota: non importa testare la presenza della connessione internet perchè già eseguita in getTabelleAnagrafichexFatturazione
                        //Nota: non testo il valore di ritorno perchè deve essere silente
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        //possibile errore dovuto al fatto che il database sia loccato da qualche form aperta
                        //HockeyApp.MetricsManager.TrackEvent(App.GetDatelog() + "Errore sincronizzazione batch dei tipi N " + ex.Message + " " + App.K_Employee1Matricola + " " + App.K_CompanyCode);
                    }
                    finally
                    {
                        //App.K_IsScheduledSynchronizationUnderway = false; //not usata
                    }
                    //return true;

                    retVal = true;
                    return true;
                });
                return retVal;
            });
        }
    }
}