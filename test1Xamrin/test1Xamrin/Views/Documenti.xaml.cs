using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Documenti : ContentPage
    {
        public ICommand cmdQuadrato1 { get; private set; }
        public ICommand cmdQuadrato2 { get; private set; }
        public ICommand cmdQuadrato3 { get; private set; }
        public ICommand cmdQuadrato4 { get; private set; }
        public ICommand cmdDocumenti { get; private set; }
        public ICommand cmdSearchBar { get; private set; }
        public ICommand cmdbtnCerca { get; private set; }
        public DocumentiItemsWrapper myDocumentiItemsWrapper { get; set; }
        public DaPassareBenzinaiWrapper myDaPassareBenzinaiWrapper { get; set; }
        private Boolean isLoaded { get; set; }
        private Boolean lClicked { get; set; } = true;
        //public string BenzinaioParam { get; set; } = "";
        public string DateFROMParam { get; set; }
        public string DateTOParam { get; set; }


        public Documenti()
        {
            InitializeComponent();

            myDaPassareBenzinaiWrapper = new DaPassareBenzinaiWrapper();

            pickerDataDA.SelectedItem = pickerDataDA.date2SFPickerFormat(new DateTime(1990, 01, 01));
            txtDataDA.Text = "";
            pickerDataDA.SelectedItem = pickerDataDA.date2SFPickerFormat(new DateTime(1990, 01, 01));
            txtDataA.Text = "";

            this.myDocumentiItemsWrapper = new DocumentiItemsWrapper();
            this.cmdQuadrato1 = new Command(async () => await QuadratoUnoEventHandler());
            this.cmdQuadrato2 = new Command(async () => await QuadratoDueEventHandler());
            this.cmdQuadrato3 = new Command(async () => await QuadratoTreEventHandler());
            this.cmdQuadrato4 = new Command(async () => await QuadratoQuattroEventHandler());
            this.cmdDocumenti = new Command<FEC_InvoiceRequests>(async (item) => await OnSelectedItem(item));
            this.cmdSearchBar = new Command<string>(async (text) => await cercaDoc(text));
            this.cmdbtnCerca = new Command(async () => await Cerca());

            this.BindingContext = this;
        }

        async void SceltaBenzinaio_Tapped(object sender, System.EventArgs e)
        {
            if (lClicked)
            {
                lClicked = false;

                await Navigation.PushAsync(new Benzinai(this.myDaPassareBenzinaiWrapper));

                lClicked = true;
            }
        }

        void txtDataDA_Tapped(object sender, System.EventArgs e)
        {
            pickerDataDA.IsOpen = true;
        }

        void lblPulisciDataDA_Tapped(object sender, System.EventArgs e)
        {
            txtDataDA.Text = "";
        }

        void pickerDataDA_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            var d = this.pickerDataDA.SFPickerFormat2Date(this.pickerDataDA.SelectedItem as ObservableCollection<object>);
            txtDataDA.Text = d.Value.ToString(Utility.getDateFormat()); //20180630"dd/MM/yyyy");
        }

        void txtDataA_Tapped(object sender, System.EventArgs e)
        {
            pickerDataA.IsOpen = true;
        }

        void lblPulisciDataA_Tapped(object sender, System.EventArgs e)
        {
            txtDataA.Text = "";
        }

        void pickerDataA_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            var d = this.pickerDataA.SFPickerFormat2Date(this.pickerDataA.SelectedItem as ObservableCollection<object>);
            txtDataA.Text = d.Value.ToString(Utility.getDateFormat()); //20180630"dd/MM/yyyy");
        }

        public List<FEC_InvoiceRequests> Ordina(List<FEC_InvoiceRequests> l)
        {
            l = l.OrderBy(y => y.CompanyName).ThenByDescending(Y => Y.Receiptdate).ToList();
            return l;
        }

        async private Task Cerca()
        {
            DateTime dataDA = new DateTime(1900, 1, 1);
            DateTime dataA = DateTime.Today;
            if (string.IsNullOrEmpty(this.DateFROMParam) == false)
                dataDA = Convert.ToDateTime(this.DateFROMParam);
            if (string.IsNullOrEmpty(this.DateTOParam) == false)
                dataA = Convert.ToDateTime(this.DateTOParam);

            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                var ll = cn.Table<FEC_InvoiceRequests>().Where(y => y.Receiptdate >= dataDA && y.Receiptdate <= dataA).ToList();

                ll = Ordina(ll);

                if (string.IsNullOrEmpty(this.myDaPassareBenzinaiWrapper.BenzinaioParam) == false)
                {
                    ll = ll.Where(y => y.IDCompanyCompanyName == this.myDaPassareBenzinaiWrapper.BenzinaioParam).ToList(); //20191125
                }

                this.myDocumentiItemsWrapper.DocumentiItems = new ObservableCollection<FEC_InvoiceRequests>(ll);
                this.myDocumentiItemsWrapper.DocumentiItemsClone = new ObservableCollection<FEC_InvoiceRequests>(ll);

                foreach (var dr in ll)
                {
                    NuovoDocumento.settaCampiUnboundTestata(dr);
                    dr.IsMsgRichiestaRespintaVisibleUnbound = false;

                    //20190121 inizio
                    if (dr.DocumentState2 != null && dr.DocumentState2 == "1") //approvate
                    {
                        var c = Application.Current.Resources["DocApprovatiColor"];
                        dr.ColoreUnbound = Color.FromRgb(((Color)c).R, ((Color)c).G, ((Color)c).B);
                    }

                    if (dr.DocumentState2 != null && dr.DocumentState2 == "2") //respinte
                    {
                        var c = Application.Current.Resources["DocRespintiColor"];
                        dr.ColoreUnbound = Color.FromRgb(((Color)c).R, ((Color)c).G, ((Color)c).B);

                        dr.IsMsgRichiestaRespintaVisibleUnbound = true;
                        dr.MsgRichiestaRespintaUnbound = AppResources.FormDocumentiParolaRespinta;

                        if (string.IsNullOrEmpty(dr.DocumentStateMessage) == false)
                            dr.MsgRichiestaRespintaUnbound += ": " + dr.DocumentStateMessage;
                    }

                    if (dr.DocumentState2 == null) //da approvare
                    {
                        var c = Application.Current.Resources["DocDaApprovareColor"];
                        dr.ColoreUnbound = Color.FromRgb(((Color)c).R, ((Color)c).G, ((Color)c).B);
                    }
                    //20190121 fine
                }

                cn.Close();
            }
        }

        async Task cercaDoc(string myText)
        {
            await Task.Delay(1);
            if (string.IsNullOrEmpty(myText))
            {
                //ripristino
                this.myDocumentiItemsWrapper.DocumentiItems = new ObservableCollection<FEC_InvoiceRequests>(this.myDocumentiItemsWrapper.DocumentiItemsClone);
            }
            else
            {
                //filtro
                var l = this.myDocumentiItemsWrapper.DocumentiItemsClone.Where
                                                    (x =>
                                                        (
                                                         (x as FEC_InvoiceRequests).DescrizioneTipoUnbound != null &&
                                                         (x as FEC_InvoiceRequests).DescrizioneTipoUnbound.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                        ||
                                                        (
                                                         (x as FEC_InvoiceRequests).NumeroEDataUnbound != null &&
                                                         (x as FEC_InvoiceRequests).NumeroEDataUnbound.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                     ||
                                                        (
                                                         (x as FEC_InvoiceRequests).IDCompanyCompanyName != null &&
                                                         (x as FEC_InvoiceRequests).IDCompanyCompanyName.ToUpper().Contains(myText.ToUpper())
                                                        )
                                                        ).ToList();
                this.myDocumentiItemsWrapper.DocumentiItems = new ObservableCollection<FEC_InvoiceRequests>(l);
            }

            //abc = myText;
        }

        void lblHideEventHandler(object sender, System.EventArgs e)
        {
            //https://forums.xamarin.com/discussion/84917/expandable-layout-in-xamarin-forms
            if (slToHide.IsVisible)
            {
                slToHide.IsVisible = false;
                //HideButton.Text = strEspandiRicerca;
                //imgCollapseExpand.Source = "ExpandIcon.png";

                imgCollapseExpand.Source = "ExpandIcon.png";
            }
            else
            {
                slToHide.IsVisible = true;
                //HideButton.Text = strChiudiRicerca;
                //imgCollapseExpand.Source = "CollapseIcon.png";

                imgCollapseExpand.Source = "CollapseIcon.png";
            }
        }

        async private Task QuadratoUnoEventHandler() //tutte le richieste
        {
            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                var l = cn.Table<FEC_InvoiceRequests>().ToList();
                l = Ordina(l);

                this.myDocumentiItemsWrapper.DocumentiItems = new ObservableCollection<FEC_InvoiceRequests>(l);
                this.myDocumentiItemsWrapper.DocumentiItemsClone = new ObservableCollection<FEC_InvoiceRequests>(l);

                foreach (var dr in l)
                {
                    NuovoDocumento.settaCampiUnboundTestata(dr);
                    dr.IsMsgRichiestaRespintaVisibleUnbound = false;

                    //20190121 inizio
                    if (dr.DocumentState2 != null && dr.DocumentState2 == "1") //approvate
                    {
                        var c = Application.Current.Resources["DocApprovatiColor"];
                        dr.ColoreUnbound = Color.FromRgb(((Color)c).R, ((Color)c).G, ((Color)c).B);
                    }

                    if (dr.DocumentState2 != null && dr.DocumentState2 == "2") //respinte
                    {
                        var c = Application.Current.Resources["DocRespintiColor"];
                        dr.ColoreUnbound = Color.FromRgb(((Color)c).R, ((Color)c).G, ((Color)c).B);

                        dr.IsMsgRichiestaRespintaVisibleUnbound = true;
                        dr.MsgRichiestaRespintaUnbound = AppResources.FormDocumentiParolaRespinta;

                        if (string.IsNullOrEmpty(dr.DocumentStateMessage) == false)
                            dr.MsgRichiestaRespintaUnbound += ": " + dr.DocumentStateMessage;
                    }

                    if (dr.DocumentState2 == null) //da approvare
                    {
                        var c = Application.Current.Resources["DocDaApprovareColor"];
                        dr.ColoreUnbound = Color.FromRgb(((Color)c).R, ((Color)c).G, ((Color)c).B);
                    }
                    //20190121 fine
                }

                cn.Close();
            }
        }

        async private Task QuadratoDueEventHandler() //approvate
        {
            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                //var l = cn.Table<FEC_InvoiceRequests>().Where(Y => Y.SaleDocDate != null).ToList(); //20190121
                var l = cn.Table<FEC_InvoiceRequests>().Where(Y => Y.DocumentState2 != null && Y.DocumentState2 == "1").ToList();
                l = Ordina(l);

                this.myDocumentiItemsWrapper.DocumentiItems = new ObservableCollection<FEC_InvoiceRequests>(l);
                this.myDocumentiItemsWrapper.DocumentiItemsClone = new ObservableCollection<FEC_InvoiceRequests>(l);

                foreach (var dr in l)
                {
                    NuovoDocumento.settaCampiUnboundTestata(dr);

                    var c = Application.Current.Resources["DocApprovatiColor"]; //20190121
                    dr.ColoreUnbound = Color.FromRgb(((Color)c).R, ((Color)c).G, ((Color)c).B);

                    dr.IsMsgRichiestaRespintaVisibleUnbound = false;
                }

                cn.Close();
            }
        }

        async private Task QuadratoTreEventHandler() //respinte //20190121
        {
            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                var l = cn.Table<FEC_InvoiceRequests>().Where(Y => Y.DocumentState2 != null && Y.DocumentState2 == "2").ToList();
                l = Ordina(l);

                this.myDocumentiItemsWrapper.DocumentiItems = new ObservableCollection<FEC_InvoiceRequests>(l);
                this.myDocumentiItemsWrapper.DocumentiItemsClone = new ObservableCollection<FEC_InvoiceRequests>(l);

                foreach (var dr in l)
                {
                    NuovoDocumento.settaCampiUnboundTestata(dr);

                    var c = Application.Current.Resources["DocRespintiColor"];
                    dr.ColoreUnbound = Color.FromRgb(((Color)c).R, ((Color)c).G, ((Color)c).B);

                    dr.IsMsgRichiestaRespintaVisibleUnbound = true;
                    dr.MsgRichiestaRespintaUnbound = AppResources.FormDocumentiParolaRespinta;

                    if (string.IsNullOrEmpty(dr.DocumentStateMessage) == false)
                        dr.MsgRichiestaRespintaUnbound += ": " + dr.DocumentStateMessage;
                }

                cn.Close();
            }
        }

        async private Task QuadratoQuattroEventHandler() //da approvare
        {
            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                //var l = cn.Table<FEC_InvoiceRequests>().Where(Y => Y.SaleDocDate == null).ToList(); //20190121
                var l = cn.Table<FEC_InvoiceRequests>().Where(Y => Y.DocumentState2 == null).ToList();
                l = Ordina(l);

                this.myDocumentiItemsWrapper.DocumentiItems = new ObservableCollection<FEC_InvoiceRequests>(l);
                this.myDocumentiItemsWrapper.DocumentiItemsClone = new ObservableCollection<FEC_InvoiceRequests>(l);

                foreach (var dr in l)
                {
                    NuovoDocumento.settaCampiUnboundTestata(dr);

                    var c = Application.Current.Resources["DocDaApprovareColor"];
                    dr.ColoreUnbound = Color.FromRgb(((Color)c).R, ((Color)c).G, ((Color)c).B); //20190121

                    dr.IsMsgRichiestaRespintaVisibleUnbound = false;
                }

                cn.Close();
            }
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((Xamarin.Forms.MenuItem)sender);
            var drInvoiceRequest = (mi.CommandParameter as FEC_InvoiceRequests);

            if (drInvoiceRequest.RowState == xUtilityPCL.BaseModel.RowStateEnum.Added)
            {
                //riga inserita ma ancora da sincronizzare: al salvataggio sincronizziamo, ma potrebbe essere che non c'era campo quindi
                //la riga ha ancora lo stato di I rispetto al server-->la cancello da sqllite
                this.myDocumentiItemsWrapper.DocumentiItems.Remove(drInvoiceRequest);
                this.myDocumentiItemsWrapper.DocumentiItemsClone.Remove(drInvoiceRequest);
                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    cn.Delete(drInvoiceRequest);
                    cn.Close();
                }
            }

            if (drInvoiceRequest.RowState == xUtilityPCL.BaseModel.RowStateEnum.Unchanged ||
                drInvoiceRequest.RowState == xUtilityPCL.BaseModel.RowStateEnum.Modified)
            {
                //la riga è presente sul server --> porto il ROWState a deleted, in modo che alla successiva sincro il record venga cancellato
                drInvoiceRequest.RowState = xUtilityPCL.BaseModel.RowStateEnum.Deleted;
                this.myDocumentiItemsWrapper.DocumentiItems.Remove(drInvoiceRequest);
                this.myDocumentiItemsWrapper.DocumentiItemsClone.Remove(drInvoiceRequest);
                using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
                {
                    cn.Update(drInvoiceRequest);
                    cn.Close();
                }

                var tSincro = await WSChiamateEF.getTabelleAnagrafichexFatturazione();

                if (tSincro.Item5 != "")
                {
                    var descr = AppResources.SincroMessErrore;
                    string labelMessaggio = AppResources.AlertLabelMessaggio;
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

                }
            }
        }

        async Task OnSelectedItem(FEC_InvoiceRequests item)
        {
            if (item == null)
                return;

            if (item.RowState == xUtilityPCL.BaseModel.RowStateEnum.Added)
            {
                //la mantengo in added
            }
            else
            {
                item.RowState = xUtilityPCL.BaseModel.RowStateEnum.Modified;
            }

            var myHud = new ChatBotLoadIndicator(); //20181202
            myHud.SettaLabel(AppResources.NuovoHudColoratoMessaggioAttesa); //20181202
            DependencyService.Get<ILodingPageService>().InitLoadingPage(myHud); //20181202
            DependencyService.Get<ILodingPageService>().ShowLoadingPage(); //20181202
            await WSChiamateEF.getItemsBenzinaio(item.IDCompany.Value);
            await WSChiamateEF.getPaymentTermsBenzinaio(item.IDCompany.Value);
            await WSChiamateEF.getTaxCodesBenzinaio(item.IDCompany.Value);

            var f = new NuovoDocumento(item, "M");
            DependencyService.Get<ILodingPageService>().HideLoadingPage(); //20181125

            await Navigation.PushAsync(f, true);
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLite.SQLiteConnection cn = await sqliteHelper.creaDataBaseORGetConnection(App.sqliteDbName))
            {
                var totRichieste = cn.Table<FEC_InvoiceRequests>().Count();
                lblDocTutti.Text = totRichieste.ToString();

                //var totApprovate = cn.Table<FEC_InvoiceRequests>().Where(Y => Y.SaleDocDate != null).Count(); //20190121
                var totApprovate = cn.Table<FEC_InvoiceRequests>().Where(Y => Y.DocumentState2 != null && Y.DocumentState2 == "1").Count();
                lblDocApprovati.Text = totApprovate.ToString();

                var totRespinte = cn.Table<FEC_InvoiceRequests>().Where(Y => Y.DocumentState2 != null && Y.DocumentState2 == "2").Count(); //20190121
                lblDocRespinti.Text = totRespinte.ToString();

                //var totDaApprovare = cn.Table<FEC_InvoiceRequests>().Where(Y => Y.SaleDocDate == null).Count();  //20190121
                var totDaApprovare = cn.Table<FEC_InvoiceRequests>().Where(Y => Y.DocumentState2 == null).Count();
                lblDocDaApprovare.Text = totDaApprovare.ToString();

                cn.Close();
            }

            if (isLoaded == false)
            {
                //await myModel.CreaLista();
                isLoaded = true;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            this.listView.SelectedItem = null;
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class DocumentiItemsWrapper
    {
        public DocumentiItemsWrapper()
        {
            this.DocumentiItems = new ObservableCollection<FEC_InvoiceRequests>();
            this.DocumentiItemsClone = new ObservableCollection<FEC_InvoiceRequests>();
        }

        public ObservableCollection<FEC_InvoiceRequests> DocumentiItems { get; set; }
        public ObservableCollection<FEC_InvoiceRequests> DocumentiItemsClone { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class DaPassareBenzinaiWrapper
    {
        public string BenzinaioParam { get; set; } = "";
    }
}