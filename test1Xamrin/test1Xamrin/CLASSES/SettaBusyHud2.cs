using Syncfusion.SfBusyIndicator.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace xFatturazioneAttiva
{
    //non usato
    public class SettaBusyHud2
    {
        private ContentPage myPage = null;
        private View oldContent = null;
        private SfBusyIndicator busyindicator = null;

        public SettaBusyHud2(ContentPage _page)
        {
            this.myPage = _page;

        }


        public void SettaVisible(Color _color, string msg = "Attendere, operazione in corso...")
        {
            this.oldContent = this.myPage.Content;
            this.busyindicator = new SfBusyIndicator();
            this.busyindicator.AnimationType = AnimationTypes.Battery;
            this.busyindicator.Title = msg;
            this.busyindicator.TextSize = 20;
            if (_color == null)
                this.busyindicator.TextColor = Color.Brown;
            else
                this.busyindicator.TextColor = _color;
            busyindicator.ViewBoxWidth = 100;
            busyindicator.ViewBoxHeight = 100;

            this.myPage.Content = busyindicator;
            this.busyindicator.IsBusy = true;
        }

        public void settaInvisible()
        {
            this.busyindicator.IsBusy = false;
            this.myPage.Content = oldContent;
        }


    }
}
