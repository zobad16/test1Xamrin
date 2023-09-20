using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.ConversationalUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace test1Xamrin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TelerikChatView : ContentView
    {

        private RepeatBotService botService;
        private Author botAuthor;

        public TelerikChatView()
        {
            InitializeComponent();
            this.botService = new RepeatBotService();
            this.botService.AttachOnReceiveMessage(this.OnBotMessageReceived);
            this.botAuthor = new Author { Name = "botty", Avatar = "girl.png" };
            chat.Author.Avatar = "icon.png";

            ((INotifyCollectionChanged)this.chat.Items).CollectionChanged += ChatItems_CollectionChanged; ;
        }
        // << chat-getting-started-initiliaze
        // >> chat-getting-started-events
        private void ChatItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems[0] is TextMessage)
                {
                    TextMessage chatMessage = (TextMessage)e.NewItems[0];
                    if (chatMessage.Author == chat.Author)
                    {
                        if (!chatMessage.Text.StartsWith("IMG-Path:"))
                            this.botService.SendToBot(chatMessage.Text);
                    }
                }
            }
        }
        private void OnBotMessageReceived(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TextMessage textMessage = new TextMessage();
                textMessage.Data = message;
                textMessage.Author = this.botAuthor;
                textMessage.Text = message;


                //chat.Items.Add(textMessage);
                //return;
                if (message.Contains("Options"))
                {
                    var arrMessage = message.Split(new char[] { '\n', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var lMessage = new List<string>(arrMessage);
                    lMessage.RemoveAt(0); //remove header
                    lMessage.RemoveAt(0); //remove Options
                    var header = arrMessage[0];
                    ItemPickerContext context = new ItemPickerContext
                    {
                        ItemsSource = lMessage //new List<string>() { "2 days", "5 days", "7 days", "Another period" }
                    };
                    PickerItem pickerItem = new PickerItem { Context = context, HeaderText = header };

                    chat.Items.Add(pickerItem);
                    context.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == "SelectedItem")
                        {
                            if (context.SelectedItem != null)
                            {
                                chat.Items.Remove(pickerItem);
                                chat.Items.Add(new TextMessage { Author = chat.Author, Text = "" + context.SelectedItem });
                            }
                        }
                    };

                }
                else if (message.Contains("petrol receipt"))
                {
                    ItemPickerContext context = new ItemPickerContext
                    {
                        ItemsSource = new List<string>() { "Browse Image", "Capture Image" }
                    };
                    PickerItem pickerItem = new PickerItem { Context = context, HeaderText = "Click Below to add image" };

                    chat.Items.Add(textMessage);
                    chat.Items.Add(pickerItem);
                    context.PropertyChanged += async (s, e) =>
                    {
                        if (e.PropertyName == "SelectedItem")
                        {
                            Plugin.Media.Abstractions.MediaFile file = null;
                            if (context.SelectedItem != null)
                            {
                                if (context.SelectedItem.ToString() == "Browse Image")
                                    file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                                    {
                                        PhotoSize = PhotoSize.Medium,
                                        CompressionQuality = 60

                                    });
                                else
                                {
                                    file = await CrossMedia.Current.TakePhotoAsync(
                                            new Plugin.Media.Abstractions.StoreCameraMediaOptions
                                            {
                                                SaveToAlbum = true,
                                                PhotoSize = PhotoSize.Medium,
                                                CompressionQuality = 60

                                            });

                                }
                                var myStream = file.GetStream();
                                var mystreamcont = new StreamContent(myStream);

                                //      https://docs.microsoft.com/en-us/azure/bot-service/rest-api/bot-framework-rest-direct-line-3-0-api-reference?view=azure-bot-service - 3.0#upload-send-files

                                chat.Items.Remove(pickerItem);
                                chat.Items.Add(new TextMessage { Author = chat.Author, Text = "IMG-Path:" + file.Path });

                                var mybotmsg = await RepeatBotService.SendMessage3("Ales", "Image Upload", mystreamcont);
                            }
                        }
                    };
                }
                else
                    chat.Items.Add(textMessage);
            });
        }
        // << chat-getting-started-events
    }
}