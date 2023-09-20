using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProvincialCell : ContentView
    {
        public ProvincialCell()
        {
            InitializeComponent();
        }
        void Handle_BindingContextChanged(object sender, System.EventArgs e)
        {

        }
    }
}