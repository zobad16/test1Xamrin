using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.ConversationalUI;
using test1Xamrin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SabicomChatMain : ContentPage
    {


        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var resulttext = await SabicomRepeatBOTService.SendMessage2("alessandro", "Cancel");
            TextMessage textMessage = new TextMessage();
            textMessage.Data = resulttext.Text;
            textMessage.Author = new Author { Name = "botty", Avatar = "girl.png" }; ;
            textMessage.Text = resulttext.Text;


            ((this.myChat as SabicomChatView).Content as RadChat).Items.Add(textMessage);

            await DisplayAlert("Messaggio", "Chat chiusa", "Ok");
            await this.Navigation.PopAsync();

        }

        private MainViewModel _model = new MainViewModel();
        public SabicomChatMain()
        {
            InitializeComponent();
            this.BindingContext = _model;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (this.isAppeared == false)
            {
                await Setup();
                this.isAppeared = true;
            }
            //(this.myChat as SabicomChatView).chatEntry.WatermarkText = "abc";

            //var c = this.myChat.Content;
            //var child1 = (c as TemplatedView).Children;

            //await _model.StartConversation();
            //var abc = await SendMessage2("alessandro", "reset");
        }

        //private string conversationId;
        //private string token;
        //private HttpClient _httpClient;
        public static string conversationId;
        private string token;
        public static HttpClient _httpClient;
        public Boolean isAppeared = false;
        //public static Boolean isLocal = false;
        public static string URL = "";
        public async Task<bool> Setup()
        {
            _httpClient = new HttpClient();

            URL = "https://directline.botframework.com/";
            _httpClient.Timeout = new TimeSpan(0, 2, 0);

            _httpClient.BaseAddress = new Uri(URL);

            var bearer = App.K_BearerChatBot; //"_QHpY260YAA.cwA.-pc.hEy9dsYA7Z0DvwYTcexrJOMOHKzRLMXROTqV_Pwjkuk")
            //_QHpY260YAA.cwA.-pc.hEy9dsYA7Z0DvwYTcexrJOMOHKzRLMXROTqV_Pwjkuk

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            //_QHpY260YAA.cwA.-pc.hEy9dsYA7Z0DvwYTcexrJOMOHKzRLMXROTqV_Pwjkuk
            var response = await _httpClient.PostAsync("/api/tokens/conversation", null);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<string>(result.Result);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await _httpClient.PostAsync("/api/conversations", null);
                if (response.IsSuccessStatusCode)
                {
                    var conversationInfo = await response.Content.ReadAsStringAsync();
                    conversationId = JsonConvert.DeserializeObject<Framework.Conversation>(conversationInfo).ConversationId;

                    this.myChat.settaTypingIndicator();
                    var resulttext = await SabicomRepeatBOTService.SendMessage2("alessandro", "start");
                    if (resulttext == null)
                    {
                        await erroreconnessione();
                        return false;
                    }

                    //chat.Items.Add(textMessage);

                    var localQRManagement = true;
                    if (localQRManagement == false)
                    {
                        TextMessage textMessage = new TextMessage();
                        textMessage.Data = resulttext.Text;
                        textMessage.Author = new Author { Name = "botty", Avatar = "girl.png" };
                        textMessage.Text = resulttext.Text;

                        ((this.myChat as SabicomChatView).Content as RadChat).Items.Add(textMessage); //Hey Welcome to search chatbot.
                        myChat.settaTypingIndicator();
                        var resulttext1 = await SabicomRepeatBOTService.SendMessage2("alessandro", "qr");
                        return true;


                    }


                    if (localQRManagement == true)
                    {
                        TextMessage textMessage = new TextMessage();
                        textMessage.Data = resulttext.Text;
                        textMessage.Author = new Author { Name = "botty", Avatar = "girl.png" };
                        textMessage.Text = resulttext.Text;

                        ((this.myChat as SabicomChatView).Content as RadChat).Items.Add(textMessage); //Hey Welcome to search chatbot.
                        this.myChat.settaTypingIndicator();
                        var resulttext1 = await SabicomRepeatBOTService.SendMessage2("alessandro", "qrlocal");
                        return true;


                    }


                }



                return true;
            }
            return false;
        }




        public async Task erroreconnessione()
        {
            TextMessage textMessage = new TextMessage();
            textMessage.Data = "Errore di connessione";
            textMessage.Author = new Author { Name = "botty", Avatar = "girl.png" }; ;
            textMessage.Text = "Errore di connessione";

            ((this.myChat as SabicomChatView).Content as RadChat).Items.Add(textMessage);

            await DisplayAlert("Messaggio", "Chat is closed", "Ok");

        }

        //public async Task<BotMessage> SendMessage2(string name, string message)
        //{
        //    var messageToSend = new BotMessage() { From = name, Text = message };
        //    var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
        //    var conversationUrl = SabicomChatMain.URL + "api/conversations/" + conversationId + "/messages/";

        //    var response = await _httpClient.PostAsync(conversationUrl, contentPost);

        //    var messagesReceived = await _httpClient.GetAsync(conversationUrl);
        //    var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
        //    var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
        //    var messages = messagesRoot.Messages;
        //    return messages.Last();
        //}





    }
}