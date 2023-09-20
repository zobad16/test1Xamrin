using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using test1Xamrin;
using test1Xamrin.Resx;
using test1Xamrin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xUtilityPCL;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DettaglioCliente : ContentPage
    {
        FEC_CustomersSystem drCustomer { get; set; }
        public DettaglioClienteViewModel myViewModel { get; set; }
        private Boolean lClicked = true;

        public DettaglioCliente(FEC_CustomersSystem _drCustomer, ObservableCollection<FEC_CustomersSystem> _lista,
                                ObservableCollection<FEC_CustomersSystem> _listaClone)
        {
            InitializeComponent();
            //NavigationPage.SetHasBackButton(this, false);

            if (_drCustomer.StatoFormRispettoSqlite_IMC == "M")
                this.Title = AppResources.TitoloFormDettCliente;
            else
                this.Title = AppResources.TitoloFormNuovoCliente;


            drCustomer = _drCustomer;
            myViewModel = new DettaglioClienteViewModel(_drCustomer, _lista, Navigation, _listaClone);


            this.BindingContext = myViewModel.GetdrCorrente();

            if (swPA.IsToggled == false)
            {
                lblCIG.IsVisible = false;
                stackCIG.IsVisible = false;
                lblCUP.IsVisible = false;
                stackCUP.IsVisible = false;
            }
            else
            {
                lblCIG.IsVisible = true;
                stackCIG.IsVisible = true;
                lblCUP.IsVisible = true;
                stackCUP.IsVisible = true;
            }

            /*
            if (App.K_Attivazione_CompanyType == 1)
            {
                imgValidaTaxIDNumber.Source = "WebIconTipo1.png";
                imgMoreProv.Source = "MoreIcon.png";
                imgMoreStateCode.Source = "MoreIcon.png";
                imgMoreAltriDati.Source = "MoreIcon.png";
                imgMoreArticoliIVA.Source = "MoreIcon.png";
            }

            if (App.K_Attivazione_CompanyType == 2)
            {
                imgValidaTaxIDNumber.Source = "WebIconTipo2.png";
                imgMoreProv.Source = "MoreIconTipo2.png";
                imgMoreStateCode.Source = "MoreIconTipo2.png";
                imgMoreAltriDati.Source = "MoreIconTipo2.png";
                imgMoreArticoliIVA.Source = "MoreIconTipo2.png";
            }

            if (App.K_Attivazione_CompanyType == 3)
            {
                imgValidaTaxIDNumber.Source = "WebIconTipo3.png";
                imgMoreProv.Source = "MoreIconTipo3.png";
                imgMoreStateCode.Source = "MoreIconTipo3.png";
                imgMoreAltriDati.Source = "MoreIconTipo3.png";
                imgMoreArticoliIVA.Source = "MoreIconTipo3.png";
            }
            */
        }

        async void ValidaTaxIDNumber_Tapped(object sender, System.EventArgs e)
        {
            var myVerificaTaxIDNumber = await WSChiamateEF.getVerificaTaxIDNumber(this.drCustomer.StateCode2_VAL.Value, this.drCustomer.TaxIDNumber_VAL.Value);

            if (myVerificaTaxIDNumber.esitoOK_KO == "KO")
            {
                //i messaggi sono delegati alla funzione getIsDeviceEnabled
            }

            if (myVerificaTaxIDNumber.esitoOK_KO == "OK")
            {
                string labelMessaggio = AppResources.AlertLabelMessaggio;

                if (myVerificaTaxIDNumber.valid == false)
                {
                    string msgValidazioni = AppResources.VerificaPartitaIVAMessaggioPIVAnonValida;
                    await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "OK");
                    return;
                }

                if (myVerificaTaxIDNumber.valid == true)
                {
                    string msgValidazioni = AppResources.VerificaPartitaIVAMessaggioPIVAvalida + Environment.NewLine;
                    msgValidazioni += Environment.NewLine + AppResources.VerificaPartitaIVAMessaggioTrovatiSeguentiDati + ":";
                    msgValidazioni += Environment.NewLine + myVerificaTaxIDNumber.name + Environment.NewLine;

                    if (string.IsNullOrEmpty(myVerificaTaxIDNumber.address) == false)
                        msgValidazioni += myVerificaTaxIDNumber.address + " - ";

                    if (string.IsNullOrEmpty(myVerificaTaxIDNumber.zipCode) == false)
                        msgValidazioni += myVerificaTaxIDNumber.zipCode + " - ";

                    if (string.IsNullOrEmpty(myVerificaTaxIDNumber.city) == false)
                        msgValidazioni += myVerificaTaxIDNumber.city;

                    if (string.IsNullOrEmpty(myVerificaTaxIDNumber.county) == false)
                        msgValidazioni += " (" + myVerificaTaxIDNumber.county + ")";

                    msgValidazioni += Environment.NewLine + Environment.NewLine + AppResources.VerificaPartitaIVAMessaggioSiDesideraSalvareDati + "?";
                    var dresult = await Application.Current.MainPage.DisplayAlert(labelMessaggio, msgValidazioni, "Si", "No");

                    if (dresult == false)
                    {
                        return;
                    }
                    else
                    {
                        /*
                        this.drCustomer.CompanyName_VAL.Value = myVerificaTaxIDNumber.name;
                        this.drCustomer.Address_VAL.Value = myVerificaTaxIDNumber.address;

                        if (string.IsNullOrEmpty(myVerificaTaxIDNumber.zipCode) == false)
                        {
                            if (myVerificaTaxIDNumber.zipCode.Length > 5)
                            {

                            }
                            else
                            {
                                this.drCustomer.ZipCode_VAL.Value = myVerificaTaxIDNumber.zipCode;
                            }
                        }

                        this.drCustomer.City_VAL.Value = myVerificaTaxIDNumber.city;

                        using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                        {
                            var drCounty = cn.Table<FEC_Counties>().FirstOrDefault(y => y.CountyCode == myVerificaTaxIDNumber.county);

                            if (drCounty != null)
                            {
                                //provincia presente nella nostra tabella
                                this.drCustomer.County2_VAL.Value = myVerificaTaxIDNumber.county;
                            }

                            cn.Close();
                        }
                        */

                        //20190110 inizio
                        if (string.IsNullOrEmpty(myVerificaTaxIDNumber.name) == false)
                        {
                            if (myVerificaTaxIDNumber.name.Trim().Length > 80)
                            {
                                //tronco il nome azienda a 80
                                this.drCustomer.CompanyName_VAL.Value = myVerificaTaxIDNumber.name.Trim().Substring(0, 80);
                            }
                            else
                                this.drCustomer.CompanyName_VAL.Value = myVerificaTaxIDNumber.name.Trim();
                        }
                        else
                            this.drCustomer.CompanyName_VAL.Value = null;

                        if (string.IsNullOrEmpty(myVerificaTaxIDNumber.address) == false)
                            this.drCustomer.Address_VAL.Value = myVerificaTaxIDNumber.address.Trim();
                        else
                            this.drCustomer.Address_VAL.Value = null;

                        if (string.IsNullOrEmpty(myVerificaTaxIDNumber.zipCode) == false)
                        {
                            if (myVerificaTaxIDNumber.zipCode.Trim().Length > 5)
                            {
                                //se il cap ha più di 5 caratteri, non lo passo alla form e pulisco un eventuale valore già presente
                                this.drCustomer.ZipCode_VAL.Value = null;
                            }
                            else
                                this.drCustomer.ZipCode_VAL.Value = myVerificaTaxIDNumber.zipCode.Trim();
                        }
                        else
                            this.drCustomer.ZipCode_VAL.Value = null;

                        if (string.IsNullOrEmpty(myVerificaTaxIDNumber.city) == false)
                            this.drCustomer.City_VAL.Value = myVerificaTaxIDNumber.city.Trim();
                        else
                            this.drCustomer.City_VAL.Value = null;

                        if (string.IsNullOrEmpty(myVerificaTaxIDNumber.county) == false)
                        {
                            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                            {
                                var prov = myVerificaTaxIDNumber.county.Trim();
                                var drCounty = cn.Table<FEC_Counties>().FirstOrDefault(y => y.CountyCode == prov);

                                if (drCounty != null)
                                {
                                    //provincia presente nella nostra tabella
                                    this.drCustomer.County2_VAL.Value = myVerificaTaxIDNumber.county.Trim();
                                }
                                else
                                {
                                    //se la provincia non è presente in tabella, non la passo alla form e pulisco un eventuale valore già presente
                                    this.drCustomer.County2_VAL.Value = AppResources.FormClientiTestoSelezionaProvincia;
                                }

                                cn.Close();
                            }
                        }
                        else
                            this.drCustomer.County2_VAL.Value = AppResources.FormClientiTestoSelezionaProvincia;
                        //20190110 fine
                    }
                }
            }
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            var nCount = 3;

            //if (App.NuovoDocumentoSistemaNavigazione == App.SistemaNavigazioneEnum.Standard)
            //{
            //    if (Navigation.NavigationStack[0] is DettaglioCliente)
            //    {
            //        return; //aperta dal menu: caso non possibile
            //    }
            //    else
            //    {
            //        nCount -= 1;
            //    }
            //}

            if (Navigation.NavigationStack.Count == nCount) //in uscita dal dettaglio alla lista clienti
            {
                if (this.myViewModel.ultimobtnPremutoSalva_Annulla == "")
                {
                    this.myViewModel.lChiamataDaDisappearing = true;
                    await myViewModel.AnnullaCliente();
                }
            }

            //foreach (var a in this.Navigation.NavigationStack)
            //{
            //  var aa = 1;
            //}
        }

        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            //throw new NotImplementedException();

            if ((sender as Xamarin.Forms.Switch).IsToggled == false)
            {
                lblCIG.IsVisible = false;
                stackCIG.IsVisible = false;
                lblCUP.IsVisible = false;
                stackCUP.IsVisible = false;
            }
            else
            {
                lblCIG.IsVisible = true;
                stackCIG.IsVisible = true;
                lblCUP.IsVisible = true;
                stackCUP.IsVisible = true;
            }
        }

        void txtPec_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtPec_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 50) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtPec.Text = e.OldTextValue;
                else
                    this.txtPec.Text = "";
            }
            else
                this.txtPec.Text = e.NewTextValue;
        }

        void txtCompanyName_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtCompanyName_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 80) //20190110 cambiato valore da 100 a 80 //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtCompanyName.Text = e.OldTextValue;
                else
                    this.txtCompanyName.Text = "";
            }
            else //20190122 rifatto else, prima c'era solo this.txtCompanyName.Text = e.NewTextValue;
            {
                if (e.NewTextValue == "")
                    this.txtCompanyName.Text = e.NewTextValue;
                else
                {
                    var challenge = @"^[\p{IsBasicLatin}\p{IsLatin-1Supplement}]+$";
                    var lSuccess = Regex.Match(e.NewTextValue, challenge).Success;

                    if (lSuccess == true)
                        this.txtCompanyName.Text = e.NewTextValue;
                    else
                    {
                        if (e.OldTextValue != null)
                            this.txtCompanyName.Text = e.OldTextValue;
                        else
                            this.txtCompanyName.Text = "";
                    }
                }
            }
        }

        //20190122 scoperto errore su carattere ALT 0146 curly single close quote
        //https://usefulshortcuts.com/alt-codes/punctuation-alt-codes.php

        void txtAddress_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtAddress_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 200) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtAddress.Text = e.OldTextValue;
                else
                    this.txtAddress.Text = "";
            }
            else //20190122 rifatto else, prima c'era solo this.txtAddress.Text = e.NewTextValue;
            {
                if (e.NewTextValue == "")
                    this.txtAddress.Text = e.NewTextValue;
                else
                {
                    var challenge = @"^[\p{IsBasicLatin}\p{IsLatin-1Supplement}]+$";
                    var lSuccess = Regex.Match(e.NewTextValue, challenge).Success;

                    if (lSuccess == true)
                        this.txtAddress.Text = e.NewTextValue;
                    else
                    {
                        if (e.OldTextValue != null)
                            this.txtAddress.Text = e.OldTextValue;
                        else
                            this.txtAddress.Text = "";
                    }
                }
            }
        }

        void txtTelephone_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtTelephone_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 50) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtTelephone.Text = e.OldTextValue;
                else
                    this.txtTelephone.Text = "";
            }
            else
                this.txtTelephone.Text = e.NewTextValue;
        }

        void txtCity_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtCity_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 50) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtCity.Text = e.OldTextValue;
                else
                    this.txtCity.Text = "";
            }
            else
                this.txtCity.Text = e.NewTextValue;
        }

        void txtZipCode_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtZipCode_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 5) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtZipCode.Text = e.OldTextValue;
                else
                    this.txtZipCode.Text = "";
            }
            else
                this.txtZipCode.Text = e.NewTextValue;

        }

        void txtFiscalCode_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (e.NewTextValue != null && e.NewTextValue != "")
                txtFiscalCode.Text = e.NewTextValue.ToUpper();
        }

        void txtEmail_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtEmail_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 50) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtEmail.Text = e.OldTextValue;
                else
                    this.txtEmail.Text = "";
            }
            else
                this.txtEmail.Text = e.NewTextValue;
        }

        void txtContactName_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtContactName_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 50) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtContactName.Text = e.OldTextValue;
                else
                    this.txtContactName.Text = "";
            }
            else
                this.txtContactName.Text = e.NewTextValue;
        }

        void txtContactTelephone_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtContactTelephone_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 50) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtContactTelephone.Text = e.OldTextValue;
                else
                    this.txtContactTelephone.Text = "";
            }
            else
                this.txtContactTelephone.Text = e.NewTextValue;
        }

        void txtContactEmail_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtContactEmail_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 50) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtContactEmail.Text = e.OldTextValue;
                else
                    this.txtContactEmail.Text = "";
            }
            else
                this.txtContactEmail.Text = e.NewTextValue;
        }

        async void AltriDati_Tapped(object sender, System.EventArgs e)
        {
            if (lClicked)
            {
                lClicked = false;
                await Navigation.PushAsync(new DettaglioClienteAltriDati(this.drCustomer));
                lClicked = true;
            }
        }

        /*
        async void ArticoliIVA_Tapped(object sender, System.EventArgs e)
        {
            if (lClicked)
            {
                lClicked = false;

                await Navigation.PushAsync(new GestioneCodIVA(this.drCustomer, false));

                lClicked = true;
            }
        }
        */

        async void County_Tapped(object sender, System.EventArgs e)
        {
            if (lClicked)
            {
                lClicked = false;
                await Navigation.PushAsync(new Province(this.drCustomer));
                lClicked = true;
            }
        }

        async void StateCode_Tapped(object sender, System.EventArgs e)
        {
            if (lClicked)
            {
                lClicked = false;
                await Navigation.PushAsync(new Stati(this.drCustomer));
                lClicked = true;
            }
        }
    }
}