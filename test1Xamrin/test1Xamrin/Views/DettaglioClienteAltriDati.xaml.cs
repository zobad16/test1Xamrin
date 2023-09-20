using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test1Xamrin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xUtilityPCL;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DettaglioClienteAltriDati : ContentPage
    {
        public FEC_CustomersSystem drCustomer { get; set; }

        public DettaglioClienteAltriDati(FEC_CustomersSystem _drCustomer)
        {
            InitializeComponent();
            drCustomer = _drCustomer;

            //20180630 inizio
            if (drCustomer.DataOrdine.HasValue)
            {
                txtDataOrdinePicker.SelectedItem = this.txtDataOrdinePicker.date2SFPickerFormat(drCustomer.DataOrdine.Value);
            }
            else
            {

            }

            if (drCustomer.DataContratto.HasValue)
            {
                txtDataContrattoPicker.SelectedItem = this.txtDataContrattoPicker.date2SFPickerFormat(drCustomer.DataContratto.Value);
            }
            else
            {

            }

            if (drCustomer.DataConvenzione.HasValue)
            {
                txtDataConvenzionePicker.SelectedItem = this.txtDataConvenzionePicker.date2SFPickerFormat(drCustomer.DataConvenzione.Value);
            }
            else
            {

            }

            if (drCustomer.DatiRicezione.HasValue)
            {
                txtDataRicezionePicker.SelectedItem = this.txtDataRicezionePicker.date2SFPickerFormat(drCustomer.DatiRicezione.Value);
            }
            else
            {

            }
            //20180630

            this.BindingContext = _drCustomer;

            /*
            if (App.K_Attivazione_CompanyType == 1)
            {
                imgPulisciDataOrdine.Source = "ClearIcon.png";
                imgPulisciDataContratto.Source = "ClearIcon.png";
                imgPulisciDataRicezione.Source = "ClearIcon.png";
                imgPulisciDataConvenzione.Source = "ClearIcon.png";
                lblSoggettoARitenuta.IsVisible = false;
                swSoggettoARitenuta.IsVisible = false;
                lblSplitPayment.IsVisible = false;
                swSplitPayment.IsVisible = false;
            }

            if (App.K_Attivazione_CompanyType == 2)
            {
                imgPulisciDataOrdine.Source = "ClearIconTipo2.png";
                imgPulisciDataContratto.Source = "ClearIconTipo2.png";
                imgPulisciDataRicezione.Source = "ClearIconTipo2.png";
                imgPulisciDataConvenzione.Source = "ClearIconTipo2.png";
                lblSoggettoARitenuta.IsVisible = true;
                swSoggettoARitenuta.IsVisible = true;
                lblSplitPayment.IsVisible = true;
                swSplitPayment.IsVisible = true;
            }

            if (App.K_Attivazione_CompanyType == 3)
            {
                imgPulisciDataOrdine.Source = "ClearIconTipo3.png";
                imgPulisciDataContratto.Source = "ClearIconTipo3.png";
                imgPulisciDataRicezione.Source = "ClearIconTipo3.png";
                imgPulisciDataConvenzione.Source = "ClearIconTipo3.png";
                lblSoggettoARitenuta.IsVisible = false;
                swSoggettoARitenuta.IsVisible = false;
                lblSplitPayment.IsVisible = false;
                swSplitPayment.IsVisible = false;
            }
            */
        }

        void txtDataOrdine_Tapped(object sender, System.EventArgs e)
        {
            txtDataOrdinePicker.IsOpen = true;
            //picker.IsOpen = true;
        }

        void txtDataOrdinePicker_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            if (this.txtDataOrdinePicker.SelectedItem != null)
            {
                var d = this.txtDataOrdinePicker.SFPickerFormat2Date(this.txtDataOrdinePicker.SelectedItem as ObservableCollection<object>);
                txtDataOrdine.Text = d.Value.ToString(Utility.getDateFormat());//20170630"dd/MM/yyyy");
            }
            else
            {
                txtDataOrdine.Text = new DateTime(1990, 01, 01).ToString(Utility.getDateFormat());//20180630 "01/01/1990";
            }
        }

        void pulisciTxtDataOrdine_Tapped(object sender, System.EventArgs e)
        {
            txtDataOrdine.Text = "";
            drCustomer.DataOrdine_VAL.Value = null;
        }

        void txtDataContratto_Tapped(object sender, System.EventArgs e)
        {
            txtDataContrattoPicker.IsOpen = true;
        }

        void txtDataContrattoPicker_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            if (this.txtDataContrattoPicker.SelectedItem != null)
            {
                var d = this.txtDataContrattoPicker.SFPickerFormat2Date(this.txtDataContrattoPicker.SelectedItem as ObservableCollection<object>);
                txtDataContratto.Text = d.Value.ToString(Utility.getDateFormat());//20170630"dd/MM/yyyy");
            }
            else
            {
                txtDataContratto.Text = new DateTime(1990, 01, 01).ToString(Utility.getDateFormat());//20180630 "01/01/1990";
            }
        }

        void pulisciTxtDataContratto_Tapped(object sender, System.EventArgs e)
        {
            txtDataContratto.Text = "";
            drCustomer.DataContratto_VAL.Value = null;
        }

        void txtDataConvenzione_Tapped(object sender, System.EventArgs e)
        {
            txtDataConvenzionePicker.IsOpen = true;
        }

        void txtDataConvenzionePicker_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            if (this.txtDataConvenzionePicker.SelectedItem != null)
            {
                var d = this.txtDataConvenzionePicker.SFPickerFormat2Date(this.txtDataConvenzionePicker.SelectedItem as ObservableCollection<object>);
                txtDataConvenzione.Text = d.Value.ToString(Utility.getDateFormat());//20180630"dd/MM/yyyy");
            }
            else
            {
                txtDataConvenzione.Text = new DateTime(1990, 01, 01).ToString(Utility.getDateFormat());//20180630 "01/01/1990";
            }
        }

        void pulisciTxtDataConvenzione_Tapped(object sender, System.EventArgs e)
        {
            txtDataConvenzione.Text = "";
            drCustomer.DataConvenzione_VAL.Value = null;
        }

        void txtDataRicezione_Tapped(object sender, System.EventArgs e)
        {
            txtDataRicezionePicker.IsOpen = true;
        }

        void txtDataRicezionePicker_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            if (this.txtDataRicezionePicker.SelectedItem != null)
            {
                var d = this.txtDataRicezionePicker.SFPickerFormat2Date(this.txtDataRicezionePicker.SelectedItem as ObservableCollection<object>);
                txtDataRicezione.Text = d.Value.ToString(Utility.getDateFormat());//20180630"dd/MM/yyyy");
            }
            else
            {
                txtDataRicezione.Text = new DateTime(1990, 01, 01).ToString(Utility.getDateFormat());//20180630 "01/01/1990";
            }
        }

        void pulisciTxtDataRicezione_Tapped(object sender, System.EventArgs e)
        {
            txtDataRicezione.Text = "";
            drCustomer.DataRicezione_VAL.Value = null;
        }
    }
}