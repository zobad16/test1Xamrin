using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test1Xamrin.Resx;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuovoDocumentoAltriDati : ContentPage
    {
        public FEC_InvoiceRequests drTestata { get; set; }
        private Boolean lClicked { get; set; } = true;

        public NuovoDocumentoAltriDati(FEC_InvoiceRequests _drTestata)
        {
            InitializeComponent();

            this.drTestata = _drTestata;

            this.BindingContext = this;

            if (drTestata.ReceiptPicture2 == null)
            {
                this.imgVediFoto2Scontrino.Source = "ViewPhotoNotPresentIcon.png";
                this.imgCancellaFoto2Scontrino.Source = "ClearNotPresentIcon.png";
            }
            else
            {
                this.imgVediFoto2Scontrino.Source = "ViewPhotoIcon.png";
                this.imgCancellaFoto2Scontrino.Source = "ClearIcon.png";
            }

            if (drTestata.ReceiptPicture3 == null)
            {
                this.imgVediFoto3Scontrino.Source = "ViewPhotoNotPresentIcon.png";
                this.imgCancellaFoto3Scontrino.Source = "ClearNotPresentIcon.png";
            }
            else
            {
                this.imgVediFoto3Scontrino.Source = "ViewPhotoIcon.png";
                this.imgCancellaFoto3Scontrino.Source = "ClearIcon.png";
            }

            if (drTestata.ReceiptQR == null)
            {
                this.imgCancellaQRScontrino.Source = "ClearNotPresentIcon.png";
            }
            else
            {
                this.imgCancellaQRScontrino.Source = "ClearIcon.png";
            }
        }

        void txtReceiptPointOfSale_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e) //20190110
        {
            System.Diagnostics.Debug.WriteLine("txtReceiptPointOfSale_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 16) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtReceiptPointOfSale.Text = e.OldTextValue;
                else
                    this.txtReceiptPointOfSale.Text = "";
            }
            else
                this.txtReceiptPointOfSale.Text = e.NewTextValue;
        }

        void txtReceiptTerminalNumber_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e) //20190110
        {
            System.Diagnostics.Debug.WriteLine("txtReceiptTerminalNumber_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 16) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtReceiptTerminalNumber.Text = e.OldTextValue;
                else
                    this.txtReceiptTerminalNumber.Text = "";
            }
            else
                this.txtReceiptTerminalNumber.Text = e.NewTextValue;
        }

        void txtReceiptPumpNumber_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e) //20190110
        {
            System.Diagnostics.Debug.WriteLine("txtReceiptPumpNumber_TextChanged " + e.NewTextValue); //20190112
            if (e.NewTextValue == null)
                return;
            if (e.NewTextValue.Length > 16) //20190112 tolto .Trim()
            {
                if (e.OldTextValue != null)
                    this.txtReceiptPumpNumber.Text = e.OldTextValue;
                else
                    this.txtReceiptPumpNumber.Text = "";
            }
            else
                this.txtReceiptPumpNumber.Text = e.NewTextValue;
        }

        async void Foto2Scontrino_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (string.IsNullOrEmpty(this.drTestata.ReceiptPicture2) == false)
            {
                var dresult = await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.FormNuovoDocDomandaSovrasciviImmagine, "Si", "No");

                if (dresult == false)
                    return;
            }

            await NuovoDocumento.TakePicture(this.drTestata, 2);

            this.imgVediFoto2Scontrino.Source = "ViewPhotoIcon.png";
            this.imgCancellaFoto2Scontrino.Source = "ClearIcon.png";
        }

        async void VediFoto2Scontrino_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (string.IsNullOrEmpty(this.drTestata.ReceiptPicture2))
            {
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.FormNuovoDocMessaggioNessunaImmPresDaVisualizzare, "OK");
                return;
            }

            byte[] arr = Convert.FromBase64String(this.drTestata.ReceiptPicture2);

            await Navigation.PushAsync(new ImageZoomer(arr) { BindingContext = new zoom.TransformImageViewModel() });
        }

        async void CancellaFoto2Scontrino_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (string.IsNullOrEmpty(this.drTestata.ReceiptPicture2))
            {
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.FormNuovoDocMessaggioNessunaImmPresDaCancellare, "OK");
                return;
            }

            var dresult = await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.MessaggioConfermaCancellazione, "Si", "No");

            if (dresult == false)
                return;
            else
            {
                drTestata.ReceiptPicture2 = null;

                this.imgVediFoto2Scontrino.Source = "ViewPhotoNotPresentIcon.png";
                this.imgCancellaFoto2Scontrino.Source = "ClearNotPresentIcon.png";
            }
        }

        async void Foto3Scontrino_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (string.IsNullOrEmpty(this.drTestata.ReceiptPicture3) == false)
            {
                var dresult = await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.FormNuovoDocDomandaSovrasciviImmagine, "Si", "No");

                if (dresult == false)
                    return;
            }

            await NuovoDocumento.TakePicture(this.drTestata, 3);

            this.imgVediFoto3Scontrino.Source = "ViewPhotoIcon.png";
            this.imgCancellaFoto3Scontrino.Source = "ClearIcon.png";
        }

        async void VediFoto3Scontrino_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (string.IsNullOrEmpty(this.drTestata.ReceiptPicture3))
            {
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.FormNuovoDocMessaggioNessunaImmPresDaVisualizzare, "OK");
                return;
            }

            byte[] arr = Convert.FromBase64String(this.drTestata.ReceiptPicture3);

            await Navigation.PushAsync(new ImageZoomer(arr) { BindingContext = new zoom.TransformImageViewModel() });
        }

        async void CancellaFoto3Scontrino_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (string.IsNullOrEmpty(this.drTestata.ReceiptPicture3))
            {
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.FormNuovoDocMessaggioNessunaImmPresDaCancellare, "OK");
                return;
            }

            var dresult = await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.MessaggioConfermaCancellazione, "Si", "No");

            if (dresult == false)
                return;
            else
            {
                drTestata.ReceiptPicture3 = null;

                this.imgVediFoto3Scontrino.Source = "ViewPhotoNotPresentIcon.png";
                this.imgCancellaFoto3Scontrino.Source = "ClearNotPresentIcon.png";
            }
        }

        async void QRScontrino_Tapped(object sender, System.EventArgs e)
        {
            if (lClicked)
            {
                lClicked = false;

                string labelMessaggio = AppResources.AlertLabelMessaggio;

                if (string.IsNullOrEmpty(this.drTestata.ReceiptQR) == false)
                {
                    var dresult = await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.FormNuovoDocDomandaSovrasciviQR, "Si", "No");

                    if (dresult == false)
                    {
                        lClicked = true;
                        return;
                    }
                }

                //inizio lettura QR standard AGE
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

                        var el0 = arrQR[0];

                        if (el0.Length > 100)
                        {
                            //maggiore di 100
                            lClicked = true;
                            await Application.Current.MainPage.DisplayAlert(labelMessaggio, QRNonStand + result.Text, "OK");
                            return;
                        }
                        else
                        {
                            drTestata.ReceiptQR = el0;

                            this.imgCancellaQRScontrino.Source = "ClearIcon.png";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        await Application.Current.MainPage.DisplayAlert(labelMessaggio, QRNonStand + result.Text, "OK");
                    }

                    //await Application.Current.MainPage.DisplayAlert("DOne", "Scanned Barcode: " + result.Text, "OK");
                }

                lClicked = true;
            }
        }

        async void CancellaQRScontrino_Tapped(object sender, System.EventArgs e)
        {
            string labelMessaggio = AppResources.AlertLabelMessaggio;

            if (string.IsNullOrEmpty(this.drTestata.ReceiptQR))
            {
                await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.FormNuovoDocMessaggioNessunQRPresDaCancellare, "OK");
                return;
            }

            var dresult = await Application.Current.MainPage.DisplayAlert(labelMessaggio, AppResources.MessaggioConfermaCancellazione, "Si", "No");

            if (dresult == false)
                return;
            else
            {
                drTestata.ReceiptQR = null;

                this.imgCancellaQRScontrino.Source = "ClearNotPresentIcon.png";
            }
        }
    }
}