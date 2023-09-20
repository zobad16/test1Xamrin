using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using xUtilityPCL;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Diagnostics;
using Plugin.Connectivity;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using Plugin.DeviceInfo;
using System.Text.RegularExpressions;

namespace test1Xamrin
{
    public partial class App : Application
    {
        public static float k_screenW { get; set; }
        public static float k_screenH { get; set; }
        public static float k_Density { get; set; }
        public static platformSpecific K_currentPlatform { get; set; }
        public static IGeolocator K_Locator { get; set; }
        public static double K_Latitudine { get; set; }
        public static double K_Longitudine { get; set; }
        public static double K_Altitudine { get; set; }
        public static string K_StreetNameReverse { get; set; }
        public static string baseURL { get; set; } = "";
        public static string K_DeviceID { get; set; }
        public static string K_Model { get; set; }
        public static string K_Platform { get; set; }
        public static string K_Version { get; set; }
        //public static Boolean K_BOTVisibile { get; set; } = true; //20190122
        public static string sqliteDbName = "FEC.db";
        public static string FECController_ActionPOSTInvoiceRequests = "FEC/postInvoiceRequestsSYSTEM";
        public static string FECController_ActionPOSTCustomers = "FEC/postCustomersSYSTEM";
        public static string K_BearerChatBot { get; set; } = "_QHpY260YAA.cwA.-pc.hEy9dsYA7Z0DvwYTcexrJOMOHKzRLMXROTqV_Pwjkuk";
        public static string K_Please_Upload_the_QR_image = "Please upload the QR image";
        public static string K_Scegli_il_carburante = "Scegli il carburante";
        public static string K_Please_Read_the_QR = "Leggi il QR";
        public static string K_Digita_Importo = "Digita l'importo pagato";
        public static string K_Digita_ReceiptNumber = "Digita n° ricevuta";

        public static string K_Fa_Una_Foto_Ricevuta = "Fa una foto della ricevuta";
        public static string K_Scegli_modalità_pagamento = "Scegli modalità pagamento";
        public static string K_Digita_Tua_PartitaIva = "Digita la tua partita iva";
        public static string K_ConfermaFinale = "Confermi i dati inseriti?";
        public static string K_ChatChiusa = "Chat chiusa";

        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();

            #region"Attività preliminari"
            K_currentPlatform = DependencyService.Get<platformSpecific>();

            try
            {
                var col = (Color)Application.Current.Resources["NavigationBarBackgroundColor"];
                //ApplyAccentColor(col);//(Color.FromHex("34B5E5"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

            Microsoft.AppCenter.AppCenter.Start("ios=d227780a-41ba-43d3-aa76-43d6c9a36f6d;" + "uwp={Your UWP App secret here};" + "android=bedcfde5-32ea-4594-879a-a86d4d5015e0;",
                                        typeof(Microsoft.AppCenter.Analytics.Analytics), typeof(Microsoft.AppCenter.Crashes.Crashes));


            #endregion

            #region "Permessi"
            if (Device.RuntimePlatform == Device.Android)
            {
                Task.Run(async () =>
                {

                    try
                    {
                        var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                        if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                            {
                                //                                  await Application.Current.MainPage.DisplayAlert("Need location", "Gunna need that location", "OK");
                            }

                            //var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                            var results = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission   > ();
                            status = results;
                        }

                        var status2 = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
                        if (status2 != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                            {
                                //                                  await Application.Current.MainPage.DisplayAlert("Need location", "Gunna need that location", "OK");
                            }

                            var results2 = await CrossPermissions.Current.RequestPermissionAsync< LocationPermission>();
                            status2 = results2;
                        }

                        var status3 = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                        if (status3 != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                            {
                                //                                  await Application.Current.MainPage.DisplayAlert("Need location", "Gunna need that location", "OK");
                            }

                            var results3 = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                            status3 = results3;
                        }



                        //                          if (status == PermissionStatus.Granted)
                        //                          {
                        //                              var results = await CrossGeolocator.Current.GetPositionAsync(10000);
                        //                              LabelGeolocation.Text = "Lat: " + results.Latitude + " Long: " + results.Longitude;
                        //                          }
                        //                          else if(status != PermissionStatus.Unknown)
                        //                          {
                        //                              await Application.Current.MainPage.DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                        //                          }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        //trap
                    }
                });


            }



            #endregion

            #region "GeoLocator"

            K_Locator = CrossGeolocator.Current;
            if (K_Locator.IsGeolocationAvailable)
            {
                K_Locator.DesiredAccuracy = 20;//20160928 era 50
                if (K_Locator.IsListening == false)
                {
                    K_Locator.StartListeningAsync(TimeSpan.FromMilliseconds(60000), 20, true); //60 sec e 500 metri //20160804 cambiato a da 500 a 200 metri e il 05/08/2016 a 100 metri e il 28/09/2016 a 10 metri
                    Task.Run(async () =>
                    {
                        var position = await K_Locator.GetPositionAsync(timeout: TimeSpan.FromMilliseconds(10000));
                        K_Altitudine = position.Altitude;
                        K_Longitudine = position.Longitude;
                        K_Latitudine = position.Latitude;
                        //ricavo l'indirizzo dalle coordinate (inizio) 20161110
                        try
                        {

                            var pos = new Xamarin.Forms.Maps.Position(K_Latitudine, K_Longitudine);

                            var geoCoder = new Geocoder();
                            var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(pos);
                            if (possibleAddresses != null)
                            {
                                Int32 i = 0;
                                foreach (var address in possibleAddresses)
                                {
                                    //  reverseGeocodedOutputLabel.Text += address + "\n";
                                    if (i == 0)
                                    {
                                        var laddress = address.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        if (laddress.Length > 0)
                                        {
                                            App.K_StreetNameReverse = laddress[0];
                                        }
                                    }
                                    i += 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //trap
                            Console.WriteLine(ex.ToString());
                        }
                        //ricavo l'indirizzo dalle coordinate (fine) 20161110


                    });
                }



                K_Locator.PositionChanged += async delegate (object sender, PositionEventArgs e)
                {
                    K_Altitudine = e.Position.Altitude;
                    K_Longitudine = e.Position.Longitude;
                    K_Latitudine = e.Position.Latitude;
                    //ricavo l'indirizzo dalle coordinate (inizio) 20161110
                    try
                    {
                        //K_Latitudine = 45.45218277;
                        //K_Longitudine = 8.61883926; //via dei mille novara
                        var pos = new Xamarin.Forms.Maps.Position(K_Latitudine, K_Longitudine);

                        var geoCoder = new Geocoder();
                        var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(pos);
                        if (possibleAddresses != null)
                        {
                            Int32 i = 0;
                            foreach (var address in possibleAddresses)
                            {
                                //  reverseGeocodedOutputLabel.Text += address + "\n";
                                if (i == 0)
                                {
                                    var laddress = address.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    if (laddress.Length > 0)
                                    {
                                        App.K_StreetNameReverse = laddress[0];
                                    }
                                }
                                i += 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //trap
                        Console.WriteLine(ex.ToString());
                    }
                    //ricavo l'indirizzo dalle coordinate (fine) 20161110


                    //20160928 inizio commento
                    //                  var dataOra = DateTime.Now.ToString ("dd/MM/yyyy HH:mm:ss");
                    //                  if (K_Positions == null) {
                    //                      K_Positions = new List<string> ();
                    //                      K_Positions.Add ("Record di test");
                    //                  }
                    //                  var myPos = $"data:{dataOra} Latitudine: {K_Latitudine} Longitudine: {K_Longitudine}";
                    //                  K_Positions.Add (myPos);
                    //20160928 fine commento
                };
            }

            #endregion

            #region "url di partenza"
            //20180107 inizio
            Task.Run(async () =>
            {
                var d = await Utility.isInternetAvailable2();
                if (d == true)
                {
                    //settare qui 
                    //App.baseURL = "http://192.168.1.10/xFatturazioneElettronicaWebApi/api/";
                    //App.baseURL = "http://192.168.1.115/xFatturazioneElettronicaWebApi/api/";
                    //App.baseURL = "http://54.38.180.44/xFatturazioneElettronicaWebApi/api/";
                    //App.baseURL = "http://sviluppoback.sabicom.cloud/xFatturazioneElettronicaWebApi/api/";
                    //App.baseURL = "http://srvvending.westeurope.cloudapp.azure.com:8095/xFatturazioneElettronciaWebApi/api/";

                    //var lContinuaAScalare = true;
                    //20190131var a1 = await CrossConnectivity.Current.IsRemoteReachable("www.anagrafecaninarer.it", 80, 5000);
                    var a1 = await CrossConnectivity.Current.IsRemoteReachable("http://acrer.alterbit.cs.factor-y.net", 80, 5000);//20190131
                    if (a1 == true && App.baseURL.Contains("192.168.1") == false && App.baseURL.Contains("sviluppoback") == false)
                    {
                        //invoco un metodo che legge dalla tabella activations e mi dà l'url (INIZIO)
                        var urlDAAnagrafe = "";
                        //20190128 using (var client = new RestClient(new Uri("http://appserver.anagrafecaninarer.it/AlterbitMobileWEBAPI/api")))
                        using (var client = new RestClient(new Uri("http://acrer.alterbit.cs.factor-y.net/AlterbitMobileWEBAPI/api")))//20190128
                        {
                            client.HttpClientFactory = new HttpClientFactoryCustom(); //20171022
                            var request = new RestRequest("Activations/getLinkxFatturazioneAttiva", Method.GET);
                            request.AddHeader("Authorization-Token",
                                            "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230");


                            try
                            {
                                var t = await client.Execute<string>(request);
                                if (t.StatusCode != System.Net.HttpStatusCode.OK)
                                {
                                    //non faccio nulla: lContinuaAScalare deve rimanere a true
                                }
                                else
                                {
                                    urlDAAnagrafe = t.Data;
                                }


                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                //non faccio nulla: lContinuaAScalare deve rimanere a true
                            }

                        }


                        if (string.IsNullOrEmpty(urlDAAnagrafe) == false)
                        {
                            App.baseURL = urlDAAnagrafe;
                            xUtilityPCL.Global.BaseURL = App.baseURL;
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //fase approvazione apple (inizio) 20180706
                                if (Device.RuntimePlatform == Device.iOS)
                                {
                                    if (urlDAAnagrafe.EndsWith("/"))
                                    {
                                        //App.K_Attivazione_IDCompany = 8;
                                        //App.K_Attivazione_Series = null;
                                        //App.K_Attivazione_CompanyType = 1; //20180924
                                        ////non importano i campi Fundxxx perchè apple tetserà solo i carburanti
                                        //App.K_Attivazione_Acronym = ""; //20180908 "FEC";
                                        //App.K_Attivazione_ActivationCode = "DEMO";
                                        //K_IsAttivata = true;
                                        //FEC_Activation drActivation = new FEC_Activation();
                                        //using (var cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
                                        //{
                                        //    drActivation.IDCompany = 8;
                                        //    drActivation.Series = null;
                                        //    drActivation.Acronym = ""; //20180908 "FEC";
                                        //    drActivation.ActivationCode = "DEMO";
                                        //    drActivation.ServiceType = "NoLimit"; //20180908 ex BASE
                                        //    cn.Insert(drActivation);
                                        //    cn.Close();
                                        //}
                                        //SettaFormPartenza(false, false);
                                    }
                                    else
                                    {
                                        //SettaFormPartenza(false, true);
                                    }
                                }
                                else
                                {
                                    //SettaFormPartenza(false, true);
                                }

                                //fase approvazione apple (fine) 20180706
                                //if (K_IsAttivata)
                                //{
                                //    if (K_IsUserLoggedIn == false) //20180701 aggiunto IF
                                //        (this.MainPage as LoginSabicom).settabtnLoginEnabled();
                                //    else
                                //    {
                                //        //credenziali nello storage oppure riavvio dovuto a cambio cultura 
                                //        //nota: non ci possono essere altri casi di riavvio se non il cambio cultura in quanto è SingleInstance
                                //    }

                                //}
                                //else
                                //((this.MainPage as NavigationPage).CurrentPage as Profilo).settabtnSalvaEnabled();
                            });


                            //K_DeviceID = CrossDeviceInfo.Current.Id;
                            //K_Model = CrossDeviceInfo.Current.Model;
                            //K_Platform = CrossDeviceInfo.Current.Platform.ToString();
                            //K_DeviceID = K_currentPlatform.Device_getDeviceID();
                            //K_Version = CrossDeviceInfo.Current.Version.ToString();


                            //chiamare restapi postRegistraDeviceSYSTEM
                            //valorizzare enable=true
                            //inserted=datettime.now

                            //FEC_DevicesSystem drDevice = new FEC_DevicesSystem();
                            //drDevice.DeviceID = App.K_DeviceID;
                            //drDevice.OS = App.K_Platform;
                            //drDevice.Brand = App.K_Version;
                            //drDevice.Model = App.K_Model;
                            //drDevice.Enabled = true;
                            //drDevice.InsertedDate = DateTime.Now;

                            var drDevice = SettaDevice();

                            var ret = await WSChiamateEF.postRegistraDevice(drDevice);
                            if (ret.esitoOK_KO == "KO")
                            {

                            }

                            if (ret.esitoOK_KO == "OK")
                            {

                            }


                            //#endregion


                        }
                    }
                    else
                    {
                        var drDevice = SettaDevice();

                        var ret = await WSChiamateEF.postRegistraDevice(drDevice);
                        if (ret.esitoOK_KO == "KO")
                        {

                        }

                        if (ret.esitoOK_KO == "OK")
                        {

                        }

                        Device.BeginInvokeOnMainThread(() =>
                        {

                        });
                    }

                }

            });


            #endregion

            //Charsets charset;
            var challenge = @"^[\p{IsBasicLatin}\p{IsLatin-1Supplement}]+$";
            var lSuccess = Regex.Match("Via Sant’Ignazio 2", challenge).Success;

            var lSuccess2 = Regex.Match("Via Sant Ignazio 2", challenge).Success;
            var np = new NavigationPage(new MenuSelf());
            np.BarBackgroundColor = (Color)Application.Current.Resources["NavigationBarBackgroundColor"];
            np.BarTextColor = (Color)Application.Current.Resources["NavigationBarTextColor"];
            Application.Current.MainPage = np;
        }
        private FEC_DevicesSystem SettaDevice() //20181210
        {
            //region "Info sulla device" (inizio)

            K_DeviceID = CrossDeviceInfo.Current.Id;
            K_Model = CrossDeviceInfo.Current.Model;
            K_Platform = CrossDeviceInfo.Current.Platform.ToString();
            K_DeviceID = K_currentPlatform.Device_getDeviceID();

            K_Version = CrossDeviceInfo.Current.Version.ToString();


            //chiamare restapi postRegistraDeviceSYSTEM
            //valorizzare enable=true
            //inserted=datettime.now

            FEC_DevicesSystem drDevice = new FEC_DevicesSystem();
            drDevice.DeviceID = App.K_DeviceID;
            drDevice.OS = App.K_Platform;
            drDevice.Brand = App.K_Version;
            drDevice.Model = App.K_Model;
            drDevice.Enabled = true;
            drDevice.InsertedDate = DateTime.Now;
            return drDevice;

        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
