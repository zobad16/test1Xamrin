using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test1Xamrin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Clienti : ContentPage
    {
        ClientiViewModel myViewModel { get; set; }
        public Boolean isLoaded { get; set; }
        public Clienti()
        {
            InitializeComponent();
            this.myViewModel = new ClientiViewModel(this.Navigation);
            this.BindingContext = this.myViewModel;
        }
    }
}