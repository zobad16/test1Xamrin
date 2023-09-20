using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace test1Xamrin
{
    public class RepeatBotService
    {
        private static Action<string> onReceiveMessage;
        internal void AttachOnReceiveMessage(Action<string> onMessageReceived)
        {
            onReceiveMessage = onMessageReceived;

        }
        internal async void SendToBot(string text)
        {
            var botreply = new BotMessage();
            //if(text.StartsWith("IMG-Path:")){
            //    botreply = await SendMessage3("alessandro", text.TrimStart("IMG-Path:".ToCharArray()), mystream);
            //}
            //else
            botreply = await SendMessage2("alessandro", text);
            //20190101onReceiveMessage?.Invoke(botreply.Text);

            //Task.Delay(500).ContinueWith(t => this.onReceiveMessage?.Invoke(text));
        }

        public async Task<BotMessage> SendMessage2(string name, string message)
        {
            var messageToSend = new BotMessage() { From = name, Text = message };
            var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
            var conversationUrl = "https://directline.botframework.com/api/conversations/" + MainPage.conversationId + "/messages/";

            var response = await MainPage._httpClient.PostAsync(conversationUrl, contentPost);

            var messagesReceived = await MainPage._httpClient.GetAsync(conversationUrl);
            var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
            var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
            var messages = messagesRoot.Messages;
            return messages.Last();


        }


        public static async Task<BotMessage> SendMessage3(string name, string message, StreamContent mystream)
        {

            var messageToSend = new BotMessage() { From = name, Text = message };
            var att = new Attachment()
            {
                Url = message,
                ContentType = "jpeg"

            };
            List<Attachment> latt = new List<Attachment>();
            messageToSend.Attachments = latt.ToArray();

            //var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
            var conversationUrl = "https://directline.botframework.com/api/conversations/" + MainPage.conversationId + "/upload?userId=alessandro";


            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent("fileToUpload");
            //form.Add(content, "fileToUpload");


            //var stream = await file.OpenStreamForReadAsync();
            //content = new StreamContent(stream);
            content = mystream;
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "fileToUpload",
                FileName = "Test Image"
            };

            content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            form.Add(content);


            try
            {


                var response = await MainPage._httpClient.PostAsync(conversationUrl, form);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //var b = 0;
            }
            conversationUrl = "https://directline.botframework.com/api/conversations/" + MainPage.conversationId + "/messages/";

            var messagesReceived = await MainPage._httpClient.GetAsync(conversationUrl);
            var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
            var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
            var messages = messagesRoot.Messages;
            var botReply = messages.Last();
            onReceiveMessage?.Invoke(botReply.Text);
            return botReply;

        }

    }
}
