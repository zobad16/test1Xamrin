using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageZoomer : MR.Gestures.ContentPage
    {
        public ImageZoomer()
        {
            InitializeComponent();
        }
        public ImageZoomer(byte[] arrBytes)
        {
            InitializeComponent();
            MemoryStream ms = new MemoryStream(arrBytes);
            ms.Seek(0, SeekOrigin.Begin);
            myimg.Source = ImageSource.FromStream(() => ms);
            myimg.VerticalOptions = LayoutOptions.FillAndExpand;
            myimg.Aspect = Aspect.AspectFit;
        }
    }
}