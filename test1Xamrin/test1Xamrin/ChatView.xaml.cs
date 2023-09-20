using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatView : ListView
    {
        public ChatView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty MessagesProperty = BindableProperty.Create(
                nameof(Messages),
                typeof(string),
                typeof(Label),
                string.Empty);

        private string _messages;
        public string Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }
    }
}
