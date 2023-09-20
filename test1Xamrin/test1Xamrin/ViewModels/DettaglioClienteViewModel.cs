using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using test1Xamrin;
using test1Xamrin.Resx;
using Xamarin.Forms;
using xUtilityPCL;

namespace test1Xamrin.ViewModels
{
    public class DettaglioClienteViewModel
    {
        private FEC_CustomersSystem drCustomer { get; set; }
        private FEC_CustomersSystem drCustomerClone { get; set; }
        private ObservableCollection<FEC_CustomersSystem> lista { get; set; }
        private ObservableCollection<FEC_CustomersSystem> listaClone { get; set; }
        public Boolean lChiamataDaDisappearing { get; set; } = false;
        public string ultimobtnPremutoSalva_Annulla { get; set; } = "";
        private Boolean lClickedSalva = true;
        public Action<FEC_CustomersSystem> Form_OnSave { get; set; }

        INavigation navigation { get; set; }
        public DettaglioClienteViewModel(FEC_CustomersSystem _drCustomer,
                                         ObservableCollection<FEC_CustomersSystem> _lista, INavigation _navigation,
                                         ObservableCollection<FEC_CustomersSystem> _listaClone
                                        )
        {
            this.drCustomer = _drCustomer;
            this.lista = _lista;
            this.listaClone = _listaClone;
            this.drCustomerClone = this.drCustomer.Clone() as FEC_CustomersSystem;
            datarow2Validatable();

            this.navigation = _navigation;
            this.drCustomer.cmdbtnSalva = new Command(async () => await SalvaCliente());
            this.drCustomer.cmdbtnAnnulla = new Command(async () => await AnnullaCliente());

            Task.Run(async () =>
            {
                await AddValidations();
            });

            //AddValidations();
        }

        void datarow2Validatable()
        {
            //x ogni validatableobject: in entrata
            drCustomer.TaxIDNumber_VAL.Value = drCustomer.TaxIDNumber;
            drCustomer.CompanyName_VAL.Value = drCustomer.CompanyName;
            drCustomer.Address_VAL.Value = drCustomer.Address;
            drCustomer.Telephone_VAL.Value = drCustomer.Telephone;
            drCustomer.City_VAL.Value = drCustomer.City;
            drCustomer.ZipCode_VAL.Value = drCustomer.ZipCode;
            //drCustomer.County_VAL.Value = drCustomer.County; //20180809

            if (string.IsNullOrEmpty(drCustomer.County))
                drCustomer.County2_VAL.Value = AppResources.FormClientiTestoSelezionaProvincia;
            else
                drCustomer.County2_VAL.Value = drCustomer.County;

            drCustomer.Country_VAL.Value = drCustomer.Country;
            //drCustomer.StateCode_VAL.Value = drCustomer.StateCode; //20180809

            if (string.IsNullOrEmpty(drCustomer.StateCode))
                drCustomer.StateCode2_VAL.Value = AppResources.FormClientiTestoSelezionaStato;
            else
                drCustomer.StateCode2_VAL.Value = drCustomer.StateCode;

            drCustomer.FiscalCode_VAL.Value = drCustomer.FiscalCode;
            drCustomer.Pec_VAL.Value = drCustomer.Pec;
            drCustomer.Email_VAL.Value = drCustomer.Email;
            drCustomer.ContactName_VAL.Value = drCustomer.ContactName;
            drCustomer.ContactTelephone_VAL.Value = drCustomer.ContactTelephone;
            drCustomer.ContactEmail_VAL.Value = drCustomer.ContactEmail;
            drCustomer.CIG_VAL.Value = drCustomer.CIG;
            drCustomer.CUP_VAL.Value = drCustomer.CUP;
            drCustomer.SID_VAL.Value = drCustomer.SID;
            drCustomer.TaxCodeDefault_VAL.Value = drCustomer.TaxCodeDefault;

            if (drCustomer.PA.HasValue == false)
                drCustomer.PA = false;

            drCustomer.NumeroOrdine_VAL.Value = drCustomer.NumeroOrdine;
            drCustomer.NumeroContratto_VAL.Value = drCustomer.NumeroContratto;
            drCustomer.NumeroConvenzione_VAL.Value = drCustomer.NumeroConvenzione;
            drCustomer.NumeroRicezione_VAL.Value = drCustomer.NumeroRicezione;

            if (drCustomer.DataOrdine.HasValue)
                drCustomer.DataOrdine_VAL.Value = drCustomer.DataOrdine.Value.ToString(Utility.getDateFormat());//20180630"dd/MM/yyyy");
            else
                drCustomer.DataOrdine_VAL.Value = null;

            if (drCustomer.DataContratto.HasValue)
                drCustomer.DataContratto_VAL.Value = drCustomer.DataContratto.Value.ToString(Utility.getDateFormat());//20180630 "dd/MM/yyyy");
            else
                drCustomer.DataContratto_VAL.Value = null;

            if (drCustomer.DataConvenzione.HasValue)
                drCustomer.DataConvenzione_VAL.Value = drCustomer.DataConvenzione.Value.ToString(Utility.getDateFormat());//20180630"dd/MM/yyyy");
            else
                drCustomer.DataConvenzione_VAL.Value = null;

            if (drCustomer.DatiRicezione.HasValue)
                drCustomer.DataRicezione_VAL.Value = drCustomer.DatiRicezione.Value.ToString(Utility.getDateFormat());//20180630"dd/MM/yyyy");
            else
                drCustomer.DataRicezione_VAL.Value = null;
        }

        void Validatable2dataRow()
        {
            //x ogni validatableobject: in uscita
            if (string.IsNullOrEmpty(drCustomer.TaxIDNumber_VAL.Value))
                drCustomer.TaxIDNumber = null;
            else
                drCustomer.TaxIDNumber = drCustomer.TaxIDNumber_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.CompanyName_VAL.Value))
                drCustomer.CompanyName = null;
            else
                drCustomer.CompanyName = drCustomer.CompanyName_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.Address_VAL.Value))
                drCustomer.Address = null;
            else
                drCustomer.Address = drCustomer.Address_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.Telephone_VAL.Value))
                drCustomer.Telephone = null;
            else
                drCustomer.Telephone = drCustomer.Telephone_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.City_VAL.Value))
                drCustomer.City = null;
            else
                drCustomer.City = drCustomer.City_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.ZipCode_VAL.Value))
                drCustomer.ZipCode = null;
            else
                drCustomer.ZipCode = drCustomer.ZipCode_VAL.Value.ToString();

            //20180809 commentato sotto
            //if (string.IsNullOrEmpty(drCustomer.County_VAL.Value))
            //    drCustomer.County = null;
            //else
            //drCustomer.County = drCustomer.County_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.County2_VAL.Value) || drCustomer.County2_VAL.Value == AppResources.FormClientiTestoSelezionaProvincia)
                drCustomer.County = null;
            else
                drCustomer.County = drCustomer.County2_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.Country_VAL.Value))
                drCustomer.Country = null;
            else
                drCustomer.Country = drCustomer.Country_VAL.Value.ToString();

            //20180809 commentato sotto
            //if (string.IsNullOrEmpty(drCustomer.StateCode_VAL.Value))
            //    drCustomer.StateCode = null;
            //else
            //drCustomer.StateCode = drCustomer.StateCode_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.StateCode2_VAL.Value) || drCustomer.StateCode2_VAL.Value == AppResources.FormClientiTestoSelezionaStato)
                drCustomer.StateCode = null;
            else
                drCustomer.StateCode = drCustomer.StateCode2_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.FiscalCode_VAL.Value))
                drCustomer.FiscalCode = null;
            else
                drCustomer.FiscalCode = drCustomer.FiscalCode_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.Pec_VAL.Value))
                drCustomer.Pec = null;
            else
                drCustomer.Pec = drCustomer.Pec_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.Email_VAL.Value))
                drCustomer.Email = null;
            else
                drCustomer.Email = drCustomer.Email_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.ContactName_VAL.Value))
                drCustomer.ContactName = null;
            else
                drCustomer.ContactName = drCustomer.ContactName_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.ContactTelephone_VAL.Value))
                drCustomer.ContactTelephone = null;
            else
                drCustomer.ContactTelephone = drCustomer.ContactTelephone_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.ContactEmail_VAL.Value))
                drCustomer.ContactEmail = null;
            else
                drCustomer.ContactEmail = drCustomer.ContactEmail_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.CIG_VAL.Value))
                drCustomer.CIG = null;
            else
                drCustomer.CIG = drCustomer.CIG_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.CUP_VAL.Value))
                drCustomer.CUP = null;
            else
                drCustomer.CUP = drCustomer.CUP_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.SID_VAL.Value))
                drCustomer.SID = null;
            else
                drCustomer.SID = drCustomer.SID_VAL.Value.ToString();

            //commentato perchè il campo è gestito da uno switch con converter myFakeConverterDichiarazioneIntenti
            //perciò il valore è direttamente messo nel campo della datarow e non si usa il validatable
            //if (string.IsNullOrEmpty(drCustomer.TaxCodeDefault_VAL.Value))
            //    drCustomer.TaxCodeDefault = null;
            //else
            //drCustomer.TaxCodeDefault = drCustomer.TaxCodeDefault_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.NumeroOrdine_VAL.Value))
                drCustomer.NumeroOrdine = null;
            else
                drCustomer.NumeroOrdine = drCustomer.NumeroOrdine_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.NumeroContratto_VAL.Value))
                drCustomer.NumeroContratto = null;
            else
                drCustomer.NumeroContratto = drCustomer.NumeroContratto_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.NumeroConvenzione_VAL.Value))
                drCustomer.NumeroConvenzione = null;
            else
                drCustomer.NumeroConvenzione = drCustomer.NumeroConvenzione_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.NumeroRicezione_VAL.Value))
                drCustomer.NumeroRicezione = null;
            else
                drCustomer.NumeroRicezione = drCustomer.NumeroRicezione_VAL.Value.ToString();

            if (string.IsNullOrEmpty(drCustomer.DataOrdine_VAL.Value))
                drCustomer.DataOrdine = null;
            else
                drCustomer.DataOrdine = Convert.ToDateTime(drCustomer.DataOrdine_VAL.Value);

            if (string.IsNullOrEmpty(drCustomer.DataContratto_VAL.Value))
                drCustomer.DataContratto = null;
            else
                drCustomer.DataContratto = Convert.ToDateTime(drCustomer.DataContratto_VAL.Value);

            if (string.IsNullOrEmpty(drCustomer.DataConvenzione_VAL.Value))
                drCustomer.DataConvenzione = null;
            else
                drCustomer.DataConvenzione = Convert.ToDateTime(drCustomer.DataConvenzione_VAL.Value);

            if (string.IsNullOrEmpty(drCustomer.DataRicezione_VAL.Value))
                drCustomer.DatiRicezione = null;
            else
                drCustomer.DatiRicezione = Convert.ToDateTime(drCustomer.DataRicezione_VAL.Value);
        }

        public FEC_CustomersSystem GetdrCorrente()
        {
            return this.drCustomer;
        }

        async private Task SalvaCliente()
        {
            if (lClickedSalva)
            {
                lClickedSalva = false;
                string labelMessaggio = AppResources.AlertLabelMessaggio;

                await Task.Delay(1);
                var lValido = this.Validate();
                if (lValido == false)
                {
                    var msg = AppResources.FormDettClienteMessaggioPresentiCampiInErrore;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msg, "OK");
                    lClickedSalva = true;
                    return;
                }

                var myHud = new ChatBotLoadIndicator(); //20181125
                myHud.SettaLabel(AppResources.NuovoHudColoratoMessaggioSalvataggio); //20181125
                //myHud.SettaTextColor(Color.FromHex("ffffff")); //20181125
                //myHud.SettaBackgroundColor(Color.FromHex("1b9ed6")); //20181125

                DependencyService.Get<ILodingPageService>().InitLoadingPage(myHud); //20181125
                DependencyService.Get<ILodingPageService>().ShowLoadingPage(); //20181125

                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    if (drCustomer.StatoFormRispettoSqlite_IMC == "I" || drCustomer.TaxIDNumber_VAL.Value != drCustomer.TaxIDNumber)
                    {
                        var drCustomerDB = cn.Table<FEC_CustomersSystem>().FirstOrDefault(y => y.TaxIDNumber == this.drCustomer.TaxIDNumber_VAL.Value);
                        cn.Close();

                        if (drCustomerDB != null)
                        {
                            //partita iva già presente in locale
                            var msg = AppResources.FormDettClienteMsgPartitaIvaGiaPres;
                            DependencyService.Get<ILodingPageService>().HideLoadingPage(); //20181125
                            await Application.Current.MainPage.DisplayAlert(labelMessaggio, msg, "OK");
                            lClickedSalva = true;
                            return;
                        }
                    }

                    if (drCustomer.StatoFormRispettoSqlite_IMC == "M" && drCustomer.TaxIDNumber_VAL.Value != drCustomer.TaxIDNumber)
                    {
                        //modifica partita iva
                        //impossibile modificare partita iva perchè esistono fatture
                        var lClientSuServer = await WSChiamateEF.getAlmenoUnaFatturaByTaxIDNumber(drCustomer.TaxIDNumber);
                        if (lClientSuServer == "SI")
                        {
                            var msgNonModPIVAEsistFatt = AppResources.FormDettClienteMsgImpossModPIVAEsistonoRichFatt;
                            DependencyService.Get<ILodingPageService>().HideLoadingPage(); //20181125
                            await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgNonModPIVAEsistFatt, "OK");
                            lClickedSalva = true;
                            return;
                        }
                        if (lClientSuServer == "KO")
                        {
                            //Errore di connessione al server remoto. Impossibile determinare se il cliente esiste già sul server 
                            var msgClienteServer = AppResources.FormDettClienteMessaggioErroreClienteSuServer;
                            DependencyService.Get<ILodingPageService>().HideLoadingPage(); //20181125
                            await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgClienteServer, "OK");
                            lClickedSalva = true;
                            return;
                        }
                    }
                }

                Validatable2dataRow();
                //salvataggio su sqllite

                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    ClientiViewModel.CreaIndirizzoCompleto(drCustomer);
                    drCustomer.RecapitoUnbound = AppResources.CellaClienteParolaRecapito + ": " + FECUtilita.getRecapito(drCustomer);

                    if (drCustomer.StatoFormRispettoSqlite_IMC == "I")
                    {
                        cn.Insert(drCustomer);
                        this.lista.Add(drCustomer);
                        this.listaClone.Add(drCustomer);
                    }

                    if (drCustomer.StatoFormRispettoSqlite_IMC == "M")
                        cn.Update(drCustomer);

                    cn.Close();
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
                    DependencyService.Get<ILodingPageService>().HideLoadingPage(); //20181125
                    return;
                }
                else
                {
                    DependencyService.Get<ILodingPageService>().HideLoadingPage(); //20181125
                }

                if (Form_OnSave != null)
                    Form_OnSave(drCustomer);

                this.ultimobtnPremutoSalva_Annulla = "Salva";
                if (this.lChiamataDaDisappearing == false)
                    await navigation.PopAsync(true);
                //await navigation.PopModalAsync(true);

                lClickedSalva = true;
            }
            else
            {
               // var aa = 1;
            }
        }

        async public Task AnnullaCliente()
        {
            await Task.Delay(1);

            drCustomer.Codiva1 = drCustomerClone.Codiva1;
            drCustomer.Codiva1Description = drCustomerClone.Codiva1Description;
            drCustomer.Codiva2 = drCustomerClone.Codiva2;
            drCustomer.Codiva2Description = drCustomerClone.Codiva2Description;
            drCustomer.Codiva3 = drCustomerClone.Codiva3;
            drCustomer.Codiva3Description = drCustomerClone.Codiva3Description;
            drCustomer.Codiva4 = drCustomerClone.Codiva4;
            drCustomer.Codiva4Description = drCustomerClone.Codiva4Description;
            drCustomer.CodivaN1 = drCustomerClone.CodivaN1;
            drCustomer.CodivaN1Description = drCustomerClone.CodivaN1Description;
            drCustomer.CodivaN2 = drCustomerClone.CodivaN2;
            drCustomer.CodivaN2Description = drCustomerClone.CodivaN2Description;
            drCustomer.CodivaN3 = drCustomerClone.CodivaN3;
            drCustomer.CodivaN3Description = drCustomerClone.CodivaN3Description;
            drCustomer.CodivaN4 = drCustomerClone.CodivaN4;
            drCustomer.CodivaN4Description = drCustomerClone.CodivaN4Description;
            drCustomer.CodivaN5 = drCustomerClone.CodivaN5;
            drCustomer.CodivaN5Description = drCustomerClone.CodivaN5Description;
            drCustomer.CodivaN6 = drCustomerClone.CodivaN6;
            drCustomer.CodivaN6Description = drCustomerClone.CodivaN6Description;
            drCustomer.CodivaN7 = drCustomerClone.CodivaN7;
            drCustomer.CodivaN7Description = drCustomerClone.CodivaN7Description;

            drCustomer.PA = drCustomerClone.PA;
            drCustomer.IsPrivate = drCustomerClone.IsPrivate;
            drCustomer.IsLiableForWithholdingTaxPercentageCompanyType2 = drCustomerClone.IsLiableForWithholdingTaxPercentageCompanyType2;
            drCustomer.isSplitPayment = drCustomerClone.isSplitPayment;
            drCustomer.StatoFormRispettoSqlite_IMC = "";
            //drCustomer.RejectChanges(this.drCustomerClone);
            drCustomer.SID_VAL.IsValid = true;
            drCustomer.TaxIDNumber_VAL.IsValid = true;
            drCustomer.CompanyName_VAL.IsValid = true;
            drCustomer.Address_VAL.IsValid = true;
            drCustomer.Telephone_VAL.IsValid = true;
            drCustomer.City_VAL.IsValid = true;
            drCustomer.ZipCode_VAL.IsValid = true;
            //drCustomer.County_VAL.IsValid = true; //20180809
            drCustomer.County2_VAL.IsValid = true;
            drCustomer.Country_VAL.IsValid = true;
            //drCustomer.StateCode_VAL.IsValid = true; //20180809
            drCustomer.StateCode2_VAL.IsValid = true;
            drCustomer.FiscalCode_VAL.IsValid = true;
            drCustomer.Pec_VAL.IsValid = true;
            drCustomer.Email_VAL.IsValid = true;
            drCustomer.ContactName_VAL.IsValid = true;
            drCustomer.ContactTelephone_VAL.IsValid = true;
            drCustomer.ContactEmail_VAL.IsValid = true;
            drCustomer.CIG_VAL.IsValid = true;
            drCustomer.CUP_VAL.IsValid = true;
            drCustomer.TaxCodeDefault_VAL.IsValid = true;
            drCustomer.NumeroOrdine_VAL.IsValid = true;
            drCustomer.NumeroContratto_VAL.IsValid = true;
            drCustomer.NumeroConvenzione_VAL.IsValid = true;
            drCustomer.NumeroRicezione_VAL.IsValid = true;
            drCustomer.DataOrdine_VAL.IsValid = true;
            drCustomer.DataContratto_VAL.IsValid = true;
            drCustomer.DataConvenzione_VAL.IsValid = true;
            drCustomer.DataRicezione_VAL.IsValid = true;

            this.ultimobtnPremutoSalva_Annulla = "Annulla";
            if (this.lChiamataDaDisappearing == false)
                await navigation.PopAsync(true);
            //await navigation.PopModalAsync(true);
        }

        #region "Validations"
        private async Task AddValidations()
        {
            var msgValidazione = "";

            //x ogni campo obbligatorio o con validazioni
            msgValidazione = AppResources.FormDettClienteValidazioniPIVA1;
            this.drCustomer.TaxIDNumber_VAL.Validations.Clear();
            this.drCustomer.TaxIDNumber_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = msgValidazione });

            if (drCustomer.IsPrivate)
                msgValidazione = AppResources.FormDettClienteValidazioniPIVA2Vers16;
            else
                msgValidazione = AppResources.FormDettClienteValidazioniPIVA2;
            //this.drCustomer.TaxIDNumber_VAL.Validations.Clear();
            this.drCustomer.TaxIDNumber_VAL.Validations.Add(new TaxIDNumberRuleLunghezza<string>
            { ValidationMessage = msgValidazione, SoloGlobale = true, drCorrente = this.drCustomer });

            msgValidazione = AppResources.FormDettClienteValidazioniPIVA3;
            //this.drCustomer.TaxIDNumber_VAL.Validations.Clear();
            this.drCustomer.TaxIDNumber_VAL.Validations.Add(new TaxIDNumberRuleSoloNumeri<string>
            { ValidationMessage = msgValidazione });

            this.drCustomer.ValidateTaxIDNumberCommand = new Command(() => ValidateTaxIDNumber(false));

            msgValidazione = AppResources.FormDettClienteValidazioniRagSoc;
            this.drCustomer.CompanyName_VAL.Validations.Clear();
            this.drCustomer.CompanyName_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = msgValidazione });

            this.drCustomer.ValidateCompanyNameCommand = new Command(() => ValidateCompanyName(false));

            msgValidazione = AppResources.FormDettClienteValidazioniIndirizzo;
            this.drCustomer.Address_VAL.Validations.Clear();
            this.drCustomer.Address_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = msgValidazione });

            this.drCustomer.ValidateAddressCommand = new Command(() => ValidateAddress(false));

            this.drCustomer.ValidateTelephoneCommand = new Command(() => ValidateTelephone(false));

            msgValidazione = AppResources.FormDettClienteValidazioniComune;
            this.drCustomer.City_VAL.Validations.Clear();
            this.drCustomer.City_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = msgValidazione });

            this.drCustomer.ValidateCityCommand = new Command(() => ValidateCity(false));

            msgValidazione = AppResources.FormDettClienteValidazioniCAP;
            this.drCustomer.ZipCode_VAL.Validations.Clear();
            this.drCustomer.ZipCode_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = msgValidazione });

            this.drCustomer.ValidateZipCodeCommand = new Command(() => ValidateZipCode(false));

            //this.drCustomer.ValidateCountyCommand = new Command(() => ValidateCounty(false)); //20180809

            //20190110 inizio
            msgValidazione = AppResources.FormDettClienteValidazioniProv;
            this.drCustomer.County2_VAL.Validations.Clear();
            this.drCustomer.County2_VAL.Validations.Add(new CountyRuleObbligatoria<string>
            { ValidationMessage = msgValidazione, SoloGlobale = true });
            //20190110 fine

            this.drCustomer.ValidateCounty2Command = new Command(() => ValidateCounty2(false));

            msgValidazione = AppResources.FormDettClienteValidazioniStato;
            this.drCustomer.Country_VAL.Validations.Clear();
            this.drCustomer.Country_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = msgValidazione });

            this.drCustomer.ValidateCountryCommand = new Command(() => ValidateCountry(false));

            //20180809 commentato sotto
            //msgValidazione = AppResources.FormDettClienteValidazioniCodiceStato;
            //this.drCustomer.StateCode_VAL.Validations.Clear();
            //this.drCustomer.StateCode_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            //{ ValidationMessage = msgValidazione });

            //this.drCustomer.ValidateStateCodeCommand = new Command(() => ValidateStateCode(false));

            msgValidazione = AppResources.FormDettClienteValidazioniCodiceStato;
            this.drCustomer.StateCode2_VAL.Validations.Clear();
            this.drCustomer.StateCode2_VAL.Validations.Add(new StateCodeRuleObbligatorio<string>
            { ValidationMessage = msgValidazione, SoloGlobale = true });

            this.drCustomer.ValidateStateCode2Command = new Command(() => ValidateStateCode2(false));

            msgValidazione = AppResources.FormDettClienteValidazioniFiscalCode1;
            this.drCustomer.FiscalCode_VAL.Validations.Clear();
            this.drCustomer.FiscalCode_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = msgValidazione });

            msgValidazione = AppResources.FormDettClienteValidazioniFiscalCode2;
            //this.drCustomer.FiscalCode_VAL.Validations.Clear();
            this.drCustomer.FiscalCode_VAL.Validations.Add(new FiscalCodeRuleLunghezza<string>
            { ValidationMessage = msgValidazione, SoloGlobale = true });

            msgValidazione = AppResources.FormDettClienteValidazioniFiscalCode3;
            //this.drCustomer.FiscalCode_VAL.Validations.Clear();
            this.drCustomer.FiscalCode_VAL.Validations.Add(new FiscalCodeRuleCaratteriNumerici<string>
            { ValidationMessage = msgValidazione });

            this.drCustomer.ValidateFiscalCodeCommand = new Command(() => ValidateFiscalCode(false));

            //msgValidazione = AppResources.FormDettClienteValidazioniPec;
            //this.drCustomer.Pec_VAL.Validations.Clear();
            //this.drCustomer.Pec_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            //{ ValidationMessage = msgValidazione });

            msgValidazione = AppResources.FormDettClienteValidazioniPec;
            this.drCustomer.Pec_VAL.Validations.Clear();
            this.drCustomer.Pec_VAL.Validations.Add(new PecRuleObbligatoria<string>
            { ValidationMessage = msgValidazione, drCorrente = this.drCustomer });

            msgValidazione = AppResources.FormDettClienteValidazioniPec2;
            //this.drCustomer.Pec_VAL.Validations.Clear();
            this.drCustomer.Pec_VAL.Validations.Add(new EmailPecValidazione<string>
            { ValidationMessage = msgValidazione });

            this.drCustomer.ValidatePecCommand = new Command(() => ValidatePec(false));

            msgValidazione = AppResources.FormDettClienteValidazioniEmail;
            this.drCustomer.Email_VAL.Validations.Clear();
            this.drCustomer.Email_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = msgValidazione });

            msgValidazione = AppResources.FormDettClienteValidazioniEmail2;
            //this.drCustomer.Email_VAL.Validations.Clear();
            this.drCustomer.Email_VAL.Validations.Add(new EmailPecValidazione<string>
            { ValidationMessage = msgValidazione });

            this.drCustomer.ValidateEmailCommand = new Command(() => ValidateEmail(false));
            this.drCustomer.ValidateContactNameCommand = new Command(() => ValidateContactName(false));
            this.drCustomer.ValidateContactTelephoneCommand = new Command(() => ValidateContactTelephone(false));
            this.drCustomer.ValidateContactEmailCommand = new Command(() => ValidateContactEmail(false));

            msgValidazione = AppResources.FormDettClienteValidazioniCIG;
            this.drCustomer.CIG_VAL.Validations.Clear();
            this.drCustomer.CIG_VAL.Validations.Add(new CIGRuleLunghezza<string>
            { ValidationMessage = msgValidazione, SoloGlobale = true });

            this.drCustomer.ValidateCIGCommand = new Command(() => ValidateCIG(false));

            msgValidazione = AppResources.FormDettClienteValidazioniCUP;
            this.drCustomer.CUP_VAL.Validations.Clear();
            this.drCustomer.CUP_VAL.Validations.Add(new CUPRuleLunghezza<string>
            { ValidationMessage = msgValidazione, SoloGlobale = true });

            this.drCustomer.ValidateCUPCommand = new Command(() => ValidateCUP(false));

            //msgValidazione = AppResources.FormDettClienteValidazioniSID1;
            //this.drCustomer.SID_VAL.Validations.Add(new IsNotNullOrEmptyRule<string>
            //{ ValidationMessage = msgValidazione });

            msgValidazione = AppResources.FormDettClienteValidazioniSID1;
            this.drCustomer.SID_VAL.Validations.Clear();
            this.drCustomer.SID_VAL.Validations.Add(new SIDRuleObbligatorio<string>
            { ValidationMessage = msgValidazione, drCorrente = this.drCustomer });

            //msgValidazione = AppResources.FormDettClienteValidazioniSID2;
            //this.drCustomer.SID_VAL.Validations.Add(new SIDRuleLunghezza<string>
            //{ ValidationMessage = msgValidazione, SoloGlobale = true, drCorrente = this.drCustomer });

            this.drCustomer.ValidateSIDCommand = new Command(() => ValidateSID(false));
            this.drCustomer.ValidateTaxCodeDefaultCommand = new Command(() => ValidateTaxCodeDefault(false));
            this.drCustomer.ValidateNumeroOrdineCommand = new Command(() => ValidateNumeroOrdine(false));
            this.drCustomer.ValidateNumeroContrattoCommand = new Command(() => ValidateNumeroContratto(false));
            this.drCustomer.ValidateNumeroConvenzioneCommand = new Command(() => ValidateNumeroConvenzione(false));
            this.drCustomer.ValidateNumeroRicezioneCommand = new Command(() => ValidateNumeroRicezione(false));
            this.drCustomer.ValidateDataOrdineCommand = new Command(() => ValidateDataOrdine(false));
            this.drCustomer.ValidateDataContrattoCommand = new Command(() => ValidateDataContratto(false));
            this.drCustomer.ValidateDataConvenzioneCommand = new Command(() => ValidateDataConvenzione(false));
            this.drCustomer.ValidateDataRicezioneCommand = new Command(() => ValidateDataRicezione(false));
        }

        private bool ValidateTaxIDNumber(Boolean daBottoneSalva)
        {
            return this.drCustomer.TaxIDNumber_VAL.Validate(daBottoneSalva);
        }

        private bool ValidateCompanyName(Boolean daBottoneSalva)
        {
            return this.drCustomer.CompanyName_VAL.Validate(daBottoneSalva);
        }

        private bool ValidateAddress(Boolean daBottoneSalva)
        {
            return this.drCustomer.Address_VAL.Validate(daBottoneSalva);
        }

        private bool ValidateTelephone(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateCity(Boolean daBottoneSalva)
        {
            return this.drCustomer.City_VAL.Validate(daBottoneSalva);
        }

        private bool ValidateZipCode(Boolean daBottoneSalva)
        {
            return this.drCustomer.ZipCode_VAL.Validate(daBottoneSalva);
        }

        //20180809 commentato sotto
        //private bool ValidateCounty(Boolean daBottoneSalva)
        //{
        //    return true;
        //}

        private bool ValidateCounty2(Boolean daBottoneSalva)
        {
            return this.drCustomer.County2_VAL.Validate(daBottoneSalva); //20190110
        }

        private bool ValidateCountry(Boolean daBottoneSalva)
        {
            return this.drCustomer.Country_VAL.Validate(daBottoneSalva);
        }

        //20180809 commentato sotto
        //private bool ValidateStateCode(Boolean daBottoneSalva)
        //{
        //    return this.drCustomer.StateCode_VAL.Validate(daBottoneSalva);
        //}

        private bool ValidateStateCode2(Boolean daBottoneSalva)
        {
            return this.drCustomer.StateCode2_VAL.Validate(daBottoneSalva);
        }

        private bool ValidateFiscalCode(Boolean daBottoneSalva)
        {
            return this.drCustomer.FiscalCode_VAL.Validate(daBottoneSalva);
        }

        private bool ValidatePec(Boolean daBottoneSalva)
        {
            return this.drCustomer.Pec_VAL.Validate(daBottoneSalva);
        }

        private bool ValidateEmail(Boolean daBottoneSalva)
        {
            return this.drCustomer.Email_VAL.Validate(daBottoneSalva);
        }

        private bool ValidateContactName(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateContactTelephone(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateContactEmail(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateCIG(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateCUP(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateSID(Boolean daBottoneSalva)
        {
            return this.drCustomer.SID_VAL.Validate(daBottoneSalva);
        }

        private bool ValidateTaxCodeDefault(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateNumeroOrdine(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateNumeroContratto(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateNumeroConvenzione(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateNumeroRicezione(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateDataOrdine(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateDataContratto(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateDataConvenzione(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool ValidateDataRicezione(Boolean daBottoneSalva)
        {
            return true;
        }

        private bool Validate()
        {
            //chiamare tutti i validatori uno ad uno e concatenare il valore
            //bool isValidPassword = ValidatePassword();
            bool isValidTaxIDNumber = ValidateTaxIDNumber(true);
            bool isValidCompanyName = ValidateCompanyName(true);
            bool isValidAddress = ValidateAddress(true);
            bool isValidTelephone = ValidateTelephone(true);
            bool isValidCity = ValidateCity(true);
            bool isValidZipCode = ValidateZipCode(true);
            //bool isValidCounty = ValidateCounty(true); //20180809
            bool isValidCounty2 = ValidateCounty2(true);
            bool isValidCountry = ValidateCountry(true);
            //bool isValidStateCode = ValidateStateCode(true); //20180809
            bool isValidStateCode2 = ValidateStateCode2(true);
            bool isValidFiscalCode = ValidateFiscalCode(true);
            bool isValidPec = ValidatePec(true);
            bool isValidEmail = ValidateEmail(true);
            bool isValidContactName = ValidateContactName(true);
            bool isValidContactTelephone = ValidateContactTelephone(true);
            bool isValidContactEmail = ValidateContactEmail(true);
            bool isValidCIG = ValidateCIG(true);
            bool isValidCUP = ValidateCUP(true);
            bool isValidSID = ValidateSID(true);
            bool isValidTaxCodeDefault = ValidateTaxCodeDefault(true);
            bool isValidNumeroOrdine = ValidateNumeroOrdine(true);
            bool isValidNumeroContratto = ValidateNumeroContratto(true);
            bool isValidNumeroConvenzione = ValidateNumeroConvenzione(true);
            bool isValidNumeroRicezione = ValidateNumeroRicezione(true);
            bool isValidDataOrdine = ValidateDataOrdine(true);
            bool isValidDataContratto = ValidateDataContratto(true);
            bool isValidDataConvenzione = ValidateDataConvenzione(true);
            bool isValidDataRicezione = ValidateDataRicezione(true);

            // && isValidPassw
            return isValidTaxIDNumber && isValidCompanyName && isValidAddress && isValidTelephone
                && isValidCity && isValidZipCode /*20180809&& isValidCounty*/ && isValidCounty2 && isValidCountry
                /*20180809&& isValidStateCode*/ && isValidStateCode2 && isValidFiscalCode && isValidPec && isValidEmail
                && isValidContactName && isValidContactTelephone && isValidContactEmail && isValidCIG && isValidCUP
                && isValidSID && isValidTaxCodeDefault && isValidNumeroOrdine && isValidNumeroContratto
                && isValidNumeroConvenzione && isValidNumeroRicezione && isValidDataOrdine
                && isValidDataContratto && isValidDataConvenzione && isValidDataRicezione;
        }
        #endregion
    }
}
