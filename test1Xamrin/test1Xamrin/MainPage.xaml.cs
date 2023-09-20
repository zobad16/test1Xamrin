using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using test1Xamrin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace test1Xamrin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        private MainViewModel _model = new MainViewModel();
        public MainPage()
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
        public async Task<bool> Setup()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://directline.botframework.com/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "c20kxmWT-mE.cwA.t-E.YSOhW2r2-xOMsjSK0Ryg5Vs6pVlKp4dGtR3Ylky3cEw");
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
                    return true;
                }


                return true;
            }
            return false;
        }

        public async Task<BotMessage> SendMessage2(string name, string message)
        {
            var messageToSend = new BotMessage() { From = name, Text = message };
            var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
            var conversationUrl = "https://directline.botframework.com/api/conversations/" + conversationId + "/messages/";

            var response = await _httpClient.PostAsync(conversationUrl, contentPost);

            var messagesReceived = await _httpClient.GetAsync(conversationUrl);
            var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
            var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
            var messages = messagesRoot.Messages;
            return messages.Last();
        }





    }
    public class BotMessage
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public DateTime Created { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public string ChannelData { get; set; }
        public string[] Images { get; set; }
        public Attachment[] Attachments { get; set; }
        public string ETag { get; set; }
    }

    public class Attachment
    {
        public string Url { get; set; }
        public string ContentType { get; set; }
    }
    public class BotMessageRoot
    {
        public List<BotMessage> Messages { get; set; }
    }

}