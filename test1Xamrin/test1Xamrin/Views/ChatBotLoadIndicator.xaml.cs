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
    public partial class ChatBotLoadIndicator : ContentPage
    {
        public ChatBotLoadIndicator()
        {
            InitializeComponent();
        }
        public void SettaLabel(string label)
        {
            this.lblChatBotIndicator.Text = label;
        }

        public void SettaBackgroundColor(Color backgroundcolor)
        {
            this.slChatBotIndicator.BackgroundColor = backgroundcolor;
        }

        public void SettaTextColor(Color textcolor)
        {
            this.lblChatBotIndicator.TextColor = textcolor;
        }
    }
}