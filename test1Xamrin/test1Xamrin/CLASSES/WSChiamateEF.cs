using Microsoft.AppCenter.Analytics;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using test1Xamrin;
using test1Xamrin.Resx;
using Xamarin.Forms;
using xUtilityPCL;
using test1Xamrin.QR;

namespace test1Xamrin
{
    public class WSChiamateEF
    {
        public static async Task<Tuple<RootObjectSYSTEM, Int32, Int32, Int32, string>> getTabelleAnagrafichexFatturazione()
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;
            Boolean lInternetAvailable = await Utility.isInternetAvailable2();

            if (lInternetAvailable == false)
            {
                string msgConnessioneInternet = AppResources.SincroConnessioneInternet;
                //await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgConnessioneInternet, "OK");
                //return;
                return new Tuple<RootObjectSYSTEM, Int32, Int32, Int32, string>(new RootObjectSYSTEM(), 0, 0, 0, msgConnessioneInternet);

            }

            var lSincronizzaPOST = true;

            //chiamate standard alla funzione di sincronizzazione (inizio)
            Int32 nRecDaSincronizzareCustomers = 0;
            Int32 nRecDaSincronizzareInvoiceRequests = 0;

            if (lSincronizzaPOST)
            {
                Action<RestRequest> myRequestCustomers = (RestRequest rr) =>
                {
                    //Parameter p = new Parameter();
                    //p.Name = "IDCompany";
                    //p.Value = App.K_Attivazione_IDCompany;
                    //p.Type = ParameterType.QueryString;
                    //rr.AddParameter(p);
                    //nota: voluto i customers non hanno alcun idCompany perchè formalmente sono collegati a tutti
                };

                nRecDaSincronizzareCustomers = await xUtilityPCL.sqliteHelper.sqlite_SincronizzaExistingLocalTable<FEC_CustomersSystem>
                                                (App.sqliteDbName, xUtilityPCL.Global.BaseURL, App.FECController_ActionPOSTCustomers,
                                                             null, false, null, "0", null, myRequestCustomers); //20171101 passato parametro valoreOK);
                if (nRecDaSincronizzareCustomers == -1)
                {
                    try
                    {
                        //Analytics.TrackEvent("POST ServiceCall Errore " + App.K_tblUser_NomeUtente, new Dictionary<string, string> {
                        //  { "Utente", App.K_tblUser_NomeUtente  },
                        //  { "Data", DateTime.Now.ToString()},
                        //});
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    return new Tuple<RootObjectSYSTEM, Int32, Int32, Int32, string>(new RootObjectSYSTEM(), 0, 0, 0, "errore sincronizzazione Customers");
                }

                Action<RestRequest> myRequestInvoiceRequests = (RestRequest rr) =>
                {
                    //Parameter p = new Parameter();
                    //p.Name = "IDCompany";
                    //p.Value = App.K_Attivazione_IDCompany;
                    //p.Type = ParameterType.QueryString;
                    //rr.AddParameter(p);

                };

                nRecDaSincronizzareInvoiceRequests = await xUtilityPCL.sqliteHelper.sqlite_SincronizzaExistingLocalTable<FEC_InvoiceRequests>
                                                (App.sqliteDbName, xUtilityPCL.Global.BaseURL, App.FECController_ActionPOSTInvoiceRequests,
                                                 null, false, null, "0", null, myRequestInvoiceRequests); //20171101 passato parametro valoreOK);
                if (nRecDaSincronizzareInvoiceRequests == -1)
                {
                    try
                    {
                        //Analytics.TrackEvent("POST ServiceCall Errore " + App.K_tblUser_NomeUtente, new Dictionary<string, string> {
                        //  { "Utente", App.K_tblUser_NomeUtente  },
                        //  { "Data", DateTime.Now.ToString()},
                        //});
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    return new Tuple<RootObjectSYSTEM, Int32, Int32, Int32, string>(new RootObjectSYSTEM(), nRecDaSincronizzareCustomers, 0, 0, "errore sincronizzazione InvoiceRequests");
                }
            }

            //chiamate standard alla funzione di sincronizzazione (fine)
            //gestione parametro taxidnumber (inizio)
            var parTaxIDNumber = "";
            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                var lEsiste = await sqliteHelper.sqlite_TableExist<FEC_Counties>(App.sqliteDbName);
                if (lEsiste)
                {
                    var lRegistrazioni = cn.Table<FEC_CustomersSystem>().ToList();
                    foreach (var dr in lRegistrazioni)
                    {
                        parTaxIDNumber += dr.TaxIDNumber + ";";
                    }

                }
                cn.Close();
            }
            //gestione parametro taxidnumber (fine)

            //esecuzione metodo get (inizio)
            string url = App.baseURL;
            var client = new RestClient(url);
            client.HttpClientFactory = new HttpClientFactoryCustom();

            string req = "";
            RootObjectSYSTEM roGlobal = new RootObjectSYSTEM();
            req = "FEC/getTabelleAnagraficheFECSYSTEM";
            var request = new RestRequest(req, Method.GET);
            Parameter p1 = new Parameter();
            p1.Name = "TaxIDNumber";
            p1.Value = parTaxIDNumber;
            request.AddParameter(p1);
            //Parameter p2 = new Parameter();
            //p2.Name = "idUser";
            //p2.Value = 0;
            //request.AddParameter(p2);
            request.AddHeader("Authorization-Token", "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230");

            try
            {
                var r = await client.Execute<RootObjectSYSTEM>(request);
                if (r.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (r.Data == null)
                    {
                        try
                        {
                            //  Analytics.TrackEvent("getTabelleAnagrafichexTime " + App.K_tblUser_NomeUtente, new Dictionary<string, string> {
                            //       { "Utente", App.K_tblUser_NomeUtente  },
                            //       { "Data", DateTime.Now.ToString()},
                            //});
                        }
                        catch (Exception ex1)
                        {
                            Console.WriteLine(ex1.ToString());
                            //trap
                        }
                        return new Tuple<RootObjectSYSTEM, Int32, Int32, Int32, string>(new RootObjectSYSTEM(), nRecDaSincronizzareCustomers, nRecDaSincronizzareInvoiceRequests, 0, "errore rilettura tabelle anagrafiche (1)");
                    }
                    else
                        roGlobal = r.Data;
                }
                else
                {
                    try
                    {
                        //Analytics.TrackEvent("getTabelleAnagrafichexTime " + App.K_tblUser_NomeUtente, new Dictionary<string, string> {
                        //       { "Utente", App.K_tblUser_NomeUtente  },
                        //       { "Data", DateTime.Now.ToString()},
                        //});
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }
                    return new Tuple<RootObjectSYSTEM, Int32, Int32, Int32, string>(new RootObjectSYSTEM(), nRecDaSincronizzareCustomers, nRecDaSincronizzareInvoiceRequests, 0, "errore rilettura tabelle anagrafiche (2)");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine(ex.ToString());
                    //Analytics.TrackEvent("getTabelleAnagrafichexTime " + App.K_tblUser_NomeUtente, new Dictionary<string, string> {
                    //  { "Utente", App.K_tblUser_NomeUtente  },
                    //  { "Data", DateTime.Now.ToString()},
                    //});


                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }
                return new Tuple<RootObjectSYSTEM, Int32, Int32, Int32, string>(new RootObjectSYSTEM(), nRecDaSincronizzareCustomers, nRecDaSincronizzareInvoiceRequests, 0, "errore rilettura tabelle anagrafiche(3)");
            }
            //esecuzione metodo get (fine)



            //RootObjectBindable ro = new RootObjectBindable ();

            using (var cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {

                //anagafiche pure (inizio)

                cn.DropTable<FEC_Counties>();
                cn.DropTable<FEC_Countries>();
                cn.DropTable<FEC_Activation>();

                cn.CreateTable<FEC_Counties>();
                cn.CreateTable<FEC_Countries>();
                cn.CreateTable<FEC_Activation>();
                //anagrafiche pure (fine)


                cn.InsertAll(roGlobal.list_FEC_Counties, true);
                cn.InsertAll(roGlobal.list_FEC_Countries, true);
                cn.InsertAll(roGlobal.list_FEC_Activation, true);

                cn.Close();
                //anagafiche pure (fine)
            }


            try
            {
                //Boolean l = false;
                //l = await xUtilityPCL.sqliteHelper.sqlite_AggiornaLocalTableFromCollection<FEC_PaymentTerms>(App.sqliteDbName, roGlobal.list_FEC_PaymentTerms);
                //l = await xUtilityPCL.sqliteHelper.sqlite_AggiornaLocalTableFromCollection<FEC_TaxCode>(App.sqliteDbName,
                //                                                                                      roGlobal.list_FEC_TaxCode);
                //implementareApp.SetTechnician();
                //implementareApp.SetParametriFromtblConfigurazione();


                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {

                    // tabelle di movimento
                    cn.DropTable<FEC_InvoiceRequests>();
                    cn.CreateTable<FEC_InvoiceRequests>();
                    cn.InsertAll(roGlobal.list_FEC_InvoiceRequests, true);


                    cn.DropTable<FEC_CustomersSystem>();
                    cn.CreateTable<FEC_CustomersSystem>();
                    cn.InsertAll(roGlobal.list_FEC_CustomersSystem, true);


                    cn.Close();

                }






                return new Tuple<RootObjectSYSTEM, Int32, Int32, Int32, string>(roGlobal, nRecDaSincronizzareCustomers, nRecDaSincronizzareInvoiceRequests, 0, "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                try
                {
                    //Analytics.TrackEvent("getTabelleAnagrafichexTime " + App.K_tblUser_NomeUtente, new Dictionary<string, string> {
                    //  { "Utente", App.K_tblUser_NomeUtente  },
                    //  { "Data", DateTime.Now.ToString()},
                    //  { "Msg", "errore scrittura dati ricevuti sul database locale"}
                    //});


                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }



                return new Tuple<RootObjectSYSTEM, Int32, Int32, Int32, string>(new RootObjectSYSTEM(),
                    0, 0, 0, "errore scrittura dei dati ricevuti sul database locale");
            }
            //aggiornate le tabelle locali (fine
        }

        public static async Task<ReturnClassValidazioneTaxIDNumber> getVerificaTaxIDNumber(string countryCode, string TaxIDNumber)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;
            //esecuzione metodo get (inizio)
            string url = App.baseURL;
            var client = new RestClient(url);
            client.HttpClientFactory = new HttpClientFactoryCustom();

            string req = "";
            RootObject roGlobal = new RootObject();
            req = "FEC/getVerificaTaxIDNumber";
            var request = new RestRequest(req, Method.GET);

            Parameter p1 = new Parameter();
            p1.Name = "countryCode";
            p1.Value = countryCode;
            request.AddParameter(p1);

            Parameter p2 = new Parameter();
            p2.Name = "TaxIDNumber";
            p2.Value = TaxIDNumber;
            request.AddParameter(p2);

            request.AddHeader("Authorization-Token", "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230");

            try
            {
                var r = await client.Execute<ReturnClassValidazioneTaxIDNumber>(request);

                if (r.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        Analytics.TrackEvent("getVerificaTaxIDNumber KO-1 " + " " + r.StatusCode + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                            //{ "Utente", IDCompany.ToString() },
                                 { "Data", DateTime.Now.ToString()},
                        });
                        //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-1", "getVerificaTaxIDNumber KO-1 " + " " + r.StatusCode + " " + countryCode + " " + TaxIDNumber, false);
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;

                    var retVal = new ReturnClassValidazioneTaxIDNumber();
                    retVal.esitoOK_KO = "KO";
                    retVal.codeMessageKO = "KO-1";
                    retVal.errorMessageKO = msgErroreHTTP + r.StatusCode;

                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");

                    return retVal;
                }
                else
                {
                    //codice http corretto: 200

                    if (r.Data.esitoOK_KO == "KO")
                    {
                        string msgErroreGenerico = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreGenerico;
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, r.Data.errorMessageKO, "OK");
                    }

                    if (r.Data.esitoOK_KO == "OK")
                    {
                        //nessun messaggio: in certe logiche (es attivazione) si potrebbe dare un messaggio da localizzare
                    }

                    try
                    {
                        Analytics.TrackEvent("getVerificaTaxIDNumber OK " + " " + r.Data.esitoOK_KO + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                            //{ "Utente", IDCompany.ToString() },
                            { "Data", DateTime.Now.ToString()},
                        });
                        //await writeSqlLiteLOG("getVerificaTaxIDNumber OK", "getVerificaTaxIDNumber OK " + " " + r.Data.esitoOK_KO + " " + countryCode + " " + TaxIDNumber, false);

                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                }

                return r.Data;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                try
                {
                    Analytics.TrackEvent("getVerificaTaxIDNumber KO-1 " + " " + ex.Message + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                        //{ "Utente", IDCompany.ToString() },
                        { "Data", DateTime.Now.ToString()},
                        { "Eccezione", ex.Message},
                        });
                    //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-1", "getVerificaTaxIDNumber KO-1 " + " " + ex.Message + " " + countryCode + " " + TaxIDNumber, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;

                var retVal = new ReturnClassValidazioneTaxIDNumber();
                retVal.esitoOK_KO = "KO";
                retVal.codeMessageKO = "KO-1";
                retVal.errorMessageKO = msgErroreHTTP + ex.Message;

                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");

                return retVal;
            }
            catch (Exception ex)
            {
                try
                {
                    Analytics.TrackEvent("getVerificaTaxIDNumber KO-2 " + ex.Message + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                        //{ "Utente", IDCompany.ToString() },
                        { "Data", DateTime.Now.ToString()},
                        { "Eccezione", ex.Message},
                        });
                    //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-2", "getVerificaTaxIDNumber KO-2 " + ex.Message + " " + countryCode + " " + TaxIDNumber, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreGenerico = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreGenerico;

                var retVal = new ReturnClassValidazioneTaxIDNumber();
                retVal.esitoOK_KO = "KO";
                retVal.codeMessageKO = "KO-2";
                retVal.errorMessageKO = msgErroreGenerico + ": " + ex.StackTrace;

                await Application.Current.MainPage.DisplayAlert(labelMessaggio, retVal.errorMessageKO, "OK");

                return retVal;
            }
        }

        public static async Task<String> getAlmenoUnaFatturaByTaxIDNumber(string TaxIDNumber)
        //ritorna SI NO KO
        {
            //esecuzione metodo get (inizio)
            string url = App.baseURL;
            var client = new RestClient(url);
            client.HttpClientFactory = new HttpClientFactoryCustom();

            string req = "";
            RootObject roGlobal = new RootObject();
            req = "FEC/getAlmenoUnaFatturaByTaxIDNumberSYSTEM";
            var request = new RestRequest(req, Method.GET);

            Parameter p1 = new Parameter();
            p1.Name = "TaxIDNumber";
            p1.Value = TaxIDNumber;
            request.AddParameter(p1);

            request.AddHeader("Authorization-Token", "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230");

            try
            {
                var r = await client.Execute<Boolean>(request);
                if (r.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        //Analytics.TrackEvent("getAlmenoUnaFatturaByTaxIDNumber OK" + App.K_Attivazione_IDCompany + "_" + App.K_FEC_Users.Username, new Dictionary<string, string> {
                        //         { "Utente", App.K_FEC_Users.Username },
                        //         { "Data", DateTime.Now.ToString()},
                        //});
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    if (r.Data)
                        return "SI";
                    else
                        return "NO";
                }
                else
                {
                    try
                    {
                        //Analytics.TrackEvent("getAlmenoUnaFatturaByTaxIDNumber KO" + App.K_Attivazione_IDCompany + "_" + App.K_FEC_Users.Username, new Dictionary<string, string> {
                        //         { "Utente", App.K_FEC_Users.Username },
                        //         { "Data", DateTime.Now.ToString()},
                        //});
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    return "KO";
                    //return new Tuple<RootObject, Int32, Int32, Int32, string>(new RootObject(), nRecDaSincronizzareCustomers, nRecDaSincronizzareItems, nRecDaSincronizzareSaleDocs, "errore rilettura tabelle anagrafiche (2)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                try
                {
                    //Analytics.TrackEvent("getAlmenoUnaFatturaByTaxIDNumber KO2" + App.K_Attivazione_IDCompany + "_" + App.K_FEC_Users.Username, new Dictionary<string, string> {
                    //             { "Utente", App.K_FEC_Users.Username },
                    //             { "Data", DateTime.Now.ToString()},
                    //});
                }
                catch (Exception ex1)
                {
                    //trap
                    Console.WriteLine(ex1.ToString());
                }

                return "KO";
            }
            //esecuzione metodo get (fine)
        }

        public static async Task<String> getControlloReceiptNumberGiaPresenteIDCompany(string ReceiptNumber, Int32 IDCompany, Int32 anno)
        //ritorna SI NO KO
        {
            //esecuzione metodo get (inizio)
            string url = App.baseURL;
            var client = new RestClient(url);
            client.HttpClientFactory = new HttpClientFactoryCustom();

            string req = "";
            RootObject roGlobal = new RootObject();
            req = "FEC/getControlloReceiptNumberGiaPresenteIDCompanySYSTEM";
            var request = new RestRequest(req, Method.GET);

            Parameter p1 = new Parameter();
            p1.Name = "ReceiptNumber";
            p1.Value = ReceiptNumber;
            request.AddParameter(p1);

            Parameter p2 = new Parameter();
            p2.Name = "IDCompany";
            p2.Value = IDCompany;
            request.AddParameter(p2);

            Parameter p3 = new Parameter();
            p3.Name = "anno";
            p3.Value = anno;
            request.AddParameter(p3);

            request.AddHeader("Authorization-Token", "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230");

            try
            {
                var r = await client.Execute<Boolean>(request);
                if (r.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        //Analytics.TrackEvent("getAlmenoUnaFatturaByTaxIDNumber OK" + App.K_Attivazione_IDCompany + "_" + App.K_FEC_Users.Username, new Dictionary<string, string> {
                        //         { "Utente", App.K_FEC_Users.Username },
                        //         { "Data", DateTime.Now.ToString()},
                        //});
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    if (r.Data)
                        return "SI";
                    else
                        return "NO";
                }
                else
                {
                    try
                    {
                        //Analytics.TrackEvent("getAlmenoUnaFatturaByTaxIDNumber KO" + App.K_Attivazione_IDCompany + "_" + App.K_FEC_Users.Username, new Dictionary<string, string> {
                        //         { "Utente", App.K_FEC_Users.Username },
                        //         { "Data", DateTime.Now.ToString()},
                        //});
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    return "KO";
                    //return new Tuple<RootObject, Int32, Int32, Int32, string>(new RootObject(), nRecDaSincronizzareCustomers, nRecDaSincronizzareItems, nRecDaSincronizzareSaleDocs, "errore rilettura tabelle anagrafiche (2)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                try
                {
                    //Analytics.TrackEvent("getAlmenoUnaFatturaByTaxIDNumber KO2" + App.K_Attivazione_IDCompany + "_" + App.K_FEC_Users.Username, new Dictionary<string, string> {
                    //             { "Utente", App.K_FEC_Users.Username },
                    //             { "Data", DateTime.Now.ToString()},
                    //});
                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                return "KO";
            }
            //esecuzione metodo get (fine)
        }

        public static async Task getItemsBenzinaio(Int32 IDCompany)
        {
            //esecuzione metodo get (inizio)
            string labelMessaggio = AppResources.AlertLabelMessaggio;
            string url = App.baseURL;
            var client = new RestClient(url);
            client.HttpClientFactory = new HttpClientFactoryCustom();

            string req = "";
            RootObject roGlobal = new RootObject();
            req = "FEC/getItemsBenzinaioSYSTEM";
            var request = new RestRequest(req, Method.GET);

            Parameter p1 = new Parameter();
            p1.Name = "IDCompany";
            p1.Value = IDCompany;
            request.AddParameter(p1);

            request.AddHeader("Authorization-Token", "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230");

            //await Task.Delay(5000);
            try
            {
                var r = await client.Execute<List<FEC_Items>>(request);
                if (r.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        //Analytics.TrackEvent("getVerificaTaxIDNumber KO-1 " + " " + r.StatusCode + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                        //    //{ "Utente", IDCompany.ToString() },
                        //         { "Data", DateTime.Now.ToString()},
                        //});
                        //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-1", "getVerificaTaxIDNumber KO-1 " + " " + r.StatusCode + " " + countryCode + " " + TaxIDNumber, false);
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");
                }
                else
                {
                    //codice http corretto: 200
                    //drop
                    //create
                    //insertall

                    using (var cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                    {
                        var lTableExist = sqliteHelper.sqlite_TableExistSYNC<FEC_Items>(App.sqliteDbName);
                        if (lTableExist == true)
                            cn.DropTable<FEC_Items>();

                        cn.CreateTable<FEC_Items>();
                        cn.InsertAll(r.Data, true);

                        cn.Close();
                    }

                    try
                    {
                        //Analytics.TrackEvent("getVerificaTaxIDNumber OK " + " " + r.Data.esitoOK_KO + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                        //    //{ "Utente", IDCompany.ToString() },
                        //    { "Data", DateTime.Now.ToString()},
                        //});
                        //await writeSqlLiteLOG("getVerificaTaxIDNumber OK", "getVerificaTaxIDNumber OK " + " " + r.Data.esitoOK_KO + " " + countryCode + " " + TaxIDNumber, false);
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Console.WriteLine(ex.ToString());
                try
                {
                    //Analytics.TrackEvent("getVerificaTaxIDNumber KO-1 " + " " + ex.Message + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                    ////{ "Utente", IDCompany.ToString() },
                    //{ "Data", DateTime.Now.ToString()},
                    //{ "Eccezione", ex.Message},
                    //});
                    //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-1", "getVerificaTaxIDNumber KO-1 " + " " + ex.Message + " " + countryCode + " " + TaxIDNumber, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                try
                {
                    //Analytics.TrackEvent("getVerificaTaxIDNumber KO-2 " + ex.Message + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                    ////{ "Utente", IDCompany.ToString() },
                    //{ "Data", DateTime.Now.ToString()},
                    //{ "Eccezione", ex.Message},
                    //});
                    //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-2", "getVerificaTaxIDNumber KO-2 " + ex.Message + " " + countryCode + " " + TaxIDNumber, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreGenerico = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreGenerico;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreGenerico, "OK");
            }
            //esecuzione metodo get (fine)
        }

        public static async Task getPaymentTermsBenzinaio(Int32 IDCompany)
        {
            //esecuzione metodo get (inizio)
            string labelMessaggio = AppResources.AlertLabelMessaggio;
            string url = App.baseURL;
            var client = new RestClient(url);
            client.HttpClientFactory = new HttpClientFactoryCustom();

            string req = "";
            RootObject roGlobal = new RootObject();
            req = "FEC/getPaymentTermsBenzinaioSYSTEM";
            var request = new RestRequest(req, Method.GET);

            Parameter p1 = new Parameter();
            p1.Name = "IDCompany";
            p1.Value = IDCompany;
            request.AddParameter(p1);

            request.AddHeader("Authorization-Token", "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230");

            try
            {
                var r = await client.Execute<List<FEC_PaymentTerms>>(request);
                if (r.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        //Analytics.TrackEvent("getVerificaTaxIDNumber KO-1 " + " " + r.StatusCode + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                        //    //{ "Utente", IDCompany.ToString() },
                        //         { "Data", DateTime.Now.ToString()},
                        //});
                        //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-1", "getVerificaTaxIDNumber KO-1 " + " " + r.StatusCode + " " + countryCode + " " + TaxIDNumber, false);
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");
                }
                else
                {
                    //codice http corretto: 200
                    //drop
                    //create
                    //insertall

                    using (var cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                    {
                        var lTableExist = sqliteHelper.sqlite_TableExistSYNC<FEC_PaymentTerms>(App.sqliteDbName);
                        if (lTableExist == true)
                            cn.DropTable<FEC_PaymentTerms>();

                        cn.CreateTable<FEC_PaymentTerms>();
                        cn.InsertAll(r.Data, true);

                        cn.Close();
                    }

                    try
                    {
                        //Analytics.TrackEvent("getVerificaTaxIDNumber OK " + " " + r.Data.esitoOK_KO + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                        //    //{ "Utente", IDCompany.ToString() },
                        //    { "Data", DateTime.Now.ToString()},
                        //});
                        //await writeSqlLiteLOG("getVerificaTaxIDNumber OK", "getVerificaTaxIDNumber OK " + " " + r.Data.esitoOK_KO + " " + countryCode + " " + TaxIDNumber, false);
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Console.WriteLine(ex.ToString());
                try
                {
                    //Analytics.TrackEvent("getVerificaTaxIDNumber KO-1 " + " " + ex.Message + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                    ////{ "Utente", IDCompany.ToString() },
                    //{ "Data", DateTime.Now.ToString()},
                    //{ "Eccezione", ex.Message},
                    //});
                    //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-1", "getVerificaTaxIDNumber KO-1 " + " " + ex.Message + " " + countryCode + " " + TaxIDNumber, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine(ex.ToString());
                    //Analytics.TrackEvent("getVerificaTaxIDNumber KO-2 " + ex.Message + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                    ////{ "Utente", IDCompany.ToString() },
                    //{ "Data", DateTime.Now.ToString()},
                    //{ "Eccezione", ex.Message},
                    //});
                    //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-2", "getVerificaTaxIDNumber KO-2 " + ex.Message + " " + countryCode + " " + TaxIDNumber, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreGenerico = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreGenerico;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreGenerico, "OK");
            }
            //esecuzione metodo get (fine)
        }

        public static async Task getTaxCodesBenzinaio(Int32 IDCompany)
        {
            //esecuzione metodo get (inizio)
            string labelMessaggio = AppResources.AlertLabelMessaggio;
            string url = App.baseURL;
            var client = new RestClient(url);
            client.HttpClientFactory = new HttpClientFactoryCustom();

            string req = "";
            RootObject roGlobal = new RootObject();
            req = "FEC/getTaxCodesBenzinaioSYSTEM";
            var request = new RestRequest(req, Method.GET);

            Parameter p1 = new Parameter();
            p1.Name = "IDCompany";
            p1.Value = IDCompany;
            request.AddParameter(p1);

            request.AddHeader("Authorization-Token", "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230");

            try
            {
                var r = await client.Execute<List<FEC_TaxCode>>(request);
                if (r.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        //Analytics.TrackEvent("getVerificaTaxIDNumber KO-1 " + " " + r.StatusCode + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                        //    //{ "Utente", IDCompany.ToString() },
                        //         { "Data", DateTime.Now.ToString()},
                        //});
                        //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-1", "getVerificaTaxIDNumber KO-1 " + " " + r.StatusCode + " " + countryCode + " " + TaxIDNumber, false);
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");
                }
                else
                {
                    //codice http corretto: 200
                    //drop
                    //create
                    //insertall

                    using (var cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                    {
                        var lTableExist = sqliteHelper.sqlite_TableExistSYNC<FEC_TaxCode>(App.sqliteDbName);
                        if (lTableExist == true)
                            cn.DropTable<FEC_TaxCode>();

                        cn.CreateTable<FEC_TaxCode>();
                        cn.InsertAll(r.Data, true);

                        cn.Close();
                    }

                    try
                    {
                        //Analytics.TrackEvent("getVerificaTaxIDNumber OK " + " " + r.Data.esitoOK_KO + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                        //    //{ "Utente", IDCompany.ToString() },
                        //    { "Data", DateTime.Now.ToString()},
                        //});
                        //await writeSqlLiteLOG("getVerificaTaxIDNumber OK", "getVerificaTaxIDNumber OK " + " " + r.Data.esitoOK_KO + " " + countryCode + " " + TaxIDNumber, false);
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Console.WriteLine(ex.ToString());
                try
                {
                    //Analytics.TrackEvent("getVerificaTaxIDNumber KO-1 " + " " + ex.Message + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                    ////{ "Utente", IDCompany.ToString() },
                    //{ "Data", DateTime.Now.ToString()},
                    //{ "Eccezione", ex.Message},
                    //});
                    //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-1", "getVerificaTaxIDNumber KO-1 " + " " + ex.Message + " " + countryCode + " " + TaxIDNumber, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                try
                {
                    //Analytics.TrackEvent("getVerificaTaxIDNumber KO-2 " + ex.Message + " " + countryCode + " " + TaxIDNumber, new Dictionary<string, string> {
                    ////{ "Utente", IDCompany.ToString() },
                    //{ "Data", DateTime.Now.ToString()},
                    //{ "Eccezione", ex.Message},
                    //});
                    //await writeSqlLiteLOG("getVerificaTaxIDNumber KO-2", "getVerificaTaxIDNumber KO-2 " + ex.Message + " " + countryCode + " " + TaxIDNumber, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreGenerico = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreGenerico;
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreGenerico, "OK");
            }
            //esecuzione metodo get (fine)
        }

        public static async Task writeSqlLiteLOG(string code, string description, Boolean lmandaEmail = false)
        {
            var NomeCliente = "";
            try
            {
                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    cn.CreateTable<xFatturazioneAttivaLogs>();
                    xFatturazioneAttivaLogs l = new xFatturazioneAttivaLogs();
                    l.Code = code;
                    l.Description = description;
                    l.TBCreated = DateTime.Now;
                    cn.Insert(l);
                    //20181213 inizio
                    var lTableExist = await sqliteHelper.sqlite_TableExist<FEC_CustomersSystem>(App.sqliteDbName);
                    if (lTableExist)
                    {
                        var lClientiRegistrati = cn.Table<FEC_CustomersSystem>().ToList();
                        if (lClientiRegistrati.Count > 0)
                        {
                            NomeCliente = lClientiRegistrati[0].CompanyName;
                        }
                    }
                    //20181213 fine
                    cn.Close();
                }
                //20171016 inizio
                if (lmandaEmail)
                {
                    var g = new MailParams();
                    g.Recipients.Add("tid3840@iperbole.bologna.it");//se non si aggiungono domini è l'unica e-mail possibile
                    var oggettoMail = "Self " + DateTime.Now + " - " + App.K_DeviceID + " " + NomeCliente + " " + App.K_currentPlatform.getApplicationVersion();

                    g.Subject = oggettoMail;
                    g.Body = description;
                    g.IsResponseRequested = true;
                    g.IsReadReceiptRequested = false;
                    g.mailgun_Key = "8c5f72cfbf19c0d58c719f2efe981dac-059e099e-c9ac93bb";// "key-c813baf3a313eb22a6d63d1dec8c6ba8";
                    g.mailgun_Domain = "sandbox715b362b3845430a8b68ef415926b761.mailgun.org";// "sandboxee693156474e40f889e7494451b846f3.mailgun.org";
                    g.Attachements.Add(new Attachement { NomeFileSenzaPath = App.sqliteDbName, NomeAllegato = App.sqliteDbName });

                    var msg = await App.K_currentPlatform.sendEmailNEW(g, "", "", Email_Type.MailGun);


                }
                //20171016 fine
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static async Task<ReturnClassGetPost> postRegistraDevice(FEC_DevicesSystem o, Boolean lSilente = true)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            //esecuzione metodo post (inizio)
            string url = App.baseURL;
            var client = new RestClient(url);
            client.HttpClientFactory = new HttpClientFactoryCustom();

            string req = "";
            RootObject roGlobal = new RootObject();
            req = "FEC/postRegistraDeviceSYSTEM";
            var request = new RestRequest(req, Method.POST);

            Parameter p1 = new Parameter();
            p1.Name = "application/json";
            p1.Value = o;// Object; Newtonsoft.Json.JsonConvert.SerializeObject(o);
            p1.Type = ParameterType.RequestBody;

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization-Token",
                                        "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230"); //20171211

            request.AddParameter(p1);

            try
            {
                var r = await client.Execute<ReturnClassGetPost>(request);

                if (r.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        //Analytics.TrackEvent("postRegistraDeviceSYSTEM KO-1 " + " " + r.StatusCode + " " + o.TaxIDNumber, new Dictionary<string, string> {
                        //    { "Utente", IDCompany.ToString() },
                        //         { "Data", DateTime.Now.ToString()},
                        //});
                        await writeSqlLiteLOG("postRegistraDeviceSYSTEM KO-1", "postRegistraDeviceSYSTEM KO-1 " + " " + r.StatusCode, false);
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;

                    var retVal = new ReturnClassGetPost();
                    retVal.esitoOK_KO = "KO";
                    retVal.codeMessageKO = "KO-1";
                    retVal.errorMessageKO = msgErroreHTTP + r.StatusCode;

                    if (lSilente == false)
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");

                    return retVal;
                }
                else
                {
                    //codice http corretto: 200

                    //errori lato server
                    if (r.Data.esitoOK_KO == "KO")
                    {
                        if (r.Data.codeMessageKO == "KO0")
                        {
                            string msgParDrDeviceNulla = AppResources.ChiamateWSPostRegistraDeviceMessaggioParametroDeviceNullo;
                            r.Data.errorMessageKO = msgParDrDeviceNulla;

                            if (lSilente == false)
                                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgParDrDeviceNulla, "OK");
                        }

                        if (r.Data.codeMessageKO == "KO1")
                        {
                            string msgErrSalvataggioDevice = AppResources.ChiamateWSPostRegistraDeviceMessaggioErroreSalvataggioDevice;
                            r.Data.errorMessageKO = msgErrSalvataggioDevice;

                            if (lSilente == false)
                                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErrSalvataggioDevice, "OK");
                        }

                        if (r.Data.codeMessageKO == "KO2")
                        {
                            string msgErrConnessioneDB = AppResources.ChiamateWSPostRegistraDeviceMessaggioErroreConnessioneDB;
                            r.Data.errorMessageKO = msgErrConnessioneDB;

                            if (lSilente == false)
                                await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErrConnessioneDB, "OK");
                        }
                    }

                    if (r.Data.esitoOK_KO == "OK")
                    {
                        //nessun messaggio: in certe logiche (es attivazione) si potrebbe dare un messaggio da localizzare
                    }

                    try
                    {
                        //Analytics.TrackEvent("postRegistraDeviceSYSTEM OK " + " " + r.Data.esitoOK_KO + " " + r.Data.contentOK + " " + o.TaxIDNumber, new Dictionary<string, string> {
                        //    { "Utente", IDCompany.ToString() },
                        //    { "Data", DateTime.Now.ToString()},
                        //});
                        await writeSqlLiteLOG("postRegistraDeviceSYSTEM OK", "postRegistraDeviceSYSTEM OK " + " " + r.Data.esitoOK_KO + " " + r.Data.contentOK, false);

                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }
                }

                return r.Data;
            }

            catch (System.Net.Http.HttpRequestException ex)
            {
                try
                {
                    //Analytics.TrackEvent("postRegistraDeviceSYSTEM KO-1 " + " " + ex.Message + " " + o.TaxIDNumber, new Dictionary<string, string> {
                    //{ "Utente", IDCompany.ToString() },
                    //{ "Data", DateTime.Now.ToString()},
                    //{ "Eccezione", ex.Message},
                    //});
                    await writeSqlLiteLOG("postRegistraDeviceSYSTEM KO-1", "postRegistraDeviceSYSTEM KO-1 " + " " + ex.Message, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;

                var retVal = new ReturnClassGetPost();
                retVal.esitoOK_KO = "KO";
                retVal.codeMessageKO = "KO-1";
                retVal.errorMessageKO = msgErroreHTTP + ex.Message;

                if (lSilente == false)
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");

                return retVal;
            }
            catch (Exception ex)
            {
                try
                {
                    //Analytics.TrackEvent("postRegistraDeviceSYSTEM KO-2 " + ex.Message + " " + o.TaxIDNumber, new Dictionary<string, string> {
                    //{ "Utente", IDCompany.ToString() },
                    //{ "Data", DateTime.Now.ToString()},
                    //{ "Eccezione", ex.Message},
                    //});
                    await writeSqlLiteLOG("postRegistraDeviceSYSTEM KO-2", "postRegistraDeviceSYSTEM KO-2 " + ex.Message, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreGenerico = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreGenerico;

                var retVal = new ReturnClassGetPost();
                retVal.esitoOK_KO = "KO";
                retVal.codeMessageKO = "KO-2";
                retVal.errorMessageKO = msgErroreGenerico + ": " + ex.StackTrace;

                if (lSilente == false)
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, retVal.errorMessageKO, "OK");

                return retVal;
            }

            //esecuzione metodo post (fine)  
        }

        public static async Task<ReturnClassCheckVersione> getCheckVersione(string DeviceID, Int32 versioneCorrente, Boolean lSilente = false)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;
            //esecuzione metodo get (inizio)
            string url = App.baseURL;
            var client = new RestClient(url);
            client.HttpClientFactory = new HttpClientFactoryCustom();

            string req = "";
            RootObject roGlobal = new RootObject();
            req = "FEC/getCheckVersioneSYSTEM";
            var request = new RestRequest(req, Method.GET);

            Parameter p1 = new Parameter();
            p1.Name = "DeviceID";
            p1.Value = DeviceID;
            request.AddParameter(p1);

            Parameter p2 = new Parameter();
            p2.Name = "versioneCorrente";
            p2.Value = versioneCorrente;
            request.AddParameter(p2);

            request.AddHeader("Authorization-Token", "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230");

            try
            {
                var r = await client.Execute<ReturnClassCheckVersione>(request);

                if (r.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        //Analytics.TrackEvent("getCheckVersioneSYSTEM KO-1 " + " " + r.StatusCode + " " + DeviceID + " " + versioneCorrente, new Dictionary<string, string> {
                        //    { "Utente", IDCompany.ToString() },
                        //         { "Data", DateTime.Now.ToString()},
                        //});
                        await writeSqlLiteLOG("getCheckVersioneSYSTEM KO-1", "getCheckVersioneSYSTEM KO-1 " + " " + r.StatusCode + " " + DeviceID + " " + versioneCorrente, false);
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                    string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;

                    var retVal = new ReturnClassCheckVersione();
                    retVal.esitoOK_KO = "KO";
                    retVal.codeMessageKO = "KO-1";
                    retVal.errorMessageKO = msgErroreHTTP + r.StatusCode;

                    if (lSilente == false)
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");

                    return retVal;
                }
                else
                {
                    //codice http corretto: 200

                    if (r.Data.esitoOK_KO == "KO")
                    {
                        if (r.Data.codeMessageKO == "KO0")
                        {
                            string msgDeviceNonAbilitata = AppResources.ChiamateWSGetCheckVersioneMessaggioDeviceNonAbilitata;
                            r.Data.errorMessageKO = msgDeviceNonAbilitata;

                            //if (lSilente == false)
                            //await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgDeviceNonAbilitata, "OK");
                        }

                        if (r.Data.codeMessageKO == "KO1")
                        {
                            string msgVersInfAConsentita = AppResources.ChiamateWSGetCheckVersioneMessaggioVersInfACons + " " + r.Data.minVersionRequired;
                            r.Data.errorMessageKO = msgVersInfAConsentita;

                            //if (lSilente == false)
                            //await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgVersInfAConsentita, "OK");
                        }

                        if (r.Data.codeMessageKO == "KO2")
                        {
                            string msgVersSupAConsentita = AppResources.ChiamateWSGetCheckVersioneMessaggioVersSupACons + " " + r.Data.maxVersionRequired;
                            r.Data.errorMessageKO = msgVersSupAConsentita;

                            //if (lSilente == false)
                            //await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgVersSupAConsentita, "OK");
                        }
                    }

                    if (r.Data.esitoOK_KO == "OK")
                    {
                        //await Application.Current.MainPage.DisplayAlert(labelMessaggio, r.Data.contentOK, "OK");
                    }

                    try
                    {
                        //Analytics.TrackEvent("getCheckVersioneSYSTEM OK " + " " + r.Data.esitoOK_KO + " " + r.Data.contentOK + " " + DeviceID + " " + versioneCorrente, new Dictionary<string, string> {
                        //    { "Utente", IDCompany.ToString() },
                        //    { "Data", DateTime.Now.ToString()},
                        //});
                        await writeSqlLiteLOG("getCheckVersioneSYSTEM OK", "getCheckVersioneSYSTEM OK " + " " + r.Data.esitoOK_KO + " " + r.Data.contentOK + " " + DeviceID + " " + versioneCorrente, false);

                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.ToString());
                        //trap
                    }

                }

                return r.Data;
            }

            catch (System.Net.Http.HttpRequestException ex)
            {
                try
                {
                    //Analytics.TrackEvent("getCheckVersioneSYSTEM KO-1 " + " " + ex.Message + " " + DeviceID + " " + versioneCorrente, new Dictionary<string, string> {
                    //    { "Utente", IDCompany.ToString() },
                    //    { "Data", DateTime.Now.ToString()},
                    //    { "Eccezione", ex.Message},
                    //});
                    await writeSqlLiteLOG("getCheckVersioneSYSTEM KO-1", "getCheckVersioneSYSTEM KO-1 " + " " + ex.Message + " " + DeviceID + " " + versioneCorrente, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreHTTP = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreHTTP;

                var retVal = new ReturnClassCheckVersione();
                retVal.esitoOK_KO = "KO";
                retVal.codeMessageKO = "KO-1";
                retVal.errorMessageKO = msgErroreHTTP + ex.Message;

                if (lSilente == false)
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgErroreHTTP, "OK");

                return retVal;
            }
            catch (Exception ex)
            {
                try
                {
                    //Analytics.TrackEvent("getCheckVersioneSYSTEM KO-2 " + ex.Message + " " + DeviceID + " " + versioneCorrente, new Dictionary<string, string> {
                    //    { "Utente", IDCompany.ToString() },
                    //    { "Data", DateTime.Now.ToString()},
                    //    { "Eccezione", ex.Message},
                    //});
                    await writeSqlLiteLOG("getCheckVersioneSYSTEM KO-2", "getCheckVersioneSYSTEM KO-2 " + ex.Message + " " + DeviceID + " " + versioneCorrente, false);

                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    //trap
                }

                string msgErroreGenerico = AppResources.FormWSChiamateMetodoDevEnabledMsgErroreGenerico;

                var retVal = new ReturnClassCheckVersione();
                retVal.esitoOK_KO = "KO";
                retVal.codeMessageKO = "KO-2";
                retVal.errorMessageKO = msgErroreGenerico + ": " + ex.StackTrace;

                if (lSilente == false)
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, retVal.errorMessageKO, "OK");

                return retVal;
            }

            //esecuzione metodo get (fine)
        }


        //20181129 inizio
        public static void Delete_and_PerformVacuum(Boolean lOnlyDelete = false)
        {


            var dep = DependencyService.Get<platformSpecific>();
            String folder = dep.getLocalDatabasePath();
            String nomeFile = App.sqliteDbName;
            String fullPath = System.IO.Path.Combine(folder, nomeFile);

            var d = dep.FileDate(fullPath);

            using (var cn = sqliteHelper.creaDataBaseORGetConnectionSYNC(App.sqliteDbName))
            {
                //var msg = "";
                try
                {
                    var dataLimiteCancellazione = DateTime.Today.AddYears(-1);
                    cn.BeginTransaction();
                    //var n = cn.Table<XPK_Statements>().Delete(X => X.StatementDate <= DateTime.Today.AddMonths(-3) && X.RowState == BaseModel.RowStateEnum.Unchanged);
                    var n = cn.Table<FEC_InvoiceRequests>().Delete(X => X.Receiptdate <= dataLimiteCancellazione && X.RowState == BaseModel.RowStateEnum.Unchanged);

                    cn.Commit();

                    if (lOnlyDelete == false)
                    {
                        SQLiteCommand command = cn.CreateCommand("VACUUM;", new object[] { });
                        command.CommandText = "VACUUM;";
                        command.ExecuteNonQuery();
                    }
                    cn.Close();
                    try
                    {
                        //HockeyApp.MetricsManager.TrackEvent(App.GetDatelog() + "VACUUM OK " + msg +
                        //" " + App.K_Employee1Matricola + " " + App.K_CompanyCode);
                    }
                    catch (Exception ex)
                    {
                        //trap
                        Console.WriteLine(ex.ToString());
                    }
                }
                catch (Exception e1)
                {
                    try
                    {
                        //HockeyApp.MetricsManager.TrackEvent(App.GetDatelog() + "VACUUM non terminato correttamente " + msg +
                        //" " + App.K_Employee1Matricola + " " + App.K_CompanyCode);
                        Console.WriteLine(e1.ToString());
                    }
                    catch (Exception ex)
                    {
                        //trap
                        Console.WriteLine(ex.ToString());
                    };
                }
            }
        }
        //20181129 fine
    }
}
